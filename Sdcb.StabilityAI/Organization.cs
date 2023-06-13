using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents an organization.
/// </summary>
public class Organization
{
    /// <summary>
    /// Gets or sets the organization's ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the organization's name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the user's role in the organization.
    /// </summary>
    [JsonPropertyName("role")]
    public required string Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the organization is the user's default organization.
    /// </summary>
    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }
}
