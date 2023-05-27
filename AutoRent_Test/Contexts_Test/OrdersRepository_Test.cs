using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.System;

namespace AutoRent_Test.Contexts_Test
{
    [TestClass]
    public class OrdersRepository_Test
    {
        [TestMethod]
        public void AddOrders_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            // Act
            OrderRepository ordersRepositority = new OrderRepository(db);
            ordersRepositority.Add(order);

            // Assert
            Assert.AreEqual(1, db.OrdersList.Count);
            Assert.AreEqual(order, db.OrdersList[0]);
        }

        [TestMethod]
        public void FindByIdOrders_True()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 10);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            Orders order = new Orders(passengerCar,user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            Orders order2 = new Orders(passengerCar, user, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            // Act
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(order);
            orderRepository.Add(order2);
            Orders result = orderRepository.FindByID(2);
            // Assert
            Assert.AreEqual(order2, result);
        }

        [TestMethod]
        public void FindByIdOrders_ReturnNull()
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
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            Orders order2 = new Orders(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            // Act
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(order);
            orderRepository.Add(order2);
            Orders result = orderRepository.FindByID(6);
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllOrders_True()
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
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            Orders order2 = new Orders(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            List<Orders> expectedOrder = new List<Orders>()
            {
               order,
               order2
            };
            // Act
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(order);
            orderRepository.Add(order2);
            var actualOrder = orderRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedOrder, actualOrder.ToList());
        }

        [TestMethod]
        public void GetAllOrders_Null()
        {
            // Arrange
            DataBase db = new DataBase();
            List<Orders> expectedOrder = new List<Orders>();
            // Act
            OrderRepository orderRepository = new OrderRepository(db);
            var actualOrder = orderRepository.GetAll();
            // Assert
            CollectionAssert.AreEqual(expectedOrder, actualOrder.ToList());
        }

        [TestMethod]
        public void RemoveOrders_ReturnTrue()
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
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            Orders order2 = new Orders(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(order);
            orderRepository.Add(order2);
            bool result = orderRepository.Remove(order);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveNullOrders_ReturnFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            OrderRepository orderRepository = new OrderRepository(db);
            // Act
            bool result = orderRepository.Remove(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataOrders_ValidData_ReturnsTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            Orders booking = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);            
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(booking);
            // Act
            Orders updatedOrder = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Novhorodska Street 4", 1, true);
            bool result = orderRepository.UpData(updatedOrder);

            // Assert
            Assert.IsTrue(result);
            var expectedOrder = orderRepository.FindByID(1);
            Assert.AreEqual(updatedOrder, expectedOrder);
        }

        [TestMethod]
        public void UpDataOrders_EntityIsNull_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            OrderRepository orderRepository = new OrderRepository(db);
            // Act
            bool result = orderRepository.UpData(null);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpDataOrders_EntityDoesNotExist_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            PassengerCar passengerCar = new PassengerCar(1, "Toyota Camry", "2.5L 4-cylinder engine", TransmissionType.Automatic, FuelType.Petrol, 7.8, 70, 1000, 5);
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            OrderRepository orderRepository = new OrderRepository(db);
            // Act
            bool result = orderRepository.UpData(order);
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
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            OrderRepository orderRepository = new OrderRepository(db);
            string testPath = Path.Combine(Path.GetTempPath(), "test_booking.json");
            orderRepository.Add(order);

            // Act            
            orderRepository.SaveData(db.OrdersList, testPath);

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
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            OrderRepository orderRepository = new OrderRepository(db);
            string testPath = Path.Combine(Path.GetTempPath(), "test/booking.json");
            orderRepository.Add(order);

            // Act + Assert
            Assert.ThrowsException<Exception>(() => orderRepository.SaveData(db.OrdersList, testPath));
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
            Orders order = new Orders(passengerCar, user, rentalStartDate, dateOfEndOfLease, "Akademika Pavlova Street 3", 1, true);
            Orders order2 = new Orders(passengerCar, user2, rentalStartDate2, dateOfEndOfLease2, "Akademika Pavlova Street 10", 2, true);
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(order);
            orderRepository.Add(order2);
            string testPath = Path.Combine(Path.GetTempPath(), "test.json");
            orderRepository.SaveData(db.OrdersList, testPath);

            // Act
            List<Orders> result = orderRepository.ReadFromJson(testPath);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(db.OrdersList.Count, result.Count);
            Assert.IsTrue(result.Any(t => t.DeliveryAddress == order.DeliveryAddress));
            Assert.IsTrue(result.Any(t => t.OrderId == order2.OrderId));
        }

        [TestMethod]
        public void ReadFromJson_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            // Arrange
            DataBase db = new DataBase();
            OrderRepository orderRepository = new OrderRepository(db);
            string testPath = "1.json";

            // Act + Assert
            Assert.ThrowsException<Exception>(() => orderRepository.ReadFromJson(testPath));
        }
    }
}
