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
    public class PassengerCarRepository : IRepository<PassengerCar>, IContextData<PassengerCar>
    {
        private DataBase _context;
        public PassengerCarRepository(DataBase context)
        {
            _context = context;
        }
        public void Add(PassengerCar entity)
        {
            _context.PassengerCarList.Add(entity);
        }

        public PassengerCar FindByID(int id)
        {
            if (id != 0)
            {
                var passengerCar = _context.PassengerCarList.FirstOrDefault(x => x.Id == id);
                if (passengerCar != null)
                    return passengerCar;
            }
            return default;
        }

        public IEnumerable<PassengerCar> GetAll()
        {
            return _context.PassengerCarList;
        }
        
        public bool Remove(PassengerCar entity)
        {
            if (entity != null)
            {
                _context.PassengerCarList.Remove(entity);
                return true;
            }
            return false;
        }   
        
        public bool UpData(PassengerCar entity)
        {
            try
            {
                if (entity != null)
                {
                    int index = _context.PassengerCarList.FindIndex(x => x.Id == entity.Id);
                    if (index != -1)
                    {
                        _context.PassengerCarList[index] = entity;
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

        public void SaveData(List<PassengerCar> passengerCars, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<PassengerCar>>(passengerCars, new JsonSerializerOptions
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

        public List<PassengerCar> ReadFromJson(string path)
        {
            try
            {
                List<PassengerCar> passengers = new List<PassengerCar>();
                string jsonString = File.ReadAllText(path);
                passengers = JsonSerializer.Deserialize<List<PassengerCar>>(jsonString);
                return passengers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }
        }
    }
}
