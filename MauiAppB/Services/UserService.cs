using MauiAppB.Models;

namespace MauiAppB.Services;

public static class UserService
{
    // Добавили '?' - теперь переменная может быть пустой без ошибок
    public static User? CurrentUser { get; set; }
}