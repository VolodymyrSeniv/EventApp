using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppB.Models;
public class ProfileEvent
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Time { get; set; }
    public string ImageUrl { get; set; } // Иконка или картинка
    public bool IsMyEvent { get; set; } // Чтобы отличать логику, если нужно
}