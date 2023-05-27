using System;
using AutoRent_Logic.Contexts;
using AutoRent_Logic;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AutoRent_Test.Services_Test
{
    [TestClass]
    public class RegisteredUserAcrion_Test
    {
        [TestMethod]
        public void Authorize_ValidData_ReturnsTrue()
        {
            // Arrange
            string email = "test@gmail.com";
            string fullName = "Stepan Shvets";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            string telephonNumber = "0951234567";
            string password = "password123";
            int id = 1;
            DataBase db = new DataBase();
            RegisteredRepository userRepository = new RegisteredRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            userRepository.Add(new RegisteredUser(email, fullName, dateOfBirth, telephonNumber, password, id));

            // Act            
            bool result = registeredUserAcrion.Authorize(email, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Authorize_WrongEmail_ReturnsFalse()
        {
            // Arrange
            string email = "test@gmail.com";
            string fullName = "Stepan Shvets";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            string telephonNumber = "0951234567";
            string password = "password123";
            int id = 1;
            string email1 = "ttttt@gmail.com";
            DataBase db = new DataBase();
            RegisteredRepository userRepository = new RegisteredRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            userRepository.Add(new RegisteredUser(email, fullName, dateOfBirth, telephonNumber, password, id));

            // Act            
            bool result = registeredUserAcrion.Authorize(email1, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Authorize_WrongPassword_ReturnsFalse()
        {
            // Arrange
            string email = "test@example.com";
            string fullName = "Stepan Shvets";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            string telephonNumber = "0951234567";
            string password = "password123";
            int id = 1;
            string password1 = "p12345678";
            DataBase db = new DataBase();
            RegisteredRepository userRepository = new RegisteredRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            userRepository.Add(new RegisteredUser(email, fullName, dateOfBirth, telephonNumber, password, id));

            // Act            
            bool result = registeredUserAcrion.Authorize(email, password1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MakeCarRental_ValidData_ReturnsTrue()
        {
            // Arrange
            int orderId;
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            int userId = 1;
            int autoID = 1;
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            truckRepository.Add(new Truck(autoID, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));            

            // Act           
            bool result = registeredUserAcrion.MakeCarRental(autoID, rentalStartDate, dateOfEndOfLease, deliveryAddress, userId, out orderId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MakeCarRental_WrongID_ReturnsFalse()
        {
            // Arrange
            int orderId;
            DateTime rentalStartDate = new DateTime(2023, 04, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 04, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            int userId = 100;
            int autoID = 1;
            int autoID1 = 2;
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(user);
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            truckRepository.Add(new Truck(autoID, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));            

            // Act           
            bool result = registeredUserAcrion.MakeCarRental(autoID1, rentalStartDate, dateOfEndOfLease, deliveryAddress, userId, out orderId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MakeCarRental_WrongDate_ReturnsFalse()
        {
            // Arrange
            int orderId;
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            int userId = 100;
            int autoID = 1;
            int orderId1 = 2;
            DateTime rentalStartDate2 = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            OrderRepository orderRepository = new OrderRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            orderRepository.Add(new Orders(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId1, false));           
           
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => registeredUserAcrion.MakeCarRental(autoID, rentalStartDate2, dateOfEndOfLease2, deliveryAddress, userId, out orderId));
        }     

        [TestMethod]
        public void PayForYourOrder_ValidData_ReturnsTrue()
        {
            // Arrange
            int orderID = 111;
            string cardNumber = "1234567891011121";
            string cardExpirationDateStr = "01/25";
            string cvv = "123";
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            bool isPaid = false;
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            waitingRepository.Add(new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, isPaid));

            // Act           
            bool result = registeredUserAcrion.PayForYourOrder(orderID, cardNumber, cardExpirationDateStr, cvv);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PayForYourOrder_WrongCardNumber_ReturnsFalse()
        {
            // Arrange
            int orderID = 111;
            string cardNumber = "1234567";
            string cardExpirationDateStr = "01/25";
            string cvv = "123";
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            bool isPaid = false;
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            waitingRepository.Add(new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, isPaid));
        
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => registeredUserAcrion.PayForYourOrder(orderID, cardNumber, cardExpirationDateStr, cvv));
        }

        [TestMethod]
        public void PayForYourOrder_WrongExpirationDateStr_ReturnsFalse()
        {
            // Arrange
            int orderID = 111;
            string cardNumber = "1234567891011121";
            string cardExpirationDateStr = "01/21";
            string cvv = "123";
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            bool isPaid = false;
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            waitingRepository.Add(new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, isPaid));

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => registeredUserAcrion.PayForYourOrder(orderID, cardNumber, cardExpirationDateStr, cvv));
        }

        [TestMethod]
        public void PayForYourOrder_WrongCVV_ReturnsFalse()
        {
            // Arrange
            int orderID = 111;
            string cardNumber = "1234567891011121";
            string cardExpirationDateStr = "01/25";
            string cvv = "12";
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            int userId = 100;
            bool isPaid = false;
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            waitingRepository.Add(new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, isPaid));

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => registeredUserAcrion.PayForYourOrder(orderID, cardNumber, cardExpirationDateStr, cvv));
        }

        [TestMethod]
        public void PayForYourOrder_WrongIsPaid_ReturnsFalse()
        {
            // Arrange
            int orderID = 111;
            string cardNumber = "1234567891011121";
            string cardExpirationDateStr = "01/20";
            string cvv = "12";
            DateTime rentalStartDate = new DateTime(2023, 07, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 07, 20);
            string deliveryAddress = "Akademika Pavlova Street 1";
            bool isPaid = true;
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            WaitingListRepository waitingRepository = new WaitingListRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            waitingRepository.Add(new WaitingForBookingConfirmation(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderID, isPaid));
           
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => registeredUserAcrion.PayForYourOrder(orderID, cardNumber, cardExpirationDateStr, cvv));
        }

        [TestMethod]
        public void LeaveResponse_AddsReviewToList_ReturnsTrue()
        {
            // Arrange
            DataBase db = new DataBase();
            RegisteredUserAction userAction = new RegisteredUserAction(db);
            string name = "Stepan Shvets";
            string feedback = "This is a great car rental service!";

            // Act
            bool result = userAction.LeaveResponse(name, feedback);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, db.ReviewsList.Count);
            Assert.AreEqual(name, db.ReviewsList[0].Name);
            Assert.AreEqual(feedback, db.ReviewsList[0].Feedback);
        }

        [TestMethod]
        public void LeaveResponse_ExceptionThrown_ReturnsFalse()
        {
            // Arrange
            DataBase db = new DataBase();
            RegisteredUserAction userAction = new RegisteredUserAction(db);
            string name = "Stepan Shvets";
            string feedback = null;

            // Act
            bool result = userAction.LeaveResponse(name, feedback);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, db.ReviewsList.Count);
        }

        [TestMethod]
        public void GetOrderHistory_WithValidUserId_ReturnsListOfOrders()
        {
            // Arrange
            DataBase db = new DataBase();
            int userId = 1;
            string email = "newuser@gmail.com";
            string fullName = "Stepan Shvets";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            string telephoneNumber = "+380951111111";
            string password = "passw0rd1";
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            string deliveryAddress = "Akademika Pavlova Street 1";
            int orderId = 1;
            int orderId2 = 2;
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 05);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);            
            RegisteredUser registeredUser = new RegisteredUser(email, fullName, dateOfBirth, telephoneNumber, password, userId);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(registeredUser);
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            OrderRepository orderRepository = new OrderRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            truckRepository.Add(new Truck(2, "Mercedes-Benz Actros", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 35.5, 150, 1200, 120));
            Orders order1 = new Orders(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId, true);
            Orders order2 = new Orders(truckRepository.FindByID(2), user, rentalStartDate2, dateOfEndOfLease2, deliveryAddress, orderId2, true);
            orderRepository.Add(order1);
            orderRepository.Add(order2);
            
            // Act
            List<Orders> result = registeredUserAcrion.GetOrderHistory(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, order1);
            CollectionAssert.Contains(result, order2);
        }

        [TestMethod]
        public void GetOrderHistory_WithValidUserId_ReturnsListOfOrders_TwoUsers()
        {
            // Arrange
            DataBase db = new DataBase();
            int userId = 1;
            int userId2 = 2;
            string email = "newuser@gmail.com";
            string email2 = "newuser2@gmail.com";
            string fullName = "Stepan Shvets";
            string fullName2 = "Emma Fox";
            DateTime dateOfBirth = new DateTime(1990, 1, 1);
            DateTime dateOfBirth2 = new DateTime(1993, 5, 18);
            string telephoneNumber = "+380951111111";
            string telephoneNumber2 = "+380952222222";
            string password = "passw0rd1";
            string password2 = "passw0rd2";
            DateTime rentalStartDate = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 23);
            string deliveryAddress = "Akademika Pavlova Street 1";
            int orderId = 1;
            int orderId2 = 2;
            DateTime rentalStartDate2 = new DateTime(2023, 07, 01);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 07, 05);
            DateTime birthday = new DateTime(1986, 05, 17);
            DateTime birthday2 = new DateTime(1990, 10, 04);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            RegisteredUser user2 = new RegisteredUser("testUser@gmail.com", "Mykhaylo Holub", birthday2, "+380504411256", "PassvOrd1", 2);
            RegisteredUser registeredUser = new RegisteredUser(email, fullName, dateOfBirth, telephoneNumber, password, userId);
            RegisteredUser registeredUser2 = new RegisteredUser(email2, fullName2, dateOfBirth2, telephoneNumber2, password2, userId2);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            registeredRepository.Add(registeredUser);
            registeredRepository.Add(registeredUser2);
            TruckRepository truckRepository = new TruckRepository(db);
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);
            OrderRepository orderRepository = new OrderRepository(db);
            truckRepository.Add(new Truck(1, "Volvo FH", "Diesel", TransmissionType.Manual, FuelType.Diesel, 35.7, 120, 1000, 90));
            truckRepository.Add(new Truck(2, "Mercedes-Benz Actros", "Diesel", TransmissionType.Automatic, FuelType.Diesel, 35.5, 150, 1200, 120));
            Orders order1 = new Orders(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId, true);
            Orders order2 = new Orders(truckRepository.FindByID(2), user2, rentalStartDate2, dateOfEndOfLease2, deliveryAddress, orderId2, true);
            orderRepository.Add(order1);
            orderRepository.Add(order2);

            // Act
            List<Orders> result = registeredUserAcrion.GetOrderHistory(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            CollectionAssert.Contains(result, order1);
            CollectionAssert.DoesNotContain(result, order2);
        }

        [TestMethod]
        public void GetOrderHistory_WithNonExistentUserId_ReturnsEmptyList()
        {
            // Arrange
            int userId = 1;
            DataBase db = new DataBase();
            RegisteredUserAction registeredUserAcrion = new RegisteredUserAction(db);

            // Act
            List<Orders> result = registeredUserAcrion.GetOrderHistory(userId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
