using System;
using System.Collections.Generic;
using System.Text;

namespace MauiApp1.Models
{
    public class User : BaseEntity
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
