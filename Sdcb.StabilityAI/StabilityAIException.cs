namespace Sdcb.StabilityAI;

/// <summary>
/// A custom exception class for Stability AI.
/// </summary>
public class StabilityAIException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StabilityAIException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public StabilityAIException(string? message) : base(message)
    {
    }
}
