# FirstAiCall - Local LLM and RAG POC

This repository is a proof of concept for running LLM workflows locally from .NET console applications. It starts with a basic Semantic Kernel chat call to Ollama, then expands into Retrieval-Augmented Generation (RAG) using local markdown files, embeddings, vector similarity, and two different .NET AI orchestration libraries.

The goal is to showcase how a .NET application can talk to a local LLM, stream responses, convert documents into embeddings, retrieve relevant context, and generate grounded answers from private data without sending prompts or documents to a cloud AI provider.

## What This POC Demonstrates

- Local LLM integration with Ollama's OpenAI-compatible API.
- Streaming chat responses from a .NET console app.
- Runtime model selection between reasoning and coding models.
- RAG over local markdown documents.
- Embedding generation with `nomic-embed-text`.
- In-memory vector storage and cosine similarity search.
- Two RAG implementations for comparison: Microsoft Semantic Kernel and LangChain for .NET.

## Solution Structure

```text
FirstAiCall/
|-- FirstAiCall.slnx
|-- FirstAiCall/
|   |-- FirstAiCall.csproj
|   `-- Program.cs
|-- FirstAiCall.RAG.SK/
|   |-- FirstAiCall.RAG.SK.csproj
|   |-- Program.cs
|   `-- Data/
|       |-- AI-Learning-Notes.md
|       `-- AI_Career_Path.md
`-- FirstAiCall.RAG.LC/
    |-- FirstAiCall.RAG.LC.csproj
    |-- Program.cs
    |-- RagPipeline.cs
    `-- Data/
        |-- AI-Learning-Notes.md
        `-- AI_Career_Path.md
```

## Projects

| Project | Purpose | Main Libraries |
| --- | --- | --- |
| `FirstAiCall` | Basic local LLM chat call with streaming output and model selection. | Semantic Kernel, Ollama connector |
| `FirstAiCall.RAG.SK` | RAG implementation using Semantic Kernel services for chat and embeddings. | Semantic Kernel, Microsoft.Extensions.AI |
| `FirstAiCall.RAG.LC` | RAG implementation using LangChain for .NET with a reusable `RagPipeline`. | LangChain, LangChain Ollama provider |

## Prerequisites

- .NET SDK that supports `net10.0`.
- Ollama installed and running locally.
- Required local models pulled into Ollama:

```powershell
ollama pull phi4
ollama pull qwen2.5-coder:14b
ollama pull nomic-embed-text
```

## How To Run

```powershell
dotnet restore
dotnet build
dotnet run --project .\FirstAiCall\FirstAiCall.csproj
dotnet run --project .\FirstAiCall.RAG.SK\FirstAiCall.RAG.SK.csproj
dotnet run --project .\FirstAiCall.RAG.LC\FirstAiCall.RAG.LC.csproj
```

## Demo Questions

```text
What roles are available in Contoso Hub?
Can viewers export billing reports?
What does the weekly summary include?
How should sign-in problems be handled?
When should a support issue be escalated?
```

## Current Limitations

- Vector data is stored only in memory.
- Documents are re-indexed on every run.
- Chunking is simple and optimized for learning clarity.
- Console apps only; no API or UI layer yet.

## Tech Stack

- C#
- .NET `net10.0`
- Microsoft Semantic Kernel
- Microsoft.Extensions.AI
- LangChain for .NET
- Ollama
- `phi4`
- `qwen2.5-coder:14b`
- `nomic-embed-text`
