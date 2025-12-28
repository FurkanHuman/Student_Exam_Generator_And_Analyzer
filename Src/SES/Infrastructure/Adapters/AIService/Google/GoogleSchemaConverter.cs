using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService.Google;

/// <summary>
/// Converts Newtonsoft.Json schemas to Google Gemini compatible format
/// </summary>
internal static class GoogleSchemaConverter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    /// <summary>
    /// Converts Newtonsoft.Json schema to Google Gemini compatible schema string
    /// </summary>
    public static string ConvertToGoogleSchema(string newtonsoftSchema)
    {
        JObject schemaObj = JObject.Parse(newtonsoftSchema);

        // Clean the schema
        CleanSchemaForGoogle(schemaObj);

        return schemaObj.ToString(Newtonsoft.Json.Formatting.None);
    }

    /// <summary>
    /// Recursively cleans the schema to be compatible with Google Gemini
    /// </summary>
    private static void CleanSchemaForGoogle(JToken token)
    {
        if (token is JObject obj)
        {
            // Remove additionalProperties - Google doesn't support this
            obj.Remove("additionalProperties");

            // Remove $schema and other meta properties
            obj.Remove("$schema");
            obj.Remove("$id");
            obj.Remove("title");
            obj.Remove("description"); // Remove root description, keep property descriptions

            // Convert type arrays to single type string
            if (obj["type"] is JArray typeArray && typeArray.Count > 0)
            {
                // Take the first non-null type
                JToken? firstType = typeArray.FirstOrDefault(t => t.ToString() != "null");
                if (firstType != null)
                    obj["type"] = firstType.ToString();
            }

            // Handle enum values - ensure they're simple arrays
            if (obj["enum"] is JArray enumArray)
            {
                JArray cleanEnum = [];
                foreach (JToken item in enumArray)
                    if (item.Type != JTokenType.Null)
                        cleanEnum.Add(item);

                obj["enum"] = cleanEnum;
            }

            // Recursively clean nested objects
            foreach (JProperty property in obj.Properties().ToList())
                CleanSchemaForGoogle(property.Value);

        }

        else if (token is JArray array)
            foreach (JToken item in array)
                CleanSchemaForGoogle(item);
    }

    /// <summary>
    /// Alternative: Creates a Google-native schema from scratch using System.Text.Json
    /// This avoids Newtonsoft.Json completely
    /// </summary>
    public static string CreateGoogleNativeSchema<T>()
    {
        var schema = new Dictionary<string, object>
        {
            ["type"] = "object"
        };

        Dictionary<string, object> properties = BuildPropertiesFromType(typeof(T));
        if (properties.Count > 0)
            schema["properties"] = properties;

        List<string> required = GetRequiredProperties(typeof(T));
        if (required.Count > 0)
            schema["required"] = required;

        return JsonSerializer.Serialize(schema, JsonOptions);
    }

    private static Dictionary<string, object> BuildPropertiesFromType(Type type)
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();

        foreach (var prop in type.GetProperties())
        {
            var propSchema = GetPropertySchema(prop.PropertyType);
            if (propSchema != null)
            {
                string propName = JsonNamingPolicy.CamelCase.ConvertName(prop.Name);
                properties[propName] = propSchema;
            }
        }

        return properties;
    }

    private static Dictionary<string, object>? GetPropertySchema(Type type)
    {
        // Handle nullable types
        Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        // String
        if (underlyingType == typeof(string))
            return new Dictionary<string, object> { ["type"] = "string" };

        // Numbers
        if (underlyingType == typeof(int) || underlyingType == typeof(long) || underlyingType == typeof(byte) || underlyingType == typeof(short))
            return new Dictionary<string, object> { ["type"] = "integer" };

        if (underlyingType == typeof(double) || underlyingType == typeof(float) || underlyingType == typeof(decimal))
            return new Dictionary<string, object> { ["type"] = "number" };

        // Boolean
        if (underlyingType == typeof(bool))
            return new Dictionary<string, object> { ["type"] = "boolean" };

        // DateTime
        if (underlyingType == typeof(DateTime) || underlyingType == typeof(DateTimeOffset))
            return new Dictionary<string, object>
            {
                ["type"] = "string",
                ["format"] = "date-time"
            };

        // Enum
        if (underlyingType.IsEnum)
        {
            string?[] enumValues = Enum.GetValues(underlyingType).Cast<object>().Select(e => e.ToString()).ToArray();
            return new Dictionary<string, object>
            {
                ["type"] = "string",
                ["enum"] = enumValues
            };
        }

        // Arrays and Lists
        if (type.IsArray)
        {
            Type elementType = type.GetElementType()!;
            object? itemSchema = GetPropertySchema(elementType);
            return new Dictionary<string, object>
            {
                ["type"] = "array",
                ["items"] = itemSchema ?? new Dictionary<string, object> { ["type"] = "object" }
            };
        }

        if (type.IsGenericType)
        {
            Type genericDef = type.GetGenericTypeDefinition();

            // List, IList, ICollection, IEnumerable
            if (genericDef == typeof(List<>) ||
                genericDef == typeof(IList<>) ||
                genericDef == typeof(ICollection<>) ||
                genericDef == typeof(IEnumerable<>))
            {
                Type elementType = type.GetGenericArguments()[0];
                object? itemSchema = GetPropertySchema(elementType);
                return new Dictionary<string, object>
                {
                    ["type"] = "array",
                    ["items"] = itemSchema ?? new Dictionary<string, object> { ["type"] = "object" }
                };
            }

            // Dictionary
            if (genericDef == typeof(Dictionary<,>) || genericDef == typeof(IDictionary<,>))
            {
                Type valueType = type.GetGenericArguments()[1];
                object? valueSchema = GetPropertySchema(valueType);

                return new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["additionalProperties"] = valueSchema ?? new Dictionary<string, object> { ["type"] = "object" }
                };
            }
        }

        // Complex object
        if (underlyingType.IsClass && underlyingType != typeof(string))
        {
            Dictionary<string, object> objSchema = new()
            {
                ["type"] = "object"
            };

            Dictionary<string, object> properties = BuildPropertiesFromType(underlyingType);
            if (properties.Count > 0)
                objSchema["properties"] = properties;

            List<string> required = GetRequiredProperties(underlyingType);
            if (required.Count > 0)
                objSchema["required"] = required;

            return objSchema;
        }

        return null;
    }

    private static List<string> GetRequiredProperties(Type type)
    {
        List<string> required = [];

        foreach (PropertyInfo prop in type.GetProperties())
        {
            // Check for Required attribute
            bool hasRequired = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Length != 0;

            // Check if type is non-nullable value type
            bool isNonNullableValueType = prop.PropertyType.IsValueType &&
                                        Nullable.GetUnderlyingType(prop.PropertyType) == null;

            if (hasRequired || isNonNullableValueType)
            {
                string propName = JsonNamingPolicy.CamelCase.ConvertName(prop.Name);
                required.Add(propName);
            }
        }

        return required;
    }
}