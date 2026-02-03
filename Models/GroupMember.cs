using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppB.Models
{
    internal class GroupMember
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int GroupId { get; set; }

        [Indexed]
        public int UserId { get; set; }
    }
}
