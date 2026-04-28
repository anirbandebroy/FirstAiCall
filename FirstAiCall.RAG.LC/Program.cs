using LangChain.Providers.Ollama;

namespace FirstAiCall.RAG.LC;

class Program
{
	static async Task Main()
	{
		var provider = new OllamaProvider();

		var llm = new OllamaChatModel(provider, id: "phi4");
		Console.WriteLine("LLM ready!");

		var embedder = new OllamaEmbeddingModel(provider, id: "nomic-embed-text");
		Console.WriteLine("Embedder ready!");

		var rag = new RagPipeline(llm, embedder);

		var files = new[]
		{
			"Data/AI_Career_Path.md",
			"Data/AI-Learning-Notes.md"
		};

		foreach (var file in files)
		{
			var text = await File.ReadAllTextAsync(file);
			var chunks = ChunkText(text, 500);

			foreach (var chunk in chunks)
				await rag.AddDocumentAsync(chunk);

			Console.WriteLine($"Indexed {chunks.Count} chunks from {file}");
		}

		Console.WriteLine("\nAll documents indexed!\n");

		var question = "What roles are available in Contoso Hub?";

		var topChunks = await rag.RetrieveWithScoresAsync(question, topN: 5);
		Console.WriteLine("[DEBUG - Top 5 chunks retrieved:]");
		foreach (var (chunk, score) in topChunks)
			Console.WriteLine($"  Score: {score:F2} | {chunk[..Math.Min(80, chunk.Length)]}");
		Console.WriteLine();

		var answer = await rag.AskAsync(question);
		Console.WriteLine($"Q: {question}");
		Console.WriteLine($"A: {answer}");
	}

	static List<string> ChunkText(string text, int chunkSize)
	{
		var chunks = new List<string>();
		var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
		var current = new System.Text.StringBuilder();

		foreach (var line in lines)
		{
			if (current.Length + line.Length > chunkSize && current.Length > 0)
			{
				chunks.Add(current.ToString().Trim());
				current.Clear();
			}

			current.AppendLine(line);
		}

		if (current.Length > 0)
			chunks.Add(current.ToString().Trim());

		return chunks;
	}
}
