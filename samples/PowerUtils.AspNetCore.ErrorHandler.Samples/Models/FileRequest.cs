using Microsoft.AspNetCore.Http;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Models;

public class FileRequest
{
    public IFormFile File { get; set; }
}
