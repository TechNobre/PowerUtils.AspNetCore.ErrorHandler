using System.IO;
using System.Net.Http;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Utils
{
    internal sealed class FileUtils
    {
        public static StreamContent LoadFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            var bytes = File.ReadAllBytes(filePath);
            return new StreamContent(new MemoryStream(bytes));
        }
    }
}
