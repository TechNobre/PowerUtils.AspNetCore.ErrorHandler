using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Json;
using PowerUtils.AspNetCore.ErrorHandler.Serializers;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests.Serializers
{
    public class ErrorProblemDetailsJsonConverterTests
    {
        private static JsonSerializerOptions _jsonSerializerOptions => new JsonOptions().SerializerOptions;

        [Fact]
        public void AllPropertiesFilled_Read_ErrorProblemDetails()
        {
            // Arrange
            var type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
            var title = "Not found";
            var status = 404;
            var detail = "Product not found";
            var instance = "http://example.com/products/14";
            var traceId = Guid.NewGuid().ToString();

            var error1 = new KeyValuePair<string, string>("key2", "error230");
            var error2 = new KeyValuePair<string, string>("key0", "error00");

            var json = $"{{\"type\":\"{type}\",\"title\":\"{title}\",\"status\":{status},\"detail\":\"{detail}\", \"instance\":\"{instance}\",\"traceId\":\"{traceId}\"," +
                $"\"errors\": {{\"{error1.Key}\":\"{error1.Value}\",\"{error2.Key}\":\"{error2.Value}\"}}}}";

            var converter = new ErrorProblemDetailsJsonConverter();
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            reader.Read();


            // Act
            var act = converter.Read(ref reader, typeof(ErrorProblemDetails), _jsonSerializerOptions);


            // Assert
            act.Type.Should().Be(type);
            act.Title.Should().Be(title);
            act.Status.Should().Be(status);
            act.Detail.Should().Be(detail);
            act.Instance.Should().Be(instance);
            act.TraceId.Should().Be(traceId);


            act.Errors.Should().HaveCount(2);

            act.Errors.Should().ContainKey(error1.Key);
            act.Errors.Should().ContainKey(error2.Key);

            act.Errors.Should().ContainValue(error1.Value);
            act.Errors.Should().ContainValue(error2.Value);
        }

        [Fact]
        public void WithSomeMissingValues_Read_ErrorProblemDetails()
        {
            // Arrange
            var type = "https://Faker.com/html/rfc9110#section-15.5.5";
            var title = "FakeError";
            var traceId = Guid.NewGuid().ToString();

            var error1 = new KeyValuePair<string, string>("key2", "error230");
            var error2 = new KeyValuePair<string, string>("key0", "error00");
            var error3 = new KeyValuePair<string, string>("key4", "error45");

            var json = $"{{\"type\":\"{type}\",\"title\":\"{title}\",\"status\":null,\"traceId\":\"{traceId}\"," +
                $"\"errors\": {{\"{error1.Key}\":\"{error1.Value}\",\"{error2.Key}\":\"{error2.Value}\",\"{error3.Key}\":\"{error3.Value}\"}}}}";

            var converter = new ErrorProblemDetailsJsonConverter();
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            reader.Read();

            // Act
            var act = converter.Read(ref reader, typeof(ErrorProblemDetails), _jsonSerializerOptions);


            // Assert
            act.Type.Should().Be(type);
            act.Title.Should().Be(title);
            act.Status.Should().BeNull();
            act.Detail.Should().BeNull();
            act.Instance.Should().BeNull();
            act.TraceId.Should().Be(traceId);


            act.Errors.Should().HaveCount(3);

            act.Errors.Should().ContainKey(error1.Key);
            act.Errors.Should().ContainKey(error2.Key);
            act.Errors.Should().ContainKey(error3.Key);

            act.Errors.Should().ContainValue(error1.Value);
            act.Errors.Should().ContainValue(error2.Value);
            act.Errors.Should().ContainValue(error3.Value);
        }

        [Fact]
        public void AllPropertiesFilled_Deserialize_ErrorProblemDetails()
        {
            // Arrange
            var type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
            var title = "Not found";
            var status = 404;
            var detail = "Product not found";
            var instance = "http://example.com/products/14";
            var traceId = Guid.NewGuid().ToString();

            var error1 = new KeyValuePair<string, string>("key2", "error230");
            var error2 = new KeyValuePair<string, string>("key0", "error00");

            var json = $"{{\"type\":\"{type}\",\"title\":\"{title}\",\"status\":{status},\"detail\":\"{detail}\", \"instance\":\"{instance}\",\"traceId\":\"{traceId}\"," +
                $"\"errors\": {{\"{error1.Key}\":\"{error1.Value}\",\"{error2.Key}\":\"{error2.Value}\"}}}}";


            // Act
            var act = JsonSerializer.Deserialize<ErrorProblemDetails>(json, _jsonSerializerOptions);


            // Assert
            act.Type.Should().Be(type);
            act.Title.Should().Be(title);
            act.Status.Should().Be(status);
            act.Detail.Should().Be(detail);
            act.Instance.Should().Be(instance);
            act.TraceId.Should().Be(traceId);


            act.Errors.Should().HaveCount(2);

            act.Errors.Should().ContainKey(error1.Key);
            act.Errors.Should().ContainKey(error2.Key);

            act.Errors.Should().ContainValue(error1.Value);
            act.Errors.Should().ContainValue(error2.Value);
        }

        [Fact]
        public void AllNull_Serialize_JsonString()
        {
            // Arrange
            var problemDetails = new ErrorProblemDetails();


            // Act
            var json = JsonSerializer.Serialize(problemDetails);
            var act = JsonSerializer.Deserialize<ErrorProblemDetails>(json, _jsonSerializerOptions);


            // Assert
            act.Type.Should().BeNull();
            act.Title.Should().BeNull();
            act.Status.Should().BeNull();
            act.Detail.Should().BeNull();
            act.Instance.Should().BeNull();
            act.TraceId.Should().BeNull();


            act.Errors.Should().HaveCount(0);

            act.Extensions.Should().HaveCount(0);
        }

        [Fact]
        public void AllPropertiesFilled_Serialize_JsonString()
        {
            // Arrange
            var traceId = Guid.NewGuid().ToString();
            var problemDetails = new ErrorProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "Not found",
                Status = 404,
                Detail = "Product not found",
                Instance = "http://example.com/products/14",
                TraceId = traceId,
                Errors = new Dictionary<string, string>()
                {
                    ["Erro2"] = "ErrrorValue",
                    ["PropError"] = "ErrrorProrp",
                }
            };


            // Act
            var json = JsonSerializer.Serialize(problemDetails);
            var act = JsonSerializer.Deserialize<ErrorProblemDetails>(json, _jsonSerializerOptions);


            // Assert
            act.Type.Should().Be(problemDetails.Type);
            act.Title.Should().Be(problemDetails.Title);
            act.Status.Should().Be(problemDetails.Status);
            act.Detail.Should().Be(problemDetails.Detail);
            act.Instance.Should().Be(problemDetails.Instance);
            act.TraceId.Should().Be(problemDetails.TraceId);


            act.Errors.Should().HaveCount(2);

            act.Errors.Should().ContainKey(problemDetails.Errors.First().Key);
            act.Errors.Should().ContainKey(problemDetails.Errors.Last().Key);

            act.Errors.Should().ContainValue(problemDetails.Errors.First().Value);
            act.Errors.Should().ContainValue(problemDetails.Errors.Last().Value);

            act.Extensions.Should().HaveCount(0);
        }

        [Fact]
        public void TraceIdFromExtensions_Serialize_JsonString()
        {
            // Arrange
            var traceId = Guid.NewGuid().ToString();
            var problemDetails = new ErrorProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "Not found",
                Status = 404,
                Detail = "Product not found",
                Instance = "http://example.com/products/14",
                Errors = new Dictionary<string, string>()
                {
                    ["Erro2"] = "ErrrorValue",
                    ["PropError"] = "ErrrorProrp",
                }
            };
            problemDetails.Extensions["traceId"] = traceId;


            // Act
            var json = JsonSerializer.Serialize(problemDetails);
            var act = JsonSerializer.Deserialize<ErrorProblemDetails>(json, _jsonSerializerOptions);


            // Assert
            act.Type.Should().Be(problemDetails.Type);
            act.Title.Should().Be(problemDetails.Title);
            act.Status.Should().Be(problemDetails.Status);
            act.Detail.Should().Be(problemDetails.Detail);
            act.Instance.Should().Be(problemDetails.Instance);
            act.TraceId.Should().Be(traceId);


            act.Errors.Should().HaveCount(2);

            act.Errors.Should().ContainKey(problemDetails.Errors.First().Key);
            act.Errors.Should().ContainKey(problemDetails.Errors.Last().Key);

            act.Errors.Should().ContainValue(problemDetails.Errors.First().Value);
            act.Errors.Should().ContainValue(problemDetails.Errors.Last().Value);

            act.Extensions.Should().HaveCount(0);
        }
    }
}
