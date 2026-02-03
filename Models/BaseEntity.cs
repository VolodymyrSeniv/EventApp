using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppB.Models
{
    public abstract class BaseEntity: ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}