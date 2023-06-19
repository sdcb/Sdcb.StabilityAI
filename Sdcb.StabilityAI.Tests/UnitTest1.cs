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
        decimal balance = await ai.GetUserBalanceAsync();
        Assert.True(balance >= 0);
    }

    [Fact]
    public async Task GetAllEnginesTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        EngineInfo[] engines = await ai.GetAllEnginesAsync();
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
            await File.WriteAllBytesAsync(fileName, Convert.FromBase64String(image.Base64));
        }
    }

    [Fact]
    public async Task ImageToImage_STEP_SCHEDULE_Test()
    {
        using StabilityAIClient ai = CreateAIClient();
        Artifact[] images = await ai.ImageToImageAsync(new ImageToImageRequest
        {
            InitImage = await File.ReadAllBytesAsync("dog.jpg"),
            Samples = 2,
            Seed = 1,
            Steps = 20, 
            InitImageMode = "STEP_SCHEDULE",
            StepScheduleStart = 0.1, 
            StepScheduleEnd = 0.2,
            TextPrompts = new TextPrompt[]
            {
                new TextPrompt("3 cat"),
            }, 
            StylePreset = "anime",
        });
        Assert.Equal(2, images.Length);
        Assert.Equal(1u, images.Min(x => x.Seed));
        foreach (Artifact image in images)
        {
            string fileName = $"img2img-sc-{image.Seed}.png";
            _console.WriteLine(fileName);
            await File.WriteAllBytesAsync(fileName, Convert.FromBase64String(image.Base64));
        }
    }

    [Fact]
    public async Task ImageToImage_IMAGE_STRENGTH_Test()
    {
        using StabilityAIClient ai = CreateAIClient();
        Artifact[] images = await ai.ImageToImageAsync(new ImageToImageRequest
        {
            InitImage = await File.ReadAllBytesAsync("dog.jpg"),
            Samples = 2,
            Seed = 1,
            Steps = 20,
            InitImageMode = "IMAGE_STRENGTH",
            ImageStrength = 0.65f,
            TextPrompts = new TextPrompt[]
            {
                new TextPrompt("3 cat"),
            },
            StylePreset = "anime",
        });
        Assert.Equal(2, images.Length);
        Assert.Equal(1u, images.Min(x => x.Seed));
        foreach (Artifact image in images)
        {
            string fileName = $"img2img-is-{image.Seed}.png";
            _console.WriteLine(fileName);
            await File.WriteAllBytesAsync(fileName, Convert.FromBase64String(image.Base64));
        }
    }

    [Fact]
    public async Task UpscaleImageTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        Artifact[] images = await ai.UpscaleImageAsync(new UpscaleRequest
        {
            Image = await File.ReadAllBytesAsync("dog.jpg"),
            Width = 2048,
        });
        foreach (Artifact image in images)
        {
            string fileName = $"upscale-{image.Seed}.png";
            _console.WriteLine(fileName);
            await File.WriteAllBytesAsync(fileName, Convert.FromBase64String(image.Base64));
        }
    }

    [Fact]
    public async Task MaskImageTest()
    {
        using StabilityAIClient ai = CreateAIClient();
        Artifact[] images = await ai.MaskImageAsync(new MaskImageRequest
        {
            InitImage = await File.ReadAllBytesAsync("dog.jpg"),
            MaskImage = await File.ReadAllBytesAsync("mask.jpg"), 
            TextPrompts = new TextPrompt[]
            {
                new TextPrompt("empty"),
            },
            MaskSource = "MASK_IMAGE_WHITE", 
            Samples = 2,
            Seed = 1, 
            Steps = 20,
        });
        foreach (Artifact image in images)
        {
            string fileName = $"mask-{image.Seed}.png";
            _console.WriteLine(fileName);
            await File.WriteAllBytesAsync(fileName, Convert.FromBase64String(image.Base64));
        }
    }
}