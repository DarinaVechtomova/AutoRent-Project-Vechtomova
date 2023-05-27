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
    public class TruckRepository_Test
    {
        [TestMethod]
        public void AddTruck_True()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);

            // Act
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);

            // Assert
            Assert.AreEqual(1, db.TruckList.Count);
            Assert.AreEqual(truck, db.TruckList[0]);
        }

        [TestMethod]
        public void FindByIdTruck_True()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            Truck truck1 = new Truck(2, "Scania R620", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 39.4, 150, 1200, 95);

            // Act
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);
            truckRepository.Add(truck1);
            Truck result = truckRepository.FindByID(2);
            // Assert
            Assert.AreEqual(truck1, result);
        }

        [TestMethod]
        public void FindByIdTruck_ReturnNull()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            Truck truck1 = new Truck(2, "Scania R620", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 39.4, 150, 1200, 95);

            // Act
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);
            truckRepository.Add(truck1);
            Truck result = truckRepository.FindByID(5);
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllTruck_True()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            Truck truck1 = new Truck(2, "Scania R620", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 39.4, 150, 1200, 95);
            List<Truck> expectedTrucks = new List<Truck>()
            {
               truck,
               truck1
            };
            // Act
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);
            truckRepository.Add(truck1);
            var actualTrucks = truckRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedTrucks, actualTrucks.ToList());
        }

        [TestMethod]
        public void GetAllTruck_Null()
        {
            // Arrange
            DataBase db = new DataBase();
            List<Truck> expectedTruck = new List<Truck>();
            // Act
            TruckRepository truckRepository = new TruckRepository(db);

            var actualTruck = truckRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedTruck, actualTruck.ToList());
        }

        [TestMethod]
        public void RemoveTruck_ReturnTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            Truck truck1 = new Truck(2, "Scania R620", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 39.4, 150, 1200, 95);

            // Act
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);
            truckRepository.Add(truck1);
            bool result = truckRepository.Remove(truck);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveNullTruck_ReturnFalse()
        {
            // Arrange
            DataBase db = new DataBase();          
            // Act
            TruckRepository truckRepository = new TruckRepository(db);          
            bool result = truckRepository.Remove(null);
            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void UpDataTruck_ValidData_ReturnsTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);

            // Act
            Truck updatedTruck = new Truck(1, "Volvo FH16", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 42.0, 160, 1500, 95);
            bool result = truckRepository.UpData(updatedTruck);

            // Assert
            Assert.IsTrue(result);
            var expectedTruck = truckRepository.FindByID(1);
            Assert.AreEqual(updatedTruck, expectedTruck);
        }

        [TestMethod]
        public void UpDataTruck_EntityIsNull_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);

            // Act
            bool result = truckRepository.UpData(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataTruck_EntityDoesNotExist_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);

            // Act
            bool result = truckRepository.UpData(truck);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveData_WritesToFile()
        {
            // Arrange
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            string testPath = Path.Combine(Path.GetTempPath(), "test_truck.json");

            // Act
            truckRepository.Add(truck);
            truckRepository.SaveData(db.TruckList, testPath);

            // Assert
            Assert.IsTrue(File.Exists(Path.GetFullPath(testPath)));            
        }

        [TestMethod]
        public void SaveData_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90);
            string testPath = Path.Combine(Path.GetTempPath(), "n/test.json");

            truckRepository.Add(truck);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => truckRepository.SaveData(db.TruckList, testPath));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnTrucks_WhenFileExists()
        {
            // Arrange
            DataBase db = new DataBase();
            Truck truck = new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90, "Assers.Volvo");
            Truck truck1 = new Truck(2, "Scania R620", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 39.4, 150, 1200, 95, "Assers.Scania");
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(truck);
            truckRepository.Add(truck1);
            string testPath = Path.Combine(Path.GetTempPath(), "test.json"); ;
            truckRepository.SaveData(db.TruckList, testPath);

            // Act
            List<Truck> result = truckRepository.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(db.TruckList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.Id == truck.Id));
            Assert.IsTrue(result.Any(t => t.VehicleName == truck1.VehicleName));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => truckRepository.ReadFromJson(testPath));
        }
    }
}
