using MauiAppB.Models;
using SQLite;

namespace MauiAppB.Services
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
            if (conn != null) return;
            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Event>();
        }

        public List<Event> GetEventsForGroup(int groupId)
        {
            Init();
            try
            {
                return conn.Table<Event>().Where(e => e.GroupId == groupId).ToList();
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }
            return new List<Event>();
        }

        public Event GetEvent(int id)
        {
            try
            {
                Init();
                return conn.Table<Event>().FirstOrDefault(q => q.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }
            return null;
        }
        public void AddGroup(Group groupka)
        {
            try
            {
                Init();

                if (groupka == null)
                    throw new Exception("Invalid Group Record");
                result = conn.Insert(groupka);
                StatusMessage = result == 0 ? "Insert Failed" : "Insert Successfull";

            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to insert data.";
            }
        }

        public int DeleteGroup(int id)
        {
            try
            {
                Init();
                return conn.Table<Group>().Delete(q => q.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data.";
            }
            return 0;
        }
        // 2. DODAJ WYDARZENIE
        public void AddEvent(Event newEvent)
        {
            try 
            {
                if (newEvent == null)
                    throw new Exception("Invalid Event Record");
                Init();
                result = conn.Insert(newEvent);
                StatusMessage = result == 0 ? "Insert Failed" : "Insert Successfull";
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to insert data.";
            }
}
        public void DeleteEvent(int eventId)
        {
            try
            {
                Init();
                conn.Delete<Event>(eventId);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data.";
            }
        }

        // W pliku EventService.cs dodaj:
        public int UpdateEvent(Event eventToUpdate)
        {
            try
            {
                Init();
                if (eventToUpdate == null || eventToUpdate.Id == 0)
                    throw new Exception("Nieprawidłowy rekord wydarzenia");

                result = conn.Update(eventToUpdate);
                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Błąd aktualizacji: {ex.Message}";
                return 0;
            }
        }
    }
}