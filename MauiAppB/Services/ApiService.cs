using System.Net.Http.Json;
using MauiAppB.Models; // Убедись, что тут твои модели

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
    public async Task<bool> CreateGroup(string groupName, int userId, string photoUrl = "default_group.png")
    {
        try
        {
            var newGroup = new Group
            {
                Name = groupName,
                // Используем переданное фото или стандартное
                PhotoUrl = photoUrl 
            };

            // Отправляем запрос на /api/Groups/create/{userId}
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Groups/create/{userId}", newGroup);
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка создания группы: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Group>> GetGroups()
    {
        try
        {
            // Скачиваем список всех групп с сервера
            // Если нужно только мои, можно будет потом сделать фильтрацию
            var groups = await _httpClient.GetFromJsonAsync<List<Group>>($"{BaseUrl}Groups");
            return groups ?? new List<Group>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения групп: {ex.Message}");
            return new List<Group>(); // Возвращаем пустой список, чтобы не крашилось
        }
    }

    // Метод для получения списка ивентов
    public async Task<List<Event>> GetEvents()
    {
        try
        {
            // 1. Делаем запрос к серверу (GET api/Events)
            var response = await _httpClient.GetAsync($"{BaseUrl}Events");

            if (response.IsSuccessStatusCode)
            {
                // 2. Если всё ок, превращаем JSON ответ в список ивентов
                var events = await response.Content.ReadFromJsonAsync<List<Event>>();
                return events ?? new List<Event>();
            }
            else
            {
                // Если ошибка сервера, пишем в консоль
                System.Diagnostics.Debug.WriteLine($"Ошибка получения ивентов: {response.StatusCode}");
                return new List<Event>(); // Возвращаем пустой список
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сети: {ex.Message}");
            return new List<Event>();
        }
    }

    public async Task<bool> CreateEvent(string name, string description, string time, string location, string link, int groupId)
    {
        try
        {
            // 1. Создаем ЧИСТЫЙ объект (анонимный), чтобы отправить только то, что нужно серверу.
            // Имена полей (слева) должны точно совпадать с Event.cs на СЕРВЕРЕ.
            var eventData = new
            {
                Name = name,
                Description = description ?? "", // Защита от null
                Time = time,                     // Сервер ждет строку?
                Location = location ?? "Online",
                GroupId = groupId,
                ImageUrl = "free.png",
                Link = link ?? ""         // Обязательно отправляем картинку, даже если её нет
            };

            // 2. Отправляем
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Events", eventData);

            // 3. ПРОВЕРКА ОШИБКИ
            if (!response.IsSuccessStatusCode)
            {
                // Читаем, что именно ответил сервер (там будет текст ошибки)
                var errorBody = await response.Content.ReadAsStringAsync();
                
                // Пишем в консоль разработчика (внизу в Visual Studio)
                System.Diagnostics.Debug.WriteLine($"================");
                System.Diagnostics.Debug.WriteLine($"ОШИБКА СЕРВЕРА: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"ДЕТАЛИ: {errorBody}");
                System.Diagnostics.Debug.WriteLine($"================");

                // Показываем ошибку на экране телефона (чтобы вы увидели)
                await Shell.Current.DisplayAlert("Ошибка сервера", errorBody, "OK");
                
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА ПРИЛОЖЕНИЯ: {ex.Message}");
            return false;
        }
    }
}