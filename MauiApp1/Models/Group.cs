using System;
using System.Collections.Generic;
using System.Text;

namespace MauiApp1.Models
{
    public class Group : BaseEntity
    {
        public string GroupName { get; set; }
        public string ImageUrl { get; set; }
        public List<User> Members { get; set; } = new List<User>();
    }
}
