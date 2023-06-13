using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Sdcb.StabilityAI;

/// <summary>
/// Represents a client for interacting with the StabilityAI API. This class is responsible for managing the HTTP requests and responses, and provides an easy way to interact with the API.
/// </summary>
public class StabilityAIClient : IDisposable
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates a new instance of the StabilityAIClient class and initializes the HttpClient 
    /// instance with the base address, HTTP request headers and stability api key to be used for requests.
    /// </summary>
    /// <param name="stabilityApiKey">A string representing the stability api key for authentication.</param>
    public StabilityAIClient(string? stabilityApiKey)
    {
        if (stabilityApiKey == null) throw new ArgumentNullException(nameof(stabilityApiKey));

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.stability.ai")
        };
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stabilityApiKey);
    }

    /// <summary>
    /// Returns the user account information.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The user account information as a task of <see cref="UserAccount"/> object.</returns>
    public async Task<UserAccount> GetUserAccountAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("v1/user/account", cancellationToken);
        return await response.DeserializeAsync<UserAccount>(cancellationToken);
    }

    /// <summary>
    /// Returns the current balance of the user's account.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The user balance as a task of <see cref="UserBalance"/> object.</returns>
    public async Task<UserBalance> GetUserBalanceAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("v1/user/balance", cancellationToken);
        return await response.DeserializeAsync<UserBalance>(cancellationToken);
    }

    /// <summary>
    /// Returns a list of all available engine names.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The engine list as a task of <see cref="List{Engine}"/> object.</returns>
    public async Task<EngineInfo[]> GetAllEnginesAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("v1/engines/list", cancellationToken);
        return await response.DeserializeAsync<EngineInfo[]>(cancellationToken);
    }

    /// <summary>
    /// Generates an image from text using the specified engine ID and image generation options.
    /// </summary>
    /// <param name="options">The image generation options to be used for generating the image.</param>
    /// <param name="engineId">The text to image engine to use, known values: <see cref="KnownEngines"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated image as a task of GeneratedImages object.</returns>
    public async Task<Artifact[]> TextToImageAsync(TextToImageRequest options, string engineId = KnownEngines.StableDiffusionXlBetaV222, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(engineId)) throw new ArgumentNullException(nameof(engineId));

        StringContent sc = new(JsonSerializer.Serialize(options));
        sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await _httpClient.PostAsync($"v1/generation/{engineId}/text-to-image", sc, cancellationToken);
        return (await response.DeserializeAsync<GeneratedImages>(cancellationToken)).Artifacts;
    }

    /// <summary>
    /// Generates a new image from an input image and prompts using the specified engine ID and image generation options.
    /// </summary>
    /// <param name="options">The image-to-image generation options to be used for generating the new image.</param>
    /// <param name="engineId">The image-to-image engine to use, known values: <see cref="KnownEngines"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated image as a task of GeneratedImages object.</returns>
    public async Task<Artifact[]> ImageToImageAsync(ImageToImageRequest options, string engineId = KnownEngines.StableDiffusionXlBetaV222, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(engineId)) throw new ArgumentNullException(nameof(engineId));

        HttpResponseMessage response = await _httpClient.PostAsync($"v1/generation/{engineId}/image-to-image", options.ToMultipartFormDataContent(), cancellationToken);
        return (await response.DeserializeAsync<GeneratedImages>(cancellationToken)).Artifacts;
    }

    /// <summary>
    /// Upscales an input image using the specified upscale engine ID, and desired width or height for the output image.
    /// </summary>
    /// <param name="options">The upscale request options, including image bytes and desired width or height.</param>
    /// <param name="engineId">The upscale engine to use, known values: <see cref="KnownEngines"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The upscaled image as a byte array in a Task.</returns>
    public async Task<Artifact[]> UpscaleImageAsync(UpscaleRequest options, string engineId = KnownEngines.StableDiffusionX4LatentUpscaler, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(engineId)) throw new ArgumentNullException(nameof(engineId));

        HttpResponseMessage response = await _httpClient.PostAsync($"v1/generation/{engineId}/image-to-image/upscale", options.ToMultipartFormDataContent(), cancellationToken);
        return (await response.DeserializeAsync<GeneratedImages>(cancellationToken)).Artifacts;
    }

    /// <summary>
    /// Generates an image based on the specified mask using the given engine ID and MaskImageRequest options.
    /// </summary>
    /// <param name="options">The MaskImageRequest options, including source image, mask image, and other related settings.</param>
    /// <param name="engineId">The engine to use for image masking.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>An array of generated images as Artifact objects in a Task.</returns>
    public async Task<Artifact[]> MaskImageAsync(MaskImageRequest options, string engineId = KnownEngines.StableInpaintingV10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(engineId)) throw new ArgumentNullException(nameof(engineId));

        HttpResponseMessage response = await _httpClient.PostAsync($"v1/generation/{engineId}/image-to-image/masking", options.ToMultipartFormDataContent(), cancellationToken);
        return (await response.DeserializeAsync<GeneratedImages>(cancellationToken)).Artifacts;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpClient.Dispose();
        }
    }

    ~StabilityAIClient()
    {
        Dispose(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
