using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoRent_Test.Contexts_Test
{
    [TestClass]
    public class PassengerCarRepository_Test
    {
        [TestMethod]
        public void AddPassengerCar_True()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);

            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);

            // Assert
            Assert.AreEqual(1, db.PassengerCarList.Count);
            Assert.AreEqual(passengerCar, db.PassengerCarList[0]);
        }

        [TestMethod]
        public void FindByIdPassengerCar_True()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            PassengerCar passengerCar2 = new PassengerCar(2, "Renault Logan", "1.5 l", TransmissionType.Manual , FuelType.Diesel, 5.8, 30, 300, 5);

            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            passengerCarRepository.Add(passengerCar2);
            PassengerCar result = passengerCarRepository.FindByID(2);
            // Assert
            Assert.AreEqual(passengerCar2, result);
        }

        [TestMethod]
        public void FindByIdPassengerCar_ReturnNull()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            PassengerCar passengerCar2 = new PassengerCar(2, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);

            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            passengerCarRepository.Add(passengerCar2);
            PassengerCar result = passengerCarRepository.FindByID(5);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllPassengerCar_True()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            PassengerCar passengerCar2 = new PassengerCar(2, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);
            List<PassengerCar> expectedPassengerCar = new List<PassengerCar>()
            {
               passengerCar,
               passengerCar2
            };
            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            passengerCarRepository.Add(passengerCar2);
            var actualPassengerCar = passengerCarRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedPassengerCar, actualPassengerCar.ToList());
        }

        [TestMethod]
        public void GetAllPassengerCar_NullCar()
        {
            // Arrange
            DataBase db = new DataBase();
            List<PassengerCar> expectedPassengerCar = new List<PassengerCar>();
            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            
            var actualPassengerCar = passengerCarRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedPassengerCar, actualPassengerCar.ToList());
        }

        [TestMethod]
        public void RemovePassengerCar_ReturnTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            PassengerCar passengerCar2 = new PassengerCar(2, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);

            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            passengerCarRepository.Add(passengerCar2);
            bool result = passengerCarRepository.Remove(passengerCar);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveNullPassengerCar_ReturnFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            // Act
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            bool result = passengerCarRepository.Remove(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataPassengerCar_ValidData_ReturnsTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);

            // Act
            PassengerCar updatePassengerCar = new PassengerCar(1, "Renault Logan", "2 l", TransmissionType.Automatic, FuelType.Diesel, 5.8, 40, 350, 5);
            bool result = passengerCarRepository.UpData(updatePassengerCar);

            // Assert
            Assert.IsTrue(result);
            var expectedPassengerCar = passengerCarRepository.FindByID(1);
            Assert.AreEqual(updatePassengerCar, expectedPassengerCar);
        }

        [TestMethod]
        public void UpDataPassengerCar_EntityIsNull_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);

            // Act
            bool result = passengerCarRepository.UpData(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataPassengerCar_EntityDoesNotExist_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            PassengerCar passengerCar = new PassengerCar(1, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);

            // Act
            bool result = passengerCarRepository.UpData(passengerCar);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveDataPassengerCar_WritesToFile()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            PassengerCar passengerCar = new PassengerCar(1, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);
            string testPath = Path.Combine(Path.GetTempPath(), "test_passengerCar.json");

            // Act
            passengerCarRepository.Add(passengerCar);
            passengerCarRepository.SaveData(db.PassengerCarList, testPath);

            // Assert
            Assert.IsTrue(File.Exists(Path.GetFullPath(testPath)));
        }

        [TestMethod]
        public void SaveDataPassengerCar_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            PassengerCar passengerCar = new PassengerCar(1, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5);
            string testPath = Path.Combine(Path.GetTempPath(), "1/passengerCar.json");

            passengerCarRepository.Add(passengerCar);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => passengerCarRepository.SaveData(db.PassengerCarList, testPath));
        }

        [TestMethod]
        public void ReadFromJsonPassengerCar_ShouldReturnTrucks_WhenFileExists()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5, "Assets.Toyota");
            PassengerCar passengerCar2 = new PassengerCar(2, "Renault Logan", "1.5 l", TransmissionType.Manual, FuelType.Diesel, 5.8, 30, 300, 5, "Assets.Renault");
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            passengerCarRepository.Add(passengerCar2);
            string testPath = Path.Combine(Path.GetTempPath(), "test.json"); ;
            passengerCarRepository.SaveData(db.PassengerCarList, testPath);

            // Act
            List<PassengerCar> result = passengerCarRepository.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(db.PassengerCarList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.Id == passengerCar.Id));
            Assert.IsTrue(result.Any(t => t.VehicleName == passengerCar2.VehicleName));
        }

        [TestMethod]
        public void ReadFromJsonPassengerCar_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            DataBase db = new DataBase();
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => passengerCarRepository.ReadFromJson(testPath));
        }
    }
}
