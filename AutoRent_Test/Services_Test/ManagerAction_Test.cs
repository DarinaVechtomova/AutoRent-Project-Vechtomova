using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoRent_Test.Services_Test
{
    [TestClass]
    public class ManagerAction_Test
    {
        [TestMethod]
        public void ConfirmCarRental_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            int orderID = 1;
            DataBase db = new DataBase();
            ManagerAction managerAction = new ManagerAction(db);

            // Act
            bool result = managerAction.ConfirmCarRental(orderID);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConfirmCarRental_OrderFound_AddsAndRemoveOrderToOrderList()
        {
            // Arrange
            int orderID = 1;
            DataBase db = new DataBase();
            string deliveryAddress = "Kotsarska Street 1";
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            ManagerAction managerAction = new ManagerAction(db);
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            WaitingForBookingConfirmation order = new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, false);
            WaitingListRepository waitingListRepository = new WaitingListRepository(db);
            waitingListRepository.Add(order);
            OrderRepository orderRepository = new OrderRepository(db);

            // Act
            bool result = managerAction.ConfirmCarRental(orderID);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(waitingListRepository.FindByID(orderID));
            Assert.IsNotNull(orderRepository.FindByID(orderID));
        }

        [TestMethod]
        public void CancelCarRental_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            int orderID = 1;
            DataBase db = new DataBase();
            ManagerAction managerAction = new ManagerAction(db);

            // Act
            bool result = managerAction.CancelCarRental(orderID);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CancelCarRental_OrderFound_AddsAndRemoveOrderToOrderList()
        {
            // Arrange
            int orderID = 1;
            DataBase db = new DataBase();
            int userID = 1;
            string deliveryAddress = "Kotsarska Street 1";
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            ManagerAction managerAction = new ManagerAction(db);
            TruckRepository truckRepository = new TruckRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            WaitingForBookingConfirmation order = new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, false);
            WaitingListRepository waitingListRepository = new WaitingListRepository(db);
            waitingListRepository.Add(order);

            // Act
            bool result = managerAction.CancelCarRental(orderID);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(waitingListRepository.FindByID(orderID));
        }
    }
}
