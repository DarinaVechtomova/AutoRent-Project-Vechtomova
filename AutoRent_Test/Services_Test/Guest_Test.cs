using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoRent_Test.Services_Test
{
    [TestClass]
    public class Guest_Test
    {
        [TestMethod]
        public void SignUp_SuccessfulRegistration_ReturnsTrue()
        {
            // Arrange            
            string email = "newuser@gmail.com";
            string fullName = "Stepan Shvets";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            string telephoneNumber = "+380951111111";
            string password = "passw0rd1";
            // Act
            DataBase db = new DataBase();
            Guest gest = new Guest(db);
            bool result = gest.SignUp(email, fullName, dateOfBirth, telephoneNumber, password);
            // Assert
            Assert.IsTrue(result);               
        }

        [TestMethod]
        public void SignUp_UserAlreadyRegistered_ReturnsFalse()
        {
            // Arrange            
            string email = "newuser@gmail.com";
            string fullName = "Stepan Shvets";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            string telephoneNumber = "+380951111111";
            string password = "passw0rd1";
            int id = 1;
            DataBase db = new DataBase();
            RegisteredUser registeredUser = new RegisteredUser(email, fullName, dateOfBirth, telephoneNumber, password, id);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(registeredUser);
            Guest guest = new Guest(db);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => guest.SignUp(email, fullName, dateOfBirth, telephoneNumber, password));
        }
    }
}
