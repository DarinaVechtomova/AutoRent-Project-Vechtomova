using System;
using System.Text.RegularExpressions;

namespace AutoRent_Logic.Services
{
    public abstract class User
    {
        private string email;
        private string fullName;
        private DateTime dateOfBirth;
        private string telephonNumber;
        private string password;
        
        public User() { }
        public User(string email, string fullName, DateTime dateOfBirth, string telephonNumber, string password, int id)
        {
            Email = email;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            TelephonNumber = telephonNumber;
            Password = password;
            Id = id;            
        }

        public int Id { get; set; }
        public string Email 
        {
            get
            {
                return email;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Email cannot be null or empty");
                }
                else if (!value.Contains("@"))
                {
                    throw new ArgumentException("Email must contain '@'");
                }
                email = value;
            }
        }
        public string FullName 
        {
            get
            {
                return fullName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be null or empty");
                }
                fullName = value;
            }
        }
        public DateTime DateOfBirth {
            get => dateOfBirth;
            set
            {
                if (value.Year < 1900)
                    throw new ArgumentException("Date of birth should be after 1900");
                int age = DateTime.Today.Year - value.Year;

                if (age < 18)
                    throw new ArgumentException("Age should be at least 18 years");
                dateOfBirth = value;
            }
        }
        public string TelephonNumber 
        {
            get
            {
                return telephonNumber;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Telephon number cannot be null or empty");
                }
                telephonNumber = value;
            }
        }
        public string Password 
        {
            get
            {
                return password;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Password cannot be null or empty");
                }
                if (!Regex.IsMatch(value, @"^(?=.*\d)(?=.*[a-zA-Z]).{6,}$"))
                {
                    throw new ArgumentException("Password must contain at least one digit and one letter, and be at least 6 characters long");
                }
                password = value;
            }
        }
    }
}