using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MauiAppB.Models;

namespace MauiAppB.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        // Use emulator host and the actual HTTP port used by your API
        private const string BaseUrl = "http://10.0.2.2:8099/api/account/";

        public AuthService()
        {
            var handler = new HttpClientHandler();

            // No TLS bypass needed for plain HTTP local dev.
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}login", new { email, password });
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<User>();
                return null;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"LoginAsync - network error: {ex}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoginAsync - unexpected error: {ex}");
                return null;
            }
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}register", user);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    string body = string.Empty;
                    try
                    {
                        body = await response.Content.ReadAsStringAsync();
                    }
                    catch (Exception readEx)
                    {
                        body = $"(failed to read response body: {readEx.Message})";
                    }

                    var msg = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {body}";
                    Debug.WriteLine($"RegisterAsync - server returned error: {msg}");
                    return (false, msg);
                }
            }
            catch (HttpRequestException ex)
            {
                var inner = ex.InnerException != null ? $" | inner: {ex.InnerException.Message}" : string.Empty;
                var msg = $"HttpRequestException: {ex.Message}{inner}";
                Debug.WriteLine($"RegisterAsync - network error: {msg}");
                return (false, msg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RegisterAsync - unexpected error: {ex}");
                return (false, $"Unexpected error: {ex.Message}");
            }
        }
    }
}