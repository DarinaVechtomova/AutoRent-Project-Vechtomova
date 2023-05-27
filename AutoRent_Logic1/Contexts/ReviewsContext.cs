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
    public class ReviewsContext : IContextData<Reviews>
    {
        public List<Reviews> ReadFromJson(string path)
        {
            try
            {
                List<Reviews> reviews = new List<Reviews>();
                string jsonString = File.ReadAllText(path);
                reviews = JsonSerializer.Deserialize<List<Reviews>>(jsonString);
                return reviews;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading data from JSON file at path {path}: {ex.Message}");
            }
        }

        public void SaveData(List<Reviews> reviews, string path)
        {
            try
            {
                string jsonstring = JsonSerializer.Serialize<List<Reviews>>(reviews, new JsonSerializerOptions
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
