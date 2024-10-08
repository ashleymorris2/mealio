using System.Collections.Concurrent;
using System.Reflection;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.NewtonsoftJson.Generation;
using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Caching;

/// <summary>
/// Manages the generation and caching of JSON schemas for given types
/// </summary>
internal class JsonSchemaCache
{
    private readonly ConcurrentDictionary<Type, JsonSchema> _schemaCache = new();

    /// <summary>
    /// Retrieves or generates a JSON schema for the specified type <typeparamref name="T"/> in a
    /// thread safe manner.
    /// </summary>
    /// <typeparam name="T">The type for which to retrieve or generate the schema.</typeparam>
    /// <returns>
    /// A <see cref="JsonSchema"/> for the specified type <typeparamref name="T"/>.
    /// If a schema for the type exists in the cache, it is returned. Otherwise, a new schema is generated
    /// and added to the cache.
    /// </returns>
    /// <remarks>
    /// An <see cref="OpenAiFormatSchemaProcessor"/> is added to the schema processors list
    /// to customise schema generation. The method also marks all properties in the schema as required,
    /// following OpenAI's requirement. This avoids adding [Required] attributes to the <typeparamref name="T"/> class,
    /// maintaining a separation between business logic and validation rules.
    /// </remarks>
    public JsonSchema GetOrGenerateSchemaForType<T>()
    {
        return _schemaCache.GetOrAdd(typeof(T), (type) =>
        {
            var settings = new NewtonsoftJsonSchemaGeneratorSettings()
            {
                DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull,
                SchemaType = SchemaType.JsonSchema
            };

            settings.SchemaProcessors.Add(new OpenAiFormatSchemaProcessor());
            
            // Create a generator with the custom settings
            var generator = new JsonSchemaGenerator(settings);

            // Generate the schema
            var schema = generator.Generate(typeof(Recipe));
            schema.AllowAdditionalProperties = false;

            // OpenAI specify that all properties to be marked as required.
            // This could be done with a [Required] attribute on the Class of type<T>
            // But would lead to tight coupling between business logic and application validation
            SetAlSchemaPropertiesAsRequiredForType<Recipe>(schema);
            return schema;
        });
    }

    private static void SetAlSchemaPropertiesAsRequiredForType<T>(JsonSchema schema)
    {
        var type = typeof(T);
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (schema.Properties.ContainsKey(property.Name))
            {
                schema.RequiredProperties.Add(property.Name);
            }

            if (IsCollection(property.PropertyType, out Type? elementType))
            {
                if (elementType == null) continue;
                if (schema.Definitions.TryGetValue(elementType.Name, out var nestedSchema))
                {
                    SetAllPropertiesAsRequiredForNestedType(elementType, nestedSchema);
                }
            }
            else if (property.PropertyType.IsClass && property.PropertyType != typeof(string) &&
                     !property.PropertyType.IsValueType)
            {
                var nestedSchema = schema.Definitions[property.PropertyType.Name];
                SetAllPropertiesAsRequiredForNestedType(property.PropertyType, nestedSchema);
            }
        }
    }

    // Checks if the type is a collection (List<T>, ICollection<T>, IEnumerable<T>) and return the element type
    private static bool IsCollection(Type type, out Type? elementType)
    {
        // Check if it's a generic type (e.g., List<T>, ICollection<T>)
        if (type.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
        {
            // Get the type argument (e.g., T in List<T>)
            elementType = type.GetGenericArguments()[0];
            return true;
        }

        elementType = null;
        return false;
    }
    
    private static void SetAllPropertiesAsRequiredForNestedType(Type type, JsonSchema nestedSchema)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            nestedSchema.RequiredProperties.Add(property.Name);
        }
    }
}

/// <summary>
///  Process the generated schema to OpenAi specification 
/// </summary>
internal class OpenAiFormatSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        // Generally want to remove any Id properties as this would be generated.
        if (context.Schema.Properties.ContainsKey("Id"))
        {
            context.Schema.Properties.Remove("Id");
        }
        
        foreach (var property in context.Schema.Properties.Values)
        {
            if (property.Format != null)
            {
                property.Format = null; 
            }
        }
    }
}