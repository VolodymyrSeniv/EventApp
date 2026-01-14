using System;
using System.Collections.Generic;
using System.Text;
using MauiApp1.Models;
using SQLite;

namespace MauiApp1.Services
{
    public class EventService
    {
        SQLiteConnection conn;
        string _dbPath;
        public string StatusMessage;

        public EventService(string dbPath)
        {
            _dbPath = dbPath;
        }

        private void Init() 
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Event>();
        }
        public List<Event> GetEvents(){
            try 
            {
                Init();
                return conn.Table<Event>().ToList();
            }
            catch(Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }
            return new List<Event>();
        }
    }
}
