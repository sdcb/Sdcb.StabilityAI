# Sdcb.StabilityAI [![NuGet](https://img.shields.io/nuget/v/Sdcb.StabilityAI.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.StabilityAI/) [![NuGet](https://img.shields.io/nuget/dt/Sdcb.StabilityAI.svg?style=flat-square)](https://www.nuget.org/packages/Sdcb.StabilityAI/) [![GitHub](https://img.shields.io/github/license/sdcb/Sdcb.StabilityAI.svg?style=flat-square&label=license)](https://github.com/sdcb/Sdcb.StabilityAI/blob/master/LICENSE.txt)

Unofficial Stability AI rest API C# SDK, upstream Stability API documentation: https://platform.stability.ai/rest-api

## Functions mapping

All functions are in class `StabilityAIClient` and all are async.

| API Endpoint                             | C# Function Name         | Description                                                     |
|------------------------------------------|--------------------------|-----------------------------------------------------------------|
| `GET /v1/user/account`                  | GetUserAccountAsync      | Returns the user account information                            |
| `GET /v1/user/balance`                  | GetUserBalanceAsync      | Returns the current balance of the user's account               |
| `GET /v1/engines/list`                  | GetAllEnginesAsync       | Returns a list of all available engine names                    |
| `POST /v1/generation/:engine/text-to-image`        | TextToImageAsync         | Generates an image from text using the specified engine ID and image generation options |
| `POST /v1/generation/:engine/image-to-image`       | ImageToImageAsync        | Generates a new image from an input image and prompts using the specified engine ID and image generation options |
| `POST /v1/generation/:engine/image-to-image/upscale` | UpscaleImageAsync     | Upscales an input image using the specified upscale engine ID and desired width or height for the output image |
| `POST /v1/generation/:engine/image-to-image/masking` | MaskImageAsync        | Generates an image based on the specified mask using the given engine ID and MaskImageRequest options |

## Usage

### Example 1: Get User Account

In this example, we demonstrate how to get user account information using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Get the user account information and display it
UserAccount userAccount = await aiClient.GetUserAccountAsync();
Console.WriteLine($"User ID: {userAccount.Id}");
Console.WriteLine($"Email: {userAccount.Email}");
```

### Example 2: Get User Balance

In this example, we demonstrate how to get the account balance using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Get the user balance and display it
UserBalance userBalance = await aiClient.GetUserBalanceAsync();
Console.WriteLine($"Remaining balance: ${userBalance.Balance}");
```

### Example 3: Get Engine List

In this example, we demonstrate how to get the list of available engines using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Get the engine list and display it
EngineInfo[] engineList = await aiClient.GetAllEnginesAsync();
foreach (EngineInfo engine in engineList)
{
    Console.WriteLine($"Engine ID: {engine.Id} - Name: {engine.Name}");
}
```

### Example 4: Text to Image

In this example, we demonstrate how to generate an image from text using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Set up the TextToImageRequest options
TextToImageRequest options = new TextToImageRequest
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
};

// Generate the image from text using the specified options
Artifact[] generatedImages = await aiClient.TextToImageAsync(options);

// Save generated images to files
foreach (Artifact image in generatedImages)
{
    string fileName = $"generated-{image.Seed}.png";
    File.WriteAllBytes(fileName, Convert.FromBase64String(image.Base64));
}
```

## Example 5: Image to Image

In this example, we demonstrate how to generate a new image from an input image and prompts using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Set up the ImageToImageRequest options
ImageToImageRequest options = new ImageToImageRequest
{
    InitImage = File.ReadAllBytes("input_image.jpg"),
    Samples = 2,
    Seed = 1,
    Steps = 20,
    TextPrompts = new TextPrompt[]
    {
        new TextPrompt("3 cat"),
    },
    StylePreset = "anime",
};

// Generate the new image from the input image and prompts using the specified options
Artifact[] generatedImages = await aiClient.ImageToImageAsync(options);

// Save generated images to files
foreach (Artifact image in generatedImages)
{
    string fileName = $"image_to_image-{image.Seed}.png";
    File.WriteAllBytes(fileName, Convert.FromBase64String(image.Base64));
}
```

### Example 6: Upscale Image

In this example, we demonstrate how to upscale an input image using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Set up the UpscaleRequest options
UpscaleRequest options = new UpscaleRequest
{
    Image = File.ReadAllBytes("input_image.jpg"),
    Width = 2048,
};

// Upscale the input image using the specified options
Artifact[] upscaledImages = await aiClient.UpscaleImageAsync(options);

// Save upscaled images to files
foreach (Artifact image in upscaledImages)
{
    string fileName = $"upscaled-{image.Seed}.png";
    File.WriteAllBytes(fileName, Convert.FromBase64String(image.Base64));
}
```

### Example 7: Mask Image

In this example, we demonstrate how to generate an image based on a specified mask using the StabilityAIClient class.

```csharp
using StabilityAI;

// Create an instance of the StabilityAIClient using your API key
StabilityAIClient aiClient = new StabilityAIClient("your_api_key_here");

// Set up the MaskImageRequest options
MaskImageRequest options = new MaskImageRequest
{
    InitImage = File.ReadAllBytes("source_image.jpg"),
    MaskImage = File.ReadAllBytes("mask_image.jpg"),
    TextPrompts = new TextPrompt[]
    {
        new TextPrompt("empty"),
    },
    MaskSource = "MASK_IMAGE_WHITE",
    Samples = 2,
    Seed = 1,
    Steps = 20,
};

// Generate the masked image using the specified options
Artifact[] maskedImages = await aiClient.MaskImageAsync(options);

// Save masked images to files
foreach (Artifact image in maskedImages)
{
    string fileName = $"masked-{image.Seed}.png";
    File.WriteAllBytes(fileName, Convert.FromBase64String(image.Base64));
}
```
