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
    public class WaitingListRepository : IRepository<WaitingForBookingConfirmation>, IContextData<WaitingForBookingConfirmation>
    {
        private DataBase _context;
        public WaitingListRepository(DataBase context)
        {
            _context = context;
        }
        public void Add(WaitingForBookingConfirmation entity)
        {
            _context.WaitingForConfirmationList.Add(entity);
        }

        public WaitingForBookingConfirmation FindByID(int id)
        {
            if (id != 0)
            {
                var booking = _context.WaitingForConfirmationList.FirstOrDefault(x => x.OrderId == id);
                if (booking != null)
                    return booking;
            }
            return default;
        }

        public IEnumerable<WaitingForBookingConfirmation> GetAll()
        {
            return _context.WaitingForConfirmationList;
        }
       
        public bool Remove(WaitingForBookingConfirmation entity)
        {
            if (entity != null)
            {
                _context.WaitingForConfirmationList.Remove(entity);
                return true;
            }
            return false;
        }
       
        public bool UpData(WaitingForBookingConfirmation entity)
        {
            try
            {
                if (entity != null)
                {
                    //_context.TruckList.Update(entity);
                    //return true;
                    int index = _context.WaitingForConfirmationList.FindIndex(x => x.OrderId == entity.OrderId);
                    if (index != -1)
                    {
                        _context.WaitingForConfirmationList[index] = entity;
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
        public List<WaitingForBookingConfirmation> ReadFromJson(string path)
        {
            try
            {
                List<WaitingForBookingConfirmation> waitings = new List<WaitingForBookingConfirmation>();
                string jsonString = File.ReadAllText(path);
                waitings = JsonSerializer.Deserialize<List<WaitingForBookingConfirmation>>(jsonString);
                return waitings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }
        }
        public void SaveData(List<WaitingForBookingConfirmation> booking, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<WaitingForBookingConfirmation>>(booking, new JsonSerializerOptions
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
