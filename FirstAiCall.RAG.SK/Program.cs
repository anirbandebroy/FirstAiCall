using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace FirstAiCall.RAG.SK;

public class Program
{
	static async Task Main()
	{
		var kernel = Kernel.CreateBuilder()
			.AddOpenAIChatCompletion(
				modelId: "phi4",
				endpoint: new Uri("http://localhost:11434/v1"),
				apiKey: "ollama",
				httpClient: new HttpClient { Timeout = TimeSpan.FromMinutes(10) })
			.AddOllamaEmbeddingGenerator(
				modelId: "nomic-embed-text",
				endpoint: new Uri("http://localhost:11434"))
			.Build();

		Console.WriteLine("Kernel ready!");

		var chatService = kernel.GetRequiredService<IChatCompletionService>();
		var embedder = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

		Console.WriteLine("Chat service ready!");
		Console.WriteLine("Embedder ready!");

		var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
		var chunks = new List<(string FileName, string Text, float[] Embedding)>();

		foreach (var file in Directory.GetFiles(dataPath, "*.md"))
		{
			var fileName = Path.GetFileName(file);
			var content = await File.ReadAllTextAsync(file);

			var lines = content
				.Split("\n", StringSplitOptions.RemoveEmptyEntries)
				.Select(l => l.Trim())
				.Where(l => l.Length > 10)
				.ToList();

			var count = 0;

			for (var i = 0; i < lines.Count; i += 2)
			{
				var window = lines.Skip(i).Take(3);
				var text = string.Join(" ", window).Trim();

				if (text.Length < 50) continue;

				var result = await embedder.GenerateAsync([text]);
				var vector = result[0].Vector.ToArray();
				chunks.Add((fileName, text, vector));
				count++;
			}

			Console.WriteLine($"  {fileName} - {count} chunks embedded");
		}

		Console.WriteLine($"\nTotal chunks ready: {chunks.Count}");
		Console.WriteLine("\nRAG ready! Ask anything about the sample Contoso documents.");
		Console.WriteLine("Type 'exit' to quit.\n");

		while (true)
		{
			Console.Write("You: ");
			var question = Console.ReadLine()?.Trim();
			if (string.IsNullOrEmpty(question) || question.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

			var qResult = await embedder.GenerateAsync([question]);
			var qVector = qResult[0].Vector.ToArray();

			var topChunks = chunks
				.Select(c => (Chunk: c, Score: CosineSimilarity(qVector, c.Embedding)))
				.OrderByDescending(x => x.Score)
				.Take(5)
				.ToList();

			Console.WriteLine("\n[DEBUG - Top 5 chunks retrieved:]");
			foreach (var (chunk, score) in topChunks)
				Console.WriteLine($"  Score: {score:F2} | {chunk.FileName} | {chunk.Text[..Math.Min(80, chunk.Text.Length)]}");
			Console.WriteLine();

			var context = string.Join("\n---\n", topChunks.Select(x => x.Chunk.Text));

			var history = new ChatHistory();
			history.AddSystemMessage(
				"You are a helpful assistant answering questions about fictional Contoso product and support documents. " +
				"The context below is extracted from local markdown files. " +
				"Answer from the context and say when the context does not contain enough information.");

			history.AddUserMessage($"Context:\n{context}\n\nQuestion: {question}");

			Console.Write("\nphi4: ");
			await foreach (var token in chatService.GetStreamingChatMessageContentsAsync(history))
				Console.Write(token.Content);

			Console.WriteLine("\n");
		}
	}

	static float CosineSimilarity(float[] a, float[] b)
	{
		float dot = 0, magA = 0, magB = 0;

		for (var i = 0; i < a.Length; i++)
		{
			dot += a[i] * b[i];
			magA += a[i] * a[i];
			magB += b[i] * b[i];
		}

		return dot / (MathF.Sqrt(magA) * MathF.Sqrt(magB));
	}
}
