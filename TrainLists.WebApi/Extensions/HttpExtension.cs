using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainLists.WebApi.Extensions
{
public static class HttpExtensions
    {
        static readonly JsonSerializerOptions _settings = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull  
        };

        public static async Task WriteJsonAsync<T>(this HttpResponse response, T obj, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json";

            var json = JsonSerializer.Serialize(obj, _settings);

            await response.WriteAsync(json);
        }
    }
}