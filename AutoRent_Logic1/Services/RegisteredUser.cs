using AutoRent_Logic.Services;
using System;

namespace AutoRent_Logic
{
    public class RegisteredUser : User
    {
        public RegisteredUser() { }
        public RegisteredUser(string email, string fullName, 
            DateTime dateOfBirth, string telephonNumber, string password, int id)
            : base(email, fullName, dateOfBirth, telephonNumber, password, id)
        { }
    }
}
