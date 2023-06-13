namespace Sdcb.StabilityAI;

/// <summary>
/// Represents a user account.
/// </summary>
public class UserAccount
{
    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the user's organizations.
    /// </summary>
    public required Organization[] Organizations { get; set; }

    /// <summary>
    /// Gets or sets the user's profile picture.
    /// </summary>
    public string? ProfilePicture { get; set; }
}
