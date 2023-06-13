using System.Net.Http.Json;
using System.Text.Json;

namespace Sdcb.StabilityAI;

internal static class HttpResponseExtensions
{
    public static JsonSerializerOptions JsonSerializerOptions { get; }

    static HttpResponseExtensions()
    {
        JsonSerializerOptions = new JsonSerializerOptions();
        JsonSerializerOptions.Converters.Add(new FinishReasonsConverter());
    }

    public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions, cancellationToken))!;
        }
        else
        {
            JsonDocument? json = await response.Content.ReadFromJsonAsync<JsonDocument>(JsonSerializerOptions, cancellationToken);
            throw new StabilityAIException(json?.RootElement.GetProperty("message").GetString() ?? response.ReasonPhrase);
        }
    }
}
