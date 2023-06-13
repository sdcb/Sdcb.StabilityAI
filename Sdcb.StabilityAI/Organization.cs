namespace Sdcb.StabilityAI;

/// <summary>
/// Represents an organization.
/// </summary>
public class Organization
{
    /// <summary>
    /// Gets or sets the organization's ID.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the organization's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the user's role in the organization.
    /// </summary>
    public required string Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the organization is the user's default organization.
    /// </summary>
    public bool IsDefault { get; set; }
}
