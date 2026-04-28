using LangChain.Providers;
using LangChain.Providers.Ollama;

namespace FirstAiCall.RAG.LC;

public class RagPipeline(OllamaChatModel llm, OllamaEmbeddingModel embedder)
{
	private readonly List<(string Text, float[] Embedding)> _store = new();

	public async Task AddDocumentAsync(string text)
	{
		var response = await embedder.CreateEmbeddingsAsync(
			new EmbeddingRequest { Strings = [text] });

		var embedding = response.Values[0];
		_store.Add((text, embedding));
	}

	public async Task<List<string>> RetrieveAsync(string query, int topN = 2)
	{
		var response = await embedder.CreateEmbeddingsAsync(
			new EmbeddingRequest { Strings = [query] });

		var queryEmbedding = response.Values[0];

		return _store
			.OrderByDescending(entry => CosineSimilarity(queryEmbedding, entry.Embedding))
			.Take(topN)
			.Select(entry => entry.Text)
			.ToList();
	}

	public async Task<string> AskAsync(string question)
	{
		var relevantChunks = await RetrieveAsync(question);
		var context = string.Join("\n\n", relevantChunks);

		var prompt = $"""
		              Answer the question based only on the context below.

		              Context:
		              {context}

		              Question: {question}
		              """;

		return await llm.GenerateAsync(prompt);
	}

	public async Task<List<(string Text, float Score)>> RetrieveWithScoresAsync(string query, int topN = 2)
	{
		var response = await embedder.CreateEmbeddingsAsync(
			new EmbeddingRequest { Strings = [query] });

		var queryEmbedding = response.Values[0];

		return _store
			.Select(entry => (entry.Text, Score: CosineSimilarity(queryEmbedding, entry.Embedding)))
			.OrderByDescending(x => x.Score)
			.Take(topN)
			.ToList();
	}

	private static float CosineSimilarity(float[] a, float[] b)
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
