using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Sdcb.StabilityAI.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _console;

    public static IConfigurationRoot Config { get; }

    static UnitTest1()
    {
        Config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .AddEnvironmentVariables()
            .Build();
    }

    public UnitTest1(ITestOutputHelper console)
    {
        _console = console;
    }

    StabilityAIClient CreateAIClient()
    {
        string? apiKey = Config["STABILITY_API_KEY"];
        return new StabilityAIClient(apiKey);
    }

    StabilityAIClient CreateNegativeAIClient()
    {
        return new StabilityAIClient("bad-api-key");
    }

    [Fact]
    public async Task GetUserAccountTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        UserAccount account = await ai.GetUserAccountAsync();
        Assert.NotNull(account);
        Assert.NotEmpty(account.Id);
        Assert.NotEmpty(account.Email);
        Assert.True(account.Organizations[0].IsDefault);
    }

    [Fact]
    public async Task GetUserAccountTest_Negative()
    {
        using StabilityAIClient ai = CreateNegativeAIClient();
        StabilityAIException ex = await Assert.ThrowsAsync<StabilityAIException>(() => ai.GetUserAccountAsync());
        Assert.Contains("Incorrect API key provided", ex.Message);
    }

    [Fact]
    public async Task GetUserBalanceTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        UserBalance balance = await ai.GetUserBalanceAsync();
        Assert.NotNull(balance);
    }

    [Fact]
    public async Task GetAllEnginesTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        Engine[] engines = await ai.GetAllEnginesAsync();
        Assert.NotEmpty(engines);
    }

    [Fact]
    public async Task TextToImageTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        Artifact[] images = await ai.TextToImageAsync(new TextToImageRequest()
        {
            Samples = 2,
            Width = 128, 
            Height = 256, 
            Seed = 1, 
            Steps = 20,
            TextPrompts = new TextPrompt[]
            {
                new TextPrompt("A beautiful sunset over the ocean."), 
            }
        });
        Assert.Equal(2, images.Length);
        Assert.Equal(1u, images.Min(x => x.Seed));
        foreach (Artifact image in images)
        {
            string fileName = $"generated-{image.Seed}.png";
            _console.WriteLine(fileName);
            await File.WriteAllBytesAsync($"generated-{image.Seed}.png", Convert.FromBase64String(image.Base64));
        }
    }
}