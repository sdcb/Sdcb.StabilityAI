using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents an artifact associated with a generated image.
/// </summary>
public class Artifact
{
    /// <summary>
    /// Gets or sets the base64 encoded representation of the artifact.
    /// </summary>
    [JsonPropertyName("base64")]
    public required string Base64 { get; set; }

    /// <summary>
    /// Gets or sets the seed value used for generating the artifact.
    /// </summary>
    [JsonPropertyName("seed")]
    public uint Seed { get; set; }

    /// <summary>
    /// Gets or sets the reason for finishing the generation process.
    /// </summary>
    [JsonPropertyName("finishReason")]
    public FinishReasons FinishReason { get; set; }
}

/// <summary>
/// The result of the generation process.
/// </summary>
public enum FinishReasons
{
    /// <summary>
    /// SUCCESS indicates success.
    /// </summary>
    Success,

    /// <summary>
    /// ERROR indicates an error.
    /// </summary>
    Error,

    /// <summary>
    /// CONTENT_FILTERED indicates the result affected by the content filter and may be blurred.
    /// </summary>
    ContentFiltered,
}

internal class FinishReasonsConverter : JsonConverter<FinishReasons>
{
    public override FinishReasons Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "SUCCESS" => FinishReasons.Success,
            "ERROR" => FinishReasons.Error,
            "CONTENT_FILTERED" => FinishReasons.ContentFiltered,
            _ => throw new JsonException($"Invalid value for FinishReasons: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, FinishReasons value, JsonSerializerOptions options)
    {
        string stringValue = value switch
        {
            FinishReasons.Success => "SUCCESS",
            FinishReasons.Error => "ERROR",
            FinishReasons.ContentFiltered => "CONTENT_FILTERED",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
        writer.WriteStringValue(stringValue);
    }
}