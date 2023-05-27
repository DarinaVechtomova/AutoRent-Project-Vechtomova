using System;

namespace AutoRent_Logic.Services
{
    public class Admin: User
    {
        public Admin() { }
        public Admin(string email, string fullName, DateTime dateOfBirth, string telephonNumber, string password, int id)
            : base(email, fullName, dateOfBirth, telephonNumber, password, id)
        { }
    }
}
