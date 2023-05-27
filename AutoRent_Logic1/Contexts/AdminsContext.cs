using AutoRent_Logic.Contexts.Interfaces;
using AutoRent_Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AutoRent_Logic.Contexts
{
    public class AdminsContext : IContextData<Admin>
    {
        public List<Admin> ReadFromJson(string path)
        {
            try
            {
                List<Admin> admins = new List<Admin>();
                string jsonString = File.ReadAllText(path);
                admins = JsonSerializer.Deserialize<List<Admin>>(jsonString);
                return admins;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }
        }

        public void SaveData(List<Admin> admins, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<Admin>>(admins, new JsonSerializerOptions
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
