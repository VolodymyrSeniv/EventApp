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
        int result = 0;

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

        public Event GetEvent(int id) 
        {
            try 
            {
                Init();
                return conn.Table<Event>().FirstOrDefault(q => q.Id == id);
            }
            catch(Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }
            return null;
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
        public void AddEvent(Event eventik)
        {
            try
            {
                Init();

                if (eventik == null)
                    throw new Exception("Invalid Event Record");
                result = conn.Insert(eventik);
                StatusMessage = result == 0 ? "Insert Failed" : "Insert Successfull"; 

            }
            catch (Exception ex) 
            {
                StatusMessage = "Failed to insert data.";
            }
        }

        public int DeleteEvent(int id) 
        {
            try
            {
                Init();
                return conn.Table<Event>().Delete(q => q.Id == id);
            }
            catch (Exception) 
            {
                StatusMessage = "Failed to delete data.";
            }
            return 0;
        }
    }
}
