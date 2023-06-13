using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents a user account.
/// </summary>
public class UserAccount
{
    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the user's organizations.
    /// </summary>
    [JsonPropertyName("organizations")]
    public required Organization[] Organizations { get; set; }

    /// <summary>
    /// Gets or sets the user's profile picture.
    /// </summary>
    [JsonPropertyName("profile_picture")]
    public string? ProfilePicture { get; set; }
}
