using MauiAppB.Models;

namespace MauiAppB.Services;

public interface IDataService
{
    Task<List<Group>> GetMyGroupsAsync();
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RegisterAsync(string email, string password);
}