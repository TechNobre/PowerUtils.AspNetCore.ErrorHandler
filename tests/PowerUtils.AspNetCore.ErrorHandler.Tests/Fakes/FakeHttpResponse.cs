using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Fakes;

public class FakeHttpResponse : HttpResponse
{
    public new PipeWriter BodyWriter { get; }
    public override HttpContext HttpContext { get; }

    public override int StatusCode { get; set; }

    public override IHeaderDictionary Headers { get; }

    public override Stream Body { get; set; }
    public override long? ContentLength { get; set; }
    public override string ContentType { get; set; }

    public override IResponseCookies Cookies => throw new NotImplementedException();

    public override bool HasStarted => false;

    public override void OnCompleted(Func<object, Task> callback, object state) => throw new NotImplementedException();
    public override void OnStarting(Func<object, Task> callback, object state) => throw new NotImplementedException();
    public override void Redirect(string location, bool permanent) => throw new NotImplementedException();

    public FakeHttpResponse()
    {
        HttpContext = null;
        Headers = new HeaderDictionary();
        Body = new MemoryStream();
        ContentType = "";
        ContentLength = 0;
    }

    public FakeHttpResponse(HttpContext httpContext)
    {
        HttpContext = httpContext;
        Headers = new HeaderDictionary();
        Body = new MemoryStream();
        ContentType = "";
        ContentLength = 0;
    }
}
