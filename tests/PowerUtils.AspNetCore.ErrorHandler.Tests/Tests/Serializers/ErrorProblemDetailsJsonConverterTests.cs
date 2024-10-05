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
    public sealed class ErrorProblemDetailsJsonConverterTests
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

            var error1 = new KeyValuePair<string, ErrorDetails>("key2", new("error230", "disc22"));
            var error2 = new KeyValuePair<string, ErrorDetails>("key0", new("error00", "disc 46546"));

            var json = $"{{\"type\":\"{type}\",\"title\":\"{title}\",\"status\":{status},\"detail\":\"{detail}\", \"instance\":\"{instance}\",\"traceId\":\"{traceId}\"," +
                $"\"errors\": {{\"{error1.Key}\":{{\"code\":\"{error1.Value.Code}\",\"description\":\"{error1.Value.Description}\"}}, \"{error2.Key}\":{{\"code\":\"{error2.Value.Code}\",\"description\":\"{error2.Value.Description}\"}}}}}}";

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

            act.Errors[error1.Key].Code.Should().Be(error1.Value.Code);
            act.Errors[error2.Key].Code.Should().Be(error2.Value.Code);

            act.Errors[error1.Key].Description.Should().Be(error1.Value.Description);
            act.Errors[error2.Key].Description.Should().Be(error2.Value.Description);
        }

        [Fact]
        public void WithSomeMissingValues_Read_ErrorProblemDetails()
        {
            // Arrange
            var type = "https://Faker.com/html/rfc9110#section-15.5.5";
            var title = "FakeError";
            var traceId = Guid.NewGuid().ToString();

            var error1 = new KeyValuePair<string, ErrorDetails>("key2", new("error230", "disc 1"));
            var error2 = new KeyValuePair<string, ErrorDetails>("key0", new("error00", "disc fake"));
            var error3 = new KeyValuePair<string, ErrorDetails>("key4", new("error45", "disc this"));

            var json = $"{{\"type\":\"{type}\",\"title\":\"{title}\",\"status\":null,\"traceId\":\"{traceId}\"," +
                $"\"errors\": {{\"{error1.Key}\":{{\"code\":\"{error1.Value.Code}\",\"description\":\"{error1.Value.Description}\"}}, \"{error2.Key}\":{{\"code\":\"{error2.Value.Code}\",\"description\":\"{error2.Value.Description}\"}}, \"{error3.Key}\":{{\"code\":\"{error3.Value.Code}\",\"description\":\"{error3.Value.Description}\"}}}}}}";

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

            act.Errors[error1.Key].Code.Should().Be(error1.Value.Code);
            act.Errors[error2.Key].Code.Should().Be(error2.Value.Code);
            act.Errors[error3.Key].Code.Should().Be(error3.Value.Code);

            act.Errors[error1.Key].Description.Should().Be(error1.Value.Description);
            act.Errors[error2.Key].Description.Should().Be(error2.Value.Description);
            act.Errors[error3.Key].Description.Should().Be(error3.Value.Description);
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

            var error1 = new KeyValuePair<string, ErrorDetails>("key2", new("error230", "disc v"));
            var error2 = new KeyValuePair<string, ErrorDetails>("key0", new("error00", "disc t"));

            var json = $"{{\"type\":\"{type}\",\"title\":\"{title}\",\"status\":{status},\"detail\":\"{detail}\", \"instance\":\"{instance}\",\"traceId\":\"{traceId}\"," +
                $"\"errors\": {{\"{error1.Key}\":{{\"code\":\"{error1.Value.Code}\",\"description\":\"{error1.Value.Description}\"}}, \"{error2.Key}\":{{\"code\":\"{error2.Value.Code}\",\"description\":\"{error2.Value.Description}\"}}}}}}";


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

            act.Errors[error1.Key].Code.Should().Be(error1.Value.Code);
            act.Errors[error2.Key].Code.Should().Be(error2.Value.Code);

            act.Errors[error1.Key].Description.Should().Be(error1.Value.Description);
            act.Errors[error2.Key].Description.Should().Be(error2.Value.Description);
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
                Errors = new Dictionary<string, ErrorDetails>()
                {
                    ["Erro2"] = new("ErrrorValue", "d22"),
                    ["PropError"] = new("ErrrorProrp", "d"),
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

            act.Errors.First().Value.Code.Should().Be(problemDetails.Errors.First().Value.Code);
            act.Errors.Last().Value.Code.Should().Be(problemDetails.Errors.Last().Value.Code);

            act.Errors.First().Value.Description.Should().Be(problemDetails.Errors.First().Value.Description);
            act.Errors.Last().Value.Description.Should().Be(problemDetails.Errors.Last().Value.Description);

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
                Errors = new Dictionary<string, ErrorDetails>()
                {
                    ["Erro2"] = new("ErrrorValue", "faker"),
                    ["PropError"] = new("ErrrorProrp", "maker"),
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

            act.Errors.First().Value.Code.Should().Be(problemDetails.Errors.First().Value.Code);
            act.Errors.Last().Value.Code.Should().Be(problemDetails.Errors.Last().Value.Code);

            act.Errors.First().Value.Description.Should().Be(problemDetails.Errors.First().Value.Description);
            act.Errors.Last().Value.Description.Should().Be(problemDetails.Errors.Last().Value.Description);

            act.Extensions.Should().HaveCount(0);
        }
    }
}
