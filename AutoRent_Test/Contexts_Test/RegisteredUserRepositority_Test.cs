using AutoRent_Logic.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AutoRent_Logic;

namespace AutoRent_Test.Contexts_Test
{
    [TestClass]
    public class RegisteredUserRepositority_Test
    {
        [TestMethod]
        public void AddUser_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);

            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);

            // Assert
            Assert.AreEqual(1, db.RegisteredList.Count);
            Assert.AreEqual(user, db.RegisteredList[0]);
        }

        [TestMethod]
        public void FindByIdUser_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            registeredRepository.Add(user2);
            RegisteredUser result = registeredRepository.FindByID(2);
            // Assert
            Assert.AreEqual(user2, result);
        }

        [TestMethod]
        public void FindByIdUser_ReturnNull()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            registeredRepository.Add(user2);
            RegisteredUser result = registeredRepository.FindByID(5);
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllUser_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            List<RegisteredUser> expectedUser = new List<RegisteredUser>()
            {
               user,
               user2
            };
            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            registeredRepository.Add(user2);
            var actualUser = registeredRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedUser, actualUser.ToList());
        }

        [TestMethod]
        public void GetAllUser_Null()
        {
            // Arrange
            DataBase db = new DataBase();
            List<RegisteredUser> expectedUser = new List<RegisteredUser>();
            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            var actualUser = registeredRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedUser, actualUser.ToList());
        }

        [TestMethod]
        public void RemoveUser_ReturnTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            registeredRepository.Add(user2);
            bool result = registeredRepository.Remove(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveNullUser_ReturnFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            // Act
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            bool result = registeredRepository.Remove(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataUser_ValidData_ReturnsTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            // Act
            RegisteredUser updatedUser = new RegisteredUser("updateTest@gmail.com", "Stepan Shvets", birthday, "+380958989898", "passvOrd7", 1);
            bool result = registeredRepository.UpData(updatedUser);

            // Assert
            Assert.IsTrue(result);
            var expectedUser = registeredRepository.FindByID(1);
            Assert.AreEqual(updatedUser, expectedUser);
        }

        [TestMethod]
        public void UpDataUser_EntityIsNull_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            // Act
            bool result = registeredRepository.UpData(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataUser_EntityDoesNotExist_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            // Act
            bool result = registeredRepository.UpData(user);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveData_WritesToFile()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            string testPath = Path.Combine(Path.GetTempPath(), "test_user.json");
            registeredRepository.Add(user);

            // Act            
            registeredRepository.SaveData(db.RegisteredList, testPath);

            // Assert
            Assert.IsTrue(File.Exists(Path.GetFullPath(testPath)));
        }

        [TestMethod]
        public void SaveData_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            string testPath = Path.Combine(Path.GetTempPath(), "test/user.json");
            registeredRepository.Add(user);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => registeredRepository.SaveData(db.RegisteredList, testPath));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnTrucks_WhenFileExists()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            registeredRepository.Add(user2);
            string testPath = Path.Combine(Path.GetTempPath(), "test.json"); ;
            registeredRepository.SaveData(db.RegisteredList, testPath);

            // Act
            List<RegisteredUser> result = registeredRepository.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(db.RegisteredList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.Id == user.Id));
            Assert.IsTrue(result.Any(t => t.Email == user2.Email));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            DataBase db = new DataBase();
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => registeredRepository.ReadFromJson(testPath));
        }
    }
}
