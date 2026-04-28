using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace FirstAiCall;

/// <summary>
/// Demonstrates Microsoft Semantic Kernel with Ollama's OpenAI-compatible endpoint.
/// The app selects a local model, sends a prompt, and streams the response.
/// </summary>
public class Program
{
	static readonly Dictionary<string, string> Models = new()
	{
		{ "1", "phi4" },
		{ "2", "qwen2.5-coder:14b" },
	};

	static async Task Main()
	{
		Console.WriteLine("Select Model:");
		Console.WriteLine("  [1] phi4             (reasoning)");
		Console.WriteLine("  [2] qwen2.5-coder    (code generation)");
		Console.Write("\nYour choice: ");

		var choice = Console.ReadLine()?.Trim() ?? "1";
		var modelId = Models.GetValueOrDefault(choice, "phi4");
		Console.WriteLine($"\nUsing: {modelId}\n");

		var kernel = Kernel.CreateBuilder()
			.AddOpenAIChatCompletion(
				modelId: modelId,
				endpoint: new Uri("http://localhost:11434/v1"),
				apiKey: "ollama",
				httpClient: new HttpClient { Timeout = TimeSpan.FromMinutes(10) })
			.Build();

		var chatService = kernel.GetRequiredService<IChatCompletionService>();
		var chatMessage = "You are a senior .NET engineer. Explain how to design a clean Todo API with dependency injection, validation, persistence, and tests.";
		Console.WriteLine($"Question: {chatMessage}");

		var history = new ChatHistory();
		history.AddUserMessage(chatMessage);

		Console.WriteLine("\nStreaming response...\n");

		await foreach (var token in chatService.GetStreamingChatMessageContentsAsync(history))
		{
			Console.Write(token.Content);
		}

		Console.WriteLine("\n\nDone.");
	}
}
