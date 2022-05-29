using System.IO;
using System.Net.Http;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Utils;

internal class FileUtils
{
    public static StreamContent LoadFile(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        return new(new MemoryStream(bytes));
    }
}
