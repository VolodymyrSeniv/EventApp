using MauiAppB.Models;
using SQLite;
using System;
using System.Collections.Generic;

namespace MauiAppB.Services
{
    public class GroupService
    {
            SQLiteConnection conn;
            string _dbPath;
            public string StatusMessage;
            int result = 0;

            public GroupService(string dbPath)
            {
                _dbPath = dbPath;
            }

            private void Init()
            {
                if (conn != null)
                    return;

                conn = new SQLiteConnection(_dbPath);
                conn.CreateTable<Group>();
            }

        public Group GetGroup(int id)
        {
            try
            {
                Init();
                return conn.Table<Group>().FirstOrDefault(q => q.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve group.";
                return null;
            }
        }

        // Fix B: return groups where user is creator OR a member
        public List<Group> GetGroups(int userId)
        {
            try
            {
                Init();
                // Filtering groups by the CreatorId column
                return conn.Table<Group>()
                           .Where(g => g.CreatorId == userId)
                           .ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to retrieve data: {ex.Message}";
                return new List<Group>();
            }
        }

        // Przy dodawaniu grupy upewnij się, że obiekt ma przypisane CreatorId
        public void AddGroup(Group groupka)
        {
            try
            {
                Init();
                if (groupka == null) throw new Exception("Invalid Group Record");

                // Ważne: CreatorId musi być ustawione w ViewModelu przed wywołaniem tej metody!
                int result = conn.Insert(groupka);
                StatusMessage = result == 0 ? "Insert Failed" : "Insert Successful";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to insert data: {ex.Message}";
            }
        }

        public int DeleteGroup(int id)
        {
            try
            {
                Init();
                return conn.Delete<Group>(id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data.";
                return 0;
            }
        }

        public int UpdateGroup(Group groupka)
        {
            try
            {
                Init();
                if (groupka == null || groupka.Id == 0)
                    throw new Exception("Invalid Group Record for update");

                int result = conn.Update(groupka);
                StatusMessage = result == 0 ? "Update Failed" : "Update Successful";
                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to update data: {ex.Message}";
                return 0;
            }
        }
    }
}