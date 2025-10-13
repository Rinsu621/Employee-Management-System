using EmployeeCRUD.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class GeminiService:IGeminiService
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        public GeminiService( IConfiguration config)
        {
            httpClient = new HttpClient();
            apiKey = config["Gemini:ApiKey"];
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts=new[]
                        {
                            new {text=prompt }
                        }
                    }
                }

            };
            var request= new HttpRequestMessage(HttpMethod.Post, "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent");
            request.Headers.Add("X-goog-api-key", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response= await httpClient.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var responseContent= await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(responseContent);
                var generatedText = document.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();
                return generatedText ?? "No content generated.";

            }
            else
            {
                var errorContent= await response.Content.ReadAsStringAsync();
                return $"Error: {response.StatusCode}, Details: {errorContent}";
            }


        }

    }
}
