using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents an Engine class to store engine information.
/// </summary>
public class Engine
{
    /// <summary>
    /// Gets or sets the description of the engine.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the engine.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the engine.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of content this engine produces.
    /// Allowed values: AUDIO, CLASSIFICATION, PICTURE, STORAGE, TEXT, VIDEO
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }
}
