using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;


/// <summary>
/// Represents a text prompt for image generation.
/// </summary>
/// <param name="Text"> Gets or sets the required text for the prompt. </param>
/// <param name="Weight"> Gets or sets the optional weight for the prompt. </param>
public record TextPrompt([property: JsonPropertyName("text")] string Text, [property: JsonPropertyName("weight")] double? Weight = null);
