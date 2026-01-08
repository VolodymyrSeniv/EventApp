using System;
using System.Collections.Generic;
using System.Text;

namespace MauiApp1.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        //public int GroupId { get; set; }
        //public Group RelatedGroup { get; set; }
        //public List<User> Participants { get; set; } = new List<User>();
    }
}
