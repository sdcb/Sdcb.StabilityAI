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
    public async Task<Engine[]> GetAllEnginesAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("v1/engines/list", cancellationToken);
        return await response.DeserializeAsync<Engine[]>(cancellationToken);
    }

    /// <summary>
    /// Generates an image from text using the specified engine ID and image generation options.
    /// </summary>
    /// <param name="options">The image generation options to be used for generating the image.</param>
    /// <param name="engineId">
    /// Known engine names:
    /// <list type="bullet">
    /// <item>esrgan-v1-x2plus</item>
    /// <item>stable-diffusion-v1</item>
    /// <item>stable-diffusion-v1-5</item>
    /// <item>stable-diffusion-512-v2-0</item>
    /// <item>stable-diffusion-768-v2-0</item>
    /// <item>stable-diffusion-depth-v2-0</item>
    /// <item>stable-diffusion-512-v2-1</item>
    /// <item>stable-diffusion-768-v2-1</item>
    /// <item>stable-diffusion-xl-beta-v2-2-2</item>
    /// <item>stable-diffusion-x4-latent-upscaler</item>
    /// <item>stable-inpainting-v1-0</item>
    /// <item>stable-inpainting-512-v2-0</item>
    /// </list>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated image as a task of GeneratedImages object.</returns>
    public async Task<Artifact[]> TextToImageAsync(TextToImageRequest options, string engineId = "stable-diffusion-xl-beta-v2-2-2", CancellationToken cancellationToken = default)
    {
        StringContent sc = new(JsonSerializer.Serialize(options));
        sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await _httpClient.PostAsync($"v1/generation/{engineId}/text-to-image", sc, cancellationToken);
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
