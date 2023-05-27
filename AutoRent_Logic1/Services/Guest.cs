using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using AutoRent_Presentation.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRent_Logic.Services
{
    public class Guest
    {
        private DataBase _context;
        public Guest(DataBase context)
        {
            _context = context;
        }
        public bool SignUp(string email, string fullName, DateTime dateOfBirth, string telephoneNumber, string password)
        {
            if (email.Contains("@enauto.com"))
            {
                if (_context.AdminsList.Any(u => u.Email == email))
                {
                    throw new Exception("This admin already exist");
                }
                else
                {
                    int id = _context.AdminsList.Count+1;
                    AdminAuthentication.IsAuthenticated = true;
                    _context.AdminsList.Add(new Admin(email, fullName, dateOfBirth, telephoneNumber, password, id));
                    return true;
                }
            }
            else
            {
                List<RegisteredUser> RegistredList = _context.RegisteredList.ToList();
                if (RegistredList.Any(u => u.Email == email))
                {
                    throw new Exception("This user already exist");
                }
                else
                {
                    int id = RegistredList.Count + 1;
                    UserAuthentication.ID = id;
                    UserAuthentication.IsAuthentication = true;
                    RegisteredRepository registeredRepository = new RegisteredRepository(_context);
                    RegisteredUser registeredUser = new RegisteredUser(email, fullName, dateOfBirth, telephoneNumber, password, id);
                    registeredRepository.Add(registeredUser);
                    return true;
                }
            }
        }
    }
}
