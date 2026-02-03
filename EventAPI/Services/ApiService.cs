using System.Net.Http.Json;
using EventAPI.Models; // Убедись, что тут твои модели

namespace MauiAppB.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    
    // ВСТАВЬ СЮДА ССЫЛКУ ИЗ NGROK (не забудь /api/ в конце)
    private const string BaseUrl = "https://nonerudite-overlushly-yadira.ngrok-free.dev/api/";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    // 1. РЕГИСТРАЦИЯ
    public async Task<bool> RegisterUser(User user)
    {
        try
        {
            // Отправляем POST запрос на /api/Users
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Users", user);
            return response.IsSuccessStatusCode; // Вернет true, если 200 OK
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка регистрации: {ex.Message}");
            return false;
        }
    }

    // 2. ЛОГИН (Простой вариант: проверяем, есть ли такой юзер)
    public async Task<User> LoginUser(string username, string password)
    {
        try
        {
            // В реальном мире тут был бы POST запрос с паролем.
            // Для теста мы просто скачаем всех юзеров и найдем нужного.
            var users = await _httpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}Users");
            
            // Ищем юзера по никнейму (пароль мы пока не сделали в базе, пускаем по нику)
            var user = users?.FirstOrDefault(u => u.Username == username);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка входа: {ex.Message}");
            return null;
        }
    }
}