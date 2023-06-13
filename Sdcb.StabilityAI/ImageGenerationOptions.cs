using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents the options for image generation.
/// </summary>
public class ImageGenerationOptions
{
    /// <summary>
    /// Gets or sets the height of the image in pixels.
    /// Default value: 512.
    /// Must be in increments of 64 and pass the validation specified in the document.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; set; } = 512;

    /// <summary>
    /// Gets or sets the width of the image in pixels.
    /// Default value: 512.
    /// Must be in increments of 64 and pass the validation specified in the document.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; set; } = 512;

    /// <summary>
    /// Gets or sets the required list of text prompts to use for generation.
    /// </summary>
    [JsonPropertyName("text_prompts")]
    public required TextPrompt[] TextPrompts { get; set; }

    /// <summary>
    /// Gets or sets how strictly the diffusion process adheres to the prompt text.
    /// Default value: 7.
    /// Range: [0, 35].
    /// </summary>
    [JsonPropertyName("cfg_scale")]
    public int CfgScale { get; set; } = 7;

    /// <summary>
    /// Gets or sets the clip guidance preset.
    /// Default value: NONE.
    /// Enum: FAST_BLUE, FAST_GREEN, NONE, SIMPLE, SLOW, SLOWER, SLOWEST.
    /// </summary>
    [JsonPropertyName("clip_guidance_preset")]
    public string ClipGuidancePreset { get; set; } = "NONE";

    /// <summary>
    /// Gets or sets the sampler to use for the diffusion process.
    /// Enum: DDIM, DDPM, K_DPMPP_2M, K_DPMPP_2S_ANCESTRAL, K_DPM_2, K_DPM_2_ANCESTRAL, K_EULER, K_EULER_ANCESTRAL, K_HEUN, K_LMS.
    /// If this value is omitted, an appropriate sampler will be selected automatically.
    /// </summary>
    [JsonPropertyName("sampler")]
    public string? Sampler { get; set; }

    /// <summary>
    /// Gets or sets the number of images to generate.
    /// Default value: 1.
    /// Range: [1, 10].
    /// </summary>
    [JsonPropertyName("samples")]
    public int Samples { get; set; } = 1;

    /// <summary>
    /// Gets or sets the random noise seed.
    /// Default value: 0.
    /// Range: [0, 4294967295].
    /// Omit this option or use 0 for a random seed.
    /// </summary>
    [JsonPropertyName("seed")]
    public uint Seed { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of diffusion steps to run.
    /// Default value: 50.
    /// Range: [10, 150].
    /// </summary>
    [JsonPropertyName("steps")]
    public int Steps { get; set; } = 50;

    /// <summary>
    /// Gets or sets the style preset to guide the image model towards a particular style.
    /// Enum: 3d-model, analog-film, anime, cinematic, comic-book, digital-art, enhance, fantasy-art, isometric, line-art, low-poly, modeling-compound, neon-punk, origami, photographic, pixel-art, tile-texture.
    /// The list of style presets is subject to change.
    /// </summary>
    [JsonPropertyName("style_preset")]
    public string? StylePreset { get; set; }

    /// <summary>
    /// Gets or sets the extra parameters passed to the engine.
    /// These parameters are used for in-development or experimental features and may change without warning, so please use with caution.
    /// </summary>
    [JsonPropertyName("extras")]
    public object? Extras { get; set; }
}
