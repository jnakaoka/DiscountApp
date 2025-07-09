using System.Text;

namespace DiscountCodeSystem.Server.Utils;

public static class CodeProtocolHelper
{
    public static void WriteCode(BinaryWriter writer, string code)
    {
        // control to avoid null and > 255 because it was causing errors between client and server
        if (string.IsNullOrEmpty(code))
            code = "";

        if (code.Length > 255)
            code = code.Substring(0, 255); 

        byte length = (byte)code.Length;
        writer.Write(length);
        writer.Write(Encoding.ASCII.GetBytes(code));
    }

    public static string ReadCode(BinaryReader reader)
    {
        byte length = reader.ReadByte();
        byte[] codeBytes = reader.ReadBytes(length);
        return Encoding.ASCII.GetString(codeBytes);
    }

    public static void WriteResult(BinaryWriter writer, bool success)
    {
        writer.Write(success);
    }

    public static bool ReadResult(BinaryReader reader)
    {
        return reader.ReadBoolean();
    }
}