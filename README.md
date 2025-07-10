# DiscountApp

A lightweight discount code generation and usage system with persistent storage and TCP-based communication. Developed in **C#** using **.NET 8**, it avoids REST APIs and supports concurrent requests. Designed for a technical evaluation task.

---

## Features

- ✅ Random, unique discount code generation
- ✅ Code usage (activation/consumption)
- ✅ Persistent storage using files (data survives service restarts)
- ✅ TCP socket communication (not REST)
- ✅ Supports concurrent requests (multi-threading)
- ✅ Unit tests using xUnit
- ✅ Sample TCP client provided

---

## Project Structure

DiscountApp/
├── DiscountCodeSystem.Server/ # Server-side logic
│ ├── Models/ # DiscountCode model
│ ├── Services/ # Business logic (generation & usage)
│ ├── Storage/ # File-based persistence
│ └── TcpServer.cs # TCP server implementation
│
├── DiscountCodeSystem.Client/ # Sample TCP client (optional)
│ └── TcpClientSample.cs # Client that communicates with server
│
├── DiscountCodeSystem.Tests/ # Unit tests
│ └── *.cs # Test classes for services
│
├── DiscountCodeSystem.sln # Visual Studio solution
└── README.md # This file


---

## How to Run

### Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download)
- Git (optional)
- PowerShell (for automation scripts, optional)

---

To run Project, it's runing by to separated builds, I did my tests running on PowerShell

### Run the Server

```bash
cd Server
dotnet run


### Run the Client

```bash
cd Client
dotnet run


NOTE
I have created the dockerfiles and docker-compose, but to make it run correctly I would need to change my connection format,
since I already have the application running correctly and I'm keeping the codes on a json file I didn't saw the need to create the docker structure.