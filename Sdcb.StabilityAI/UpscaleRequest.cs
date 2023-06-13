namespace Sdcb.StabilityAI;

/// <summary>
/// Represents an upscale request with image bytes, desired width or height for the output image.
/// </summary>
public class UpscaleRequest
{
    /// <summary>
    /// Image bytes to upscale.
    /// </summary>
    public required byte[] Image { get; set; }

    /// <summary>
    /// Desired width of the output image. Only one of width/height may be specified.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Desired height of the output image. Only one of width/height may be specified.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Creates a multipart/form-data content object that includes the image and specified size.
    /// </summary>
    /// <returns>A multipart/form-data content object that includes the image and specified size.</returns>
    public MultipartFormDataContent ToMultipartFormDataContent()
    {
        if (Width.HasValue && Height.HasValue || !Width.HasValue && !Height.HasValue)
        {
            throw new ArgumentException("Exactly one of width or height must be specified.");
        }

        MultipartFormDataContent content = new()
        {
            { new ByteArrayContent(Image), "image" }
        };

        if (Width.HasValue)
        {
            content.Add(new StringContent(Width.Value.ToString()), "width");
        }
        else if (Height.HasValue)
        {
            content.Add(new StringContent(Height.Value.ToString()), "height");
        }

        return content;
    }
}
