using AutoRent_Logic.Contexts.Interfaces;
using AutoRent_Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AutoRent_Logic.Contexts
{
    public class TruckRepository : IRepository<Truck>, IContextData<Truck>
    {
        private DataBase _context;
        public TruckRepository(DataBase context)
        {
            _context = context;
        }
        public void Add(Truck entity)
        {
            _context.TruckList.Add(entity);
        }

        public Truck FindByID(int id)
        {
            if(id != 0)
            {
                var truck = _context.TruckList.FirstOrDefault(x => x.Id == id);
                if (truck != null)
                    return truck;
            }
            return default;
        }

        public IEnumerable<Truck> GetAll()
        {
            return _context.TruckList;
        }
        public bool Remove(Truck entity)
        {
            if (entity != null)
            {
                _context.TruckList.Remove(entity);
                return true;
            }
            return false;
        }
        public bool UpData(Truck entity)
        {
            try
            {
                if (entity != null)
                {
                    int index = _context.TruckList.FindIndex(x => x.Id == entity.Id);
                    if (index != -1)
                    {
                        _context.TruckList[index] = entity;
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

        public List<Truck> ReadFromJson(string path)
        {            
            try
            {
                List<Truck> trucks = new List<Truck>(); 
                string jsonString = File.ReadAllText(path);
                trucks = JsonSerializer.Deserialize<List<Truck>>(jsonString);
                
                return trucks;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }                      
        }

        public void SaveData(List<Truck> trucks, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<Truck>>(trucks, new JsonSerializerOptions
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