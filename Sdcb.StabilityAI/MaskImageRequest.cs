namespace Sdcb.StabilityAI;

/// <summary>
/// Represents the options for Mask Image generation.
/// </summary>
public class MaskImageRequest
{
    /// <summary>
    /// Gets or sets the required array of text prompts to use for generation.
    /// </summary>
    public required TextPrompt[] TextPrompts { get; set; }

    /// <summary>
    /// Gets or sets the required image used to initialize the diffusion process, in lieu of random noise.
    /// </summary>
    public required byte[] InitImage { get; set; }

    /// <summary>
    /// Gets or sets the mask source.
    /// Determines where to source the mask from.
    /// Enum: MASK_IMAGE_WHITE, MASK_IMAGE_BLACK, INIT_IMAGE_ALPHA
    /// </summary>
    public required string MaskSource { get; set; } = "MASK_IMAGE_WHITE";

    /// <summary>
    /// Gets or sets the required grayscale mask that allows for influence over which pixels are eligible for diffusion and at what strength.
    /// Must be the same dimensions as the init_image.
    /// </summary>
    public required byte[] MaskImage { get; set; }

    /// <summary>
    /// Gets or sets how strictly the diffusion process adheres to the prompt text (higher values keep your image closer to your prompt).
    /// Default value: 7.
    /// Range: [0, 35].
    /// </summary>
    public int CfgScale { get; set; } = 7;

    /// <summary>
    /// Gets or sets the clip guidance preset.
    /// Enum: FAST_BLUE, FAST_GREEN, NONE, SIMPLE, SLOW, SLOWER, SLOWEST
    /// Default value: NONE.
    /// </summary>
    public string ClipGuidancePreset { get; set; } = "NONE";

    /// <summary>
    /// Which sampler to use for the diffusion process.
    /// Enum: DDIM, DDPM, K_DPMPP_2M, K_DPMPP_2S_ANCESTRAL, K_DPM_2, K_DPM_2_ANCESTRAL, K_EULER, K_EULER_ANCESTRAL, K_HEUN, K_LMS
    /// </summary>
    public string? Sampler { get; set; }

    /// <summary>
    /// Gets or sets the number of images to generate.
    /// Default value: 1.
    /// Range: [1, 10].
    /// </summary>
    public int Samples { get; set; } = 1;

    /// <summary>
    /// Gets or sets the random noise seed (omit this option or use for a random seed).
    /// Default value: 0.
    /// Range: [0, 4294967295].
    /// </summary>
    public uint Seed { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of diffusion steps to run.
    /// Default value: 50.
    /// Range: [10, 150].
    /// </summary>
    public int Steps { get; set; } = 50;

    /// <summary>
    /// Gets or sets the style preset to guide the image model towards a particular style.
    /// Enum: 3d-model, analog-film, anime, cinematic, comic-book, digital-art, enhance, fantasy-art, isometric, line-art, low-poly, modeling-compound, neon-punk, origami, photographic, pixel-art, tile-texture
    /// </summary>
    public string? StylePreset { get; set; }

    /// <summary>
    /// Gets or sets the extra parameters passed to the engine.
    /// These parameters are used for in-development or experimental features and may change without warning, so please use with caution.
    /// </summary>
    public object? Extras { get; set; }

    /// <summary>
    /// Converts the MaskImageRequest object to a MultipartFormDataContent object for making HTTP requests.
    /// </summary>
    /// <returns>A MultipartFormDataContent object containing the properties of the MaskImageRequest.</returns>
    public MultipartFormDataContent ToMultipartFormDataContent()
    {
        var content = new MultipartFormDataContent();

        for (int i = 0; i < TextPrompts.Length; i++)
        {
            content.Add(new StringContent(TextPrompts[i].Text), $"text_prompts[{i}][text]");
            if (TextPrompts[i].Weight != null)
            {
                content.Add(new StringContent(TextPrompts[i].Weight!.Value.ToString()), $"text_prompts[{i}][weight]");
            }
        }

        content.Add(new ByteArrayContent(InitImage), "init_image");
        content.Add(new StringContent(MaskSource), "mask_source");
        content.Add(new ByteArrayContent(MaskImage), "mask_image");
        content.Add(new StringContent(CfgScale.ToString()), "cfg_scale");
        content.Add(new StringContent(ClipGuidancePreset), "clip_guidance_preset");

        if (!string.IsNullOrEmpty(Sampler))
        {
            content.Add(new StringContent(Sampler), "sampler");
        }

        content.Add(new StringContent(Samples.ToString()), "samples");
        content.Add(new StringContent(Seed.ToString()), "seed");
        content.Add(new StringContent(Steps.ToString()), "steps");

        if (!string.IsNullOrEmpty(StylePreset))
        {
            content.Add(new StringContent(StylePreset), "style_preset");
        }

        if (Extras != null)
        {
            string? extrasString = Extras.ToString();
            if (extrasString != null)
            {
                content.Add(new StringContent(extrasString), "extras");
            }
        }

        return content;
    }
}