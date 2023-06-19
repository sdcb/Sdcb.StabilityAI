using System.Text.Json.Serialization;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents the options for image to image generation.
/// </summary>
public class ImageToImageRequest
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
    /// Gets or sets how the init_image influences the result.
    /// Default value: IMAGE_STRENGTH.
    /// Values: IMAGE_STRENGTH, STEP_SCHEDULE.
    /// </summary>
    public string InitImageMode { get; set; } = "IMAGE_STRENGTH";

    /// <summary>
    /// Gets or sets how much influence the init_image has on the diffusion process.
    /// Default value: 0.35.
    /// Range: [0, 1].
    /// </summary>
    public float ImageStrength { get; set; } = 0.35f;

    /// <summary>
    /// Gets or sets the step schedule start value. Skips a proportion of the start of the diffusion steps, allowing the init_image to influence the final generated image.
    /// Default value: 0.65.
    /// Range: [0, 1].
    /// </summary>
    public double StepScheduleStart { get; set; } = 0.65;

    /// <summary>
    /// Gets or sets the step schedule end value. Skips a proportion of the end of the diffusion steps, allowing the init_image to influence the final generated image.
    /// Range: [0, 1].
    /// </summary>
    public double? StepScheduleEnd { get; set; }

    /// <summary>
    /// Gets or sets how strictly the diffusion process adheres to the prompt text.
    /// Default value: 7.
    /// Range: [0, 35].
    /// </summary>
    public int CfgScale { get; set; } = 7;

    /// <summary>
    /// Gets or sets the clip guidance preset.
    /// Default value: NONE.
    /// Enum: FAST_BLUE, FAST_GREEN, NONE, SIMPLE, SLOW, SLOWER, SLOWEST.
    /// </summary>
    public string ClipGuidancePreset { get; set; } = "NONE";

    /// <summary>
    /// Gets or sets the sampler to use for the diffusion process.
    /// Enum: DDIM, DDPM, K_DPMPP_2M, K_DPMPP_2S_ANCESTRAL, K_DPM_2, K_DPM_2_ANCESTRAL, K_EULER, K_EULER_ANCESTRAL, K_HEUN, K_LMS.
    /// If this value is omitted, an appropriate sampler will be selected automatically.
    /// </summary>
    public string? Sampler { get; set; }

    /// <summary>
    /// Gets or sets the number of images to generate.
    /// Default value: 1.
    /// Range: [1, 10].
    /// </summary>
    public int Samples { get; set; } = 1;

    /// <summary>
    /// Gets or sets the random noise seed.
    /// Default value: 0.
    /// Range: [0, 4294967295].
    /// Omit this option or use 0 for a random seed.
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
    /// Enum: 3d-model, analog-film, anime, cinematic, comic-book, digital-art, enhance, fantasy-art, isometric, line-art, low-poly, modeling-compound, neon-punk, origami, photographic, pixel-art, tile-texture.
    /// The list of style presets is subject to change.
    /// </summary>
    public string? StylePreset { get; set; }

    /// <summary>
    /// Gets or sets the extra parameters passed to the engine.
    /// These parameters are used for in-development or experimental features and may change without warning, so please use with caution.
    /// </summary>
    public object? Extras { get; set; }

    /// <summary>
    /// Converts ImageToImageRequest to MultipartFormDataContent
    /// </summary>
    /// <returns>MultipartFormDataContent instance that contains the converted ImageToImageRequest object</returns>
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

        content.Add(new StringContent(InitImageMode), "init_image_mode");
        if (InitImageMode == "IMAGE_STRENGTH")
        {
            content.Add(new StringContent(ImageStrength.ToString()), "image_strength");
        }
        else if (InitImageMode == "STEP_SCHEDULE")
        {
            content.Add(new StringContent(StepScheduleStart.ToString()), "step_schedule_start");

            if (StepScheduleEnd.HasValue)
            {
                content.Add(new StringContent(StepScheduleEnd.Value.ToString()), "step_schedule_end");
            }
        }       

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
