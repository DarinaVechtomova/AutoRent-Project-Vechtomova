using AutoRent_Logic.Contexts.Interfaces;
using AutoRent_Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AutoRent_Logic.Contexts
{
    public class OrderRepository : IRepository<Orders>, IContextData<Orders>
    {
        private DataBase _context;
        public OrderRepository(DataBase context)
        {
            _context = context;
        }
        public void Add(Orders entity)
        {
            _context.OrdersList.Add(entity);
        }

        public Orders FindByID(int id)
        {
            if (id != 0)
            {
                var order = _context.OrdersList.FirstOrDefault(x => x.OrderId == id);
                if (order != null)
                    return order;
            }
            return default;
        }

        public IEnumerable<Orders> GetAll()
        {
            return _context.OrdersList;
        }
        public bool Remove(Orders entity)
        {
            if (entity != null)
            {
                _context.OrdersList.Remove(entity);
                return true;
            }
            return false;
        }
        public bool UpData(Orders entity)
        {
            try
            {
                if (entity != null)
                {
                    int index = _context.OrdersList.FindIndex(x => x.OrderId == entity.OrderId);
                    if (index != -1)
                    {
                        _context.OrdersList[index] = entity;
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

        public List<Orders> ReadFromJson(string path)
        {
            try
            {
                List<Orders> orders = new List<Orders>();
                string jsonString = File.ReadAllText(path);
                orders = JsonSerializer.Deserialize<List<Orders>>(jsonString);
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }
        }

        public void SaveData(List<Orders> orders, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<Orders>>(orders, new JsonSerializerOptions
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
    }
}
