using DiscountCodeSystem.Server.Networking;
using DiscountCodeSystem.Server.Services;
using DiscountCodeSystem.Server.Storage;

Console.WriteLine("Starting server on port 5000...");

var storage = new FileStorageService();
var generator = new CodeGeneratorService(storage);
var usage = new CodeUsageService(storage);

var server = new TcpServer(generator, usage);
server.Start(); // Start block and keep runnign
