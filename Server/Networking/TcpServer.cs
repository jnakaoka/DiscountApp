using System.Net;
using System.Net.Sockets;
using System.Text;
using DiscountCodeSystem.Server.Services;
using System.Text.Json;
using DiscountCodeSystem.Server.Utils;
using System.Reflection.Emit;

namespace DiscountCodeSystem.Server.Networking;

public class TcpServer
{
    private readonly TcpListener _listener;
    private readonly CodeGeneratorService _generator;
    private readonly CodeUsageService _usage;

    public TcpServer(CodeGeneratorService generator, CodeUsageService usage, int port = 5000)
    {
        _generator = generator;
        _usage = usage;
        //_listener = new TcpListener(IPAddress.Loopback, port);
        _listener = new TcpListener(IPAddress.Any, 5000);
    }

    public void Start()
    {
        try
        {
            _listener.Start();
            Console.WriteLine("[INFO] Listener started. Waiting conection...");

            while (true)
            {
                Console.WriteLine($"[SERVER] Waiting new client on {DateTime.Now}");
                var client = _listener.AcceptTcpClient();
                Console.WriteLine("[INFO] Connected client.");
                Task.Run(() => HandleClient(client));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SERVER ERROR] {ex.Message}");
        }
    }

    private void HandleClient(TcpClient client)
    {
        using var stream = client.GetStream();
        using var reader = new BinaryReader(stream);
        using var writer = new BinaryWriter(stream);

        try
        {
            var command = reader.ReadByte();

            if (command == 1)
            {
                ushort count = reader.ReadUInt16();
                byte length = reader.ReadByte();

                Console.WriteLine($"[SERVER] Request to generate {count} code(s) with {length} characters");

                var codes = _generator.GenerateCodes(count, length);

                Console.WriteLine("[SERVER] Generated codes:");
                foreach (var code in codes)
                    Console.WriteLine($" > {code}");

                CodeProtocolHelper.WriteResult(writer, true);
                foreach (var code in codes)
                {
                    CodeProtocolHelper.WriteCode(writer, code);
                }
            }
            else if (command == 2)
            {
                string code = CodeProtocolHelper.ReadCode(reader);
                var result = _usage.UseCode(code);
                CodeProtocolHelper.WriteResult(writer, result == 0);
            }
            else if (command == 3)
            {
                var allCodes = _usage.GetAllCodes();
                writer.Write(allCodes.Count);
                Console.WriteLine($"[SERVER] Sending {allCodes.Count} codes:");

                foreach (var code in allCodes)
                {
                    try
                    {
                        CodeProtocolHelper.WriteCode(writer, code.Code);
                        writer.Write(code.Used);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERRO] Fail writing code '{code?.Code}': {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("[SERVER] Unknown command.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
    }
}
