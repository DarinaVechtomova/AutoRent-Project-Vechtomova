using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoRent_Test.Contexts_Test
{
    [TestClass]
    public class AdminsContext_Test
    {
        [TestMethod]
        public void SaveDataAdmins_WritesToFile()
        {
            // Arrange           
            AdminsContext adminsContext = new AdminsContext();
            DateTime birthday = new DateTime(1986, 05, 17);
            Admin admin = new Admin("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            string testPath = Path.Combine(Path.GetTempPath(), "test_reviews.json");
            DataBase dataBase = new DataBase();
            dataBase.AdminsList.Add(admin);

            // Act         
            adminsContext.SaveData(dataBase.AdminsList, testPath);

            // Assert
            Assert.IsTrue(File.Exists(Path.GetFullPath(testPath)));
        }

        [TestMethod]
        public void SaveDataAdmins_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            AdminsContext adminsContext = new AdminsContext();
            DateTime birthday = new DateTime(1986, 05, 17);
            Admin admin = new Admin("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            string testPath = Path.Combine(Path.GetTempPath(), "1/admins.json");
            DataBase dataBase = new DataBase();
            dataBase.AdminsList.Add(admin);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => adminsContext.SaveData(dataBase.AdminsList, testPath));
        }

        [TestMethod]
        public void ReadFromJsonAdmins_ShouldReturnTrucks_WhenFileExists()
        {
            // Arrange
            AdminsContext adminsContext = new AdminsContext();
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            Admin admin = new Admin("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            Admin admin2 = new Admin("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            DataBase dataBase = new DataBase();
            dataBase.AdminsList.Add(admin);
            dataBase.AdminsList.Add(admin2);
            string testPath = Path.Combine(Path.GetTempPath(), "test.json"); ;
            adminsContext.SaveData(dataBase.AdminsList, testPath);

            // Act
            List<Admin> result = adminsContext.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataBase.AdminsList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.FullName == admin.FullName));
            Assert.IsTrue(result.Any(t => t.Email == admin2.Email));
        }

        [TestMethod]
        public void ReadFromJsonAdmins_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            AdminsContext adminsContext = new AdminsContext();
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => adminsContext.ReadFromJson(testPath));
        }
    }
}
