using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.NewtonsoftJson.Generation;
using OpenAI.Chat;
using RecipeProcessing.Core.Entities;
using RecipeProcessing.Infrastructure.Integrations.OpenAi;
using RecipeProcessing.Infrastructure.Interfaces;


namespace RecipeProcessing.Infrastructure.Services;

internal class OpenAiImageService : IImageService
{
    private readonly OpenAiConfig _openAiConfig;
    private readonly ChatClient _chatClient;

    public OpenAiImageService(IOptions<OpenAiConfig> options)
    {
        _openAiConfig = options.Value;
        _chatClient = new ChatClient(_openAiConfig.gptModels[GptModel.Gpt4o], _openAiConfig.ApiKey);
    }

    public async Task<string> Process(Stream imageStream, string imageFileContentType)
    {
        var promptPath = Path.Combine(Directory.GetCurrentDirectory(), _openAiConfig.Prompts.ImageProcessing);
        var prompt = await File.ReadAllTextAsync(promptPath);
        var schema = JsonSchemaCache.GetOrGenerateRecipeSchema();

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                name: "recipe_details",
                jsonSchema: BinaryData.FromString(schema),
                strictSchemaEnabled: true
            )
        };

        BinaryData imageBytes = await BinaryData.FromStreamAsync(imageStream);
        List<ChatMessage> messages =
        [
            new UserChatMessage(
                ChatMessageContentPart.CreateTextMessageContentPart(prompt),
                ChatMessageContentPart.CreateImageMessageContentPart(imageBytes, imageFileContentType))
        ];

        ChatCompletion chatCompletion = await _chatClient.CompleteChatAsync(messages, options);

        return chatCompletion.Content[0].Text;
    }
}

public class JsonSchemaCache
{
    private static readonly Dictionary<Type, string> SchemaCache = new();

    public static string GetOrGenerateRecipeSchema()
    {
        var type = typeof(Recipe);
        if (SchemaCache.TryGetValue(type, out var cachedSchema))
        {
            return cachedSchema;
        }
        
        var settings = new NewtonsoftJsonSchemaGeneratorSettings()
        {
            DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull, 
            SchemaType = SchemaType.JsonSchema
        };
        settings.SchemaProcessors.Add(new RemoveNumberFormatSchemaProcessor()); 
        
        // Create a generator with the custom settings
        var generator = new JsonSchemaGenerator(settings);
        
        // Generate the schema for the Recipe class
        var schema = generator.Generate(typeof(Recipe));
        
        schema.RequiredProperties.Add("Name");
        schema.RequiredProperties.Add("Title");
        schema.RequiredProperties.Add("Servings");
        schema.RequiredProperties.Add("PreparationTime");
        schema.RequiredProperties.Add("TotalTime");
        schema.RequiredProperties.Add("Ingredients");
        schema.RequiredProperties.Add("Instructions");
        schema.RequiredProperties.Add("NutritionPerServing");
        
        // Set required properties for sub-object Ingredient
        var ingredientSchema = schema.Definitions["Ingredient"];
        ingredientSchema.RequiredProperties.Add("Name");
        ingredientSchema.RequiredProperties.Add("Quantity");
        ingredientSchema.RequiredProperties.Add("Unit");

        // Set required properties for sub-object InstructionStep
        var instructionStepSchema = schema.Definitions["InstructionStep"];
        instructionStepSchema.RequiredProperties.Add("StepNumber");
        instructionStepSchema.RequiredProperties.Add("Description");
        
        var nutritionalDetailsSchema = schema.Definitions["NutritionalDetails"];
        nutritionalDetailsSchema.RequiredProperties.Add("Calories");
        nutritionalDetailsSchema.RequiredProperties.Add("Protein");
        nutritionalDetailsSchema.RequiredProperties.Add("Carbohydrates");
        nutritionalDetailsSchema.RequiredProperties.Add("Fat");
        nutritionalDetailsSchema.RequiredProperties.Add("SaturatedFat");
        nutritionalDetailsSchema.RequiredProperties.Add("Fiber");
        nutritionalDetailsSchema.RequiredProperties.Add("Sugars");
        nutritionalDetailsSchema.RequiredProperties.Add("Salt");

        // Generate schema if not cached
        var schemaJson = schema.ToJson();

        // Cache the schema
        SchemaCache[type] = schemaJson;

        return schemaJson;
    }
}



public class RemoveNumberFormatSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        // Remove 'format' property from numeric types
        foreach (var property in context.Schema.Properties.Values)
        {
            if ((property.Type == JsonObjectType.Integer || property.Type == JsonObjectType.Number) && property.Format != null)
            {
                property.Format = null; // Remove the 'format' field for numbers
            }
        }
    }
}