using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PowerUtils.AspNetCore.ErrorHandler.Serializers
{
    internal sealed class ErrorProblemDetailsJsonConverter : JsonConverter<ErrorProblemDetails>
    {
        private static readonly JsonEncodedText _type = JsonEncodedText.Encode("type");
        private static readonly JsonEncodedText _title = JsonEncodedText.Encode("title");
        private static readonly JsonEncodedText _status = JsonEncodedText.Encode("status");
        private static readonly JsonEncodedText _detail = JsonEncodedText.Encode("detail");
        private static readonly JsonEncodedText _instance = JsonEncodedText.Encode("instance");
        private static readonly JsonEncodedText _traceId = JsonEncodedText.Encode("traceId");
        private static readonly JsonEncodedText _errors = JsonEncodedText.Encode("errors");


        public override ErrorProblemDetails Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Unexcepted end when reading JSON.");
            }

            var problemDetails = new ErrorProblemDetails();

            while(reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                _readValue(ref reader, problemDetails, options);
            }

            if(reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("Unexcepted end when reading JSON.");
            }

            return problemDetails;
        }

        [RequiresUnreferencedCode("JSON serialization and deserialization of ProblemDetails.Extensions might require types that cannot be statically analyzed.")]
        private static void _readValue(ref Utf8JsonReader reader, ErrorProblemDetails value, JsonSerializerOptions options)
        {
            if(_tryReadStringProperty(ref reader, _type, out var propertyValue))
            {
                value.Type = propertyValue;
            }
            else if(_tryReadStringProperty(ref reader, _title, out propertyValue))
            {
                value.Title = propertyValue;
            }
            else if(_tryReadStringProperty(ref reader, _detail, out propertyValue))
            {
                value.Detail = propertyValue;
            }
            else if(_tryReadStringProperty(ref reader, _instance, out propertyValue))
            {
                value.Instance = propertyValue;
            }
            else if(_tryReadStringProperty(ref reader, _traceId, out propertyValue))
            {
                value.TraceId = propertyValue;
            }
            else if(reader.ValueTextEquals(_status.EncodedUtf8Bytes))
            {
                reader.Read();
                if(reader.TokenType == JsonTokenType.Null)
                {
                    value.Status = null;
                }
                else
                {
                    value.Status = reader.GetInt32();
                }
            }
            else if(reader.ValueTextEquals(_errors.EncodedUtf8Bytes))
            {
                value.Errors = JsonSerializer.Deserialize(ref reader, typeof(IDictionary<string, ErrorDetails>), options) as IDictionary<string, ErrorDetails>;
            }
        }

        private static bool _tryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, [NotNullWhen(true)] out string value)
        {
            if(!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
            {
                value = default;
                return false;
            }

            reader.Read();
            value = reader.GetString()!;
            return true;
        }

        [UnconditionalSuppressMessage("Trimmer", "IL2026", Justification = "Trimmer does not allow annotating overriden methods with annotations different from the ones in base type.")]
        public override void Write(Utf8JsonWriter writer, ErrorProblemDetails value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if(value.Type is null)
            {
                writer.WriteNull(_type);
            }
            else
            {
                writer.WriteString(_type, value.Type);
            }

            if(value.Type is null)
            {
                writer.WriteNull(_title);
            }
            else
            {
                writer.WriteString(_title, value.Title);
            }

            if(value.Status is null)
            {
                writer.WriteNull(_status);
            }
            else
            {
                writer.WriteNumber(_status, value.Status.Value);
            }

            if(value.Detail is null)
            {
                writer.WriteNull(_detail);
            }
            else
            {
                writer.WriteString(_detail, value.Detail);
            }

            if(value.Instance is null)
            {
                writer.WriteNull(_instance);
            }
            else
            {
                writer.WriteString(_instance, value.Instance);
            }


            if(value.TraceId is null)
            {
                var propertyName = _traceId.ToString();
                if(value.Extensions.TryGetValue(propertyName, out var property))
                {
                    var traceId = Convert.ToString(property);
                    writer.WriteString(_traceId, traceId);
                }
            }
            else
            {
                writer.WriteString(_traceId, value.TraceId);
            }


            writer.WritePropertyName(_errors);
            SerializeErrors(writer, value.Errors, options);

            writer.WriteEndObject();


            [UnconditionalSuppressMessage("Trimmer", "IL2026", Justification = "We ensure IDictionary<string, ErrorDetails> is preserved.")]
            [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(IDictionary<string, ErrorDetails>))]
            static void SerializeErrors(Utf8JsonWriter writer, IDictionary<string, ErrorDetails> errors, JsonSerializerOptions options)
                => JsonSerializer.Serialize(writer, errors, options);
        }
    }

}
