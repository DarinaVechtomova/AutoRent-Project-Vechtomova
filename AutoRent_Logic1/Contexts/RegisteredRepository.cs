using AutoRent_Logic.Contexts.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AutoRent_Logic.Contexts
{
    public class RegisteredRepository : IRepository<RegisteredUser>, IContextData<RegisteredUser>
    {
        private DataBase _context;
        public RegisteredRepository(DataBase context)
        {
            _context = context;
        }

        public void Add(RegisteredUser entity)
        {
            _context.RegisteredList.Add(entity);
        }

        public RegisteredUser FindByID(int id)
        {
            if (id != 0)
            {
                var user = _context.RegisteredList.FirstOrDefault(x => x.Id == id);
                if (user != null)
                    return user;
            }
            return default;
        }

        public IEnumerable<RegisteredUser> GetAll()
        {
            return _context.RegisteredList;
        }

        public bool Remove(RegisteredUser entity)
        {
            if (entity != null)
            {
                _context.RegisteredList.Remove(entity);
                return true;
            }
            return false;
        }

        public bool UpData(RegisteredUser entity)
        {
            try
            {
                if (entity != null)
                {
                    int index = _context.RegisteredList.FindIndex(x => x.Id == entity.Id);
                    if (index != -1)
                    {
                        _context.RegisteredList[index] = entity;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public void SaveData(List<RegisteredUser> users, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<RegisteredUser>>(users, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                });

                File.WriteAllText(path, jsonstring);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving data to JSON file at path {path}: {ex.Message}");
            }
        }

        public List<RegisteredUser> ReadFromJson(string path)
        {
            try
            {
                List<RegisteredUser> users = new List<RegisteredUser>();
                string jsonString = File.ReadAllText(path);
                users = JsonSerializer.Deserialize<List<RegisteredUser>>(jsonString);
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }
        }
    }
}
