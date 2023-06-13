using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents a generated image.
/// </summary>
internal class GeneratedImages
{
    /// <summary>
    /// Gets or sets the array of artifacts associated with the generated image.
    /// </summary>
    [JsonPropertyName("artifacts")]
    public required Artifact[] Artifacts { get; set; }
}
