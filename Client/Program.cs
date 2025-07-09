using System.Net.Sockets;
using DiscountCodeSystem.Client.Utils;

while (true)
{
    Console.Clear();
    Console.WriteLine("==== Discount Code System ====");
    Console.WriteLine("[1] Generate codes");
    Console.WriteLine("[2] Use code");
    Console.WriteLine("[3] List codes");
    Console.WriteLine("[0] Exit");
    Console.Write("Choose: ");

    var input = Console.ReadLine();
    if (input == "0")
        break;

    try
    {
        using var client = new TcpClient();
        await client.ConnectAsync("127.0.0.1", 5000);
        using var stream = client.GetStream();
        using var writer = new BinaryWriter(stream);
        using var reader = new BinaryReader(stream);

        if (input == "1")
        {
            Console.Write("How many codes do you want to generate?");
            ushort count = ushort.Parse(Console.ReadLine() ?? "1");

            Console.Write("Size of each code (8 recommended):");
            byte length = byte.Parse(Console.ReadLine() ?? "8");

            writer.Write((byte)1);
            writer.Write(count);
            writer.Write(length);

            bool result = reader.ReadBoolean();
            Console.WriteLine(result ? "[SUCCESS] Generated codes:" : "[ERROR] Generation failed");

            if (result)
            {
                List<string> codes = new();
                for (int i = 0; i < count; i++)
                {
                    var code = CodeProtocolHelper.ReadCode(reader);
                    codes.Add(code);
                }

                foreach (var code in codes)
                {
                    Console.WriteLine($" > {code}");
                }
            }
        }
        else if (input == "2")
        {
            Console.Write("Enter the code to use: ");
            string code = Console.ReadLine()?.Trim() ?? "";

            if (code.Length < 7 || code.Length > 8)
            {
                Console.WriteLine("Invalid code. Must be between 7 and 8 characters long.");
            }
            else
            {
                writer.Write((byte)2);
                CodeProtocolHelper.WriteCode(writer, code);

                bool result = CodeProtocolHelper.ReadResult(reader);
                Console.WriteLine(result ? "[OK] Code used successfully!" : "[X] Invalid or already used code.");
            }
        }
        else if (input == "3")
        {
            writer.Write((byte)3);

            int count = reader.ReadInt32();
            Console.WriteLine($"[INFO] Total codes: {count}");

            for (int i = 0; i < count; i++)
            {
                string code = CodeProtocolHelper.ReadCode(reader);
                bool used = reader.ReadBoolean();
                Console.WriteLine($" > {code} | {(used ? "USED" : "AVAILABLE")}");
            }
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] {ex.Message}");
    }

    Console.WriteLine("\nPress ENTER to continue...");
    Console.ReadLine();
}
