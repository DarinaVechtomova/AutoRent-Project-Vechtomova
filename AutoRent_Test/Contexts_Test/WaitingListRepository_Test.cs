using AutoRent_Logic.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using System.IO;
using AutoRent_Logic;

namespace AutoRent_Test.Contexts_Test
{
    [TestClass]
    public class WaitingListRepository_Test
    {
        [TestMethod]
        public void AddWaiting_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);           
            // Act
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);

            // Assert
            Assert.AreEqual(1, db.WaitingForConfirmationList.Count);
            Assert.AreEqual(booking, db.WaitingForConfirmationList[0]);
        }

        [TestMethod]
        public void FindByIdWaiting_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 10);
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingForBookingConfirmation booking2 = new WaitingForBookingConfirmation(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            // Act
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);
            waitingRepository.Add(booking2);
            WaitingForBookingConfirmation result = waitingRepository.FindByID(2);
            // Assert
            Assert.AreEqual(booking2, result);
        }

        [TestMethod]
        public void FindByIdWaiting_ReturnNull()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 10);
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingForBookingConfirmation booking2 = new WaitingForBookingConfirmation(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            // Act
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);
            waitingRepository.Add(booking2);
            WaitingForBookingConfirmation result = waitingRepository.FindByID(6);
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllWaiting_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 10);
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingForBookingConfirmation booking2 = new WaitingForBookingConfirmation(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            List<WaitingForBookingConfirmation> expectedBooking = new List<WaitingForBookingConfirmation>()
            {
               booking,
               booking2
            };
            // Act
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);
            waitingRepository.Add(booking2);
            var actualBooking = waitingRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedBooking, actualBooking.ToList());
        }

        [TestMethod]
        public void GetAllWaiting_Null()
        {
            // Arrange
            DataBase db = new DataBase();
            List<WaitingForBookingConfirmation> expectedBooking = new List<WaitingForBookingConfirmation>();
            // Act
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            var actualBooking = waitingRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedBooking, actualBooking.ToList());
        }

        [TestMethod]
        public void RemoveWaiting_ReturnTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 10);
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingForBookingConfirmation booking2 = new WaitingForBookingConfirmation(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            // Act
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);
            waitingRepository.Add(booking2);
            bool result = waitingRepository.Remove(booking);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveNullWaiting_ReturnFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            // Act
            bool result = waitingRepository.Remove(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataWaiting_ValidData_ReturnsTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);
            // Act
            WaitingForBookingConfirmation updatedBooking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Novhorodska Street 4", 1, true);
            bool result = waitingRepository.UpData(updatedBooking);

            // Assert
            Assert.IsTrue(result);
            var expectedBooking = waitingRepository.FindByID(1);
            Assert.AreEqual(updatedBooking, expectedBooking);
        }

        [TestMethod]
        public void UpDataWaiting_EntityIsNull_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            // Act
            bool result = waitingRepository.UpData(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataWaiting_EntityDoesNotExist_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            // Act
            bool result = waitingRepository.UpData(booking);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveData_WritesToFile()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            string testPath = Path.Combine(Path.GetTempPath(), "test_booking.json");
            waitingRepository.Add(booking);

            // Act            
            waitingRepository.SaveData(db.WaitingForConfirmationList, testPath);

            // Assert
            Assert.IsTrue(File.Exists(Path.GetFullPath(testPath)));
        }

        [TestMethod]
        public void SaveData_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            string testPath = Path.Combine(Path.GetTempPath(), "test/booking.json");
            waitingRepository.Add(booking);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => waitingRepository.SaveData(db.WaitingForConfirmationList, testPath));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnTrucks_WhenFileExists()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 10);
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5, "Assets.Toyota");
            WaitingForBookingConfirmation booking = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            WaitingForBookingConfirmation booking2 = new WaitingForBookingConfirmation(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            waitingRepository.Add(booking);
            waitingRepository.Add(booking2);
            string testPath = Path.Combine(Path.GetTempPath(), "test1.json");
            waitingRepository.SaveData(db.WaitingForConfirmationList, testPath);

            // Act
            List<WaitingForBookingConfirmation> result = waitingRepository.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(db.WaitingForConfirmationList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.DeliveryAddress == booking.DeliveryAddress));
            Assert.IsTrue(result.Any(t => t.OrderId == booking2.OrderId));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            DataBase db = new DataBase();
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => waitingRepository.ReadFromJson(testPath));
        }
    }
}
