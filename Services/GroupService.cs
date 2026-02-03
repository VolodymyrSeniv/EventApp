using MauiAppB.Models;
using SQLite;
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
                    StatusMessage = "Failed to retrieve data.";
                }
                return null;
            }

            public List<Group> GetGroups()
            {
                try
                {
                    Init();
                    return conn.Table<Group>().ToList();
                }
                catch (Exception)
                {
                    StatusMessage = "Failed to retrieve data.";
                }
                return new List<Group>();
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

            public int UpdateGroup(Group groupka)
            {
                try
                {
                    Init();
                    if (groupka == null || groupka.Id == 0)
                        throw new Exception("Invalid Group Record for update");

                    result = conn.Update(groupka);
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