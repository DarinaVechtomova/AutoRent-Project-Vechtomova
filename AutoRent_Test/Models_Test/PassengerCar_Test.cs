using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoRent_Test.Models_Test
{
    [TestClass]
    public class PassengerCar_Test
    {

        [TestMethod]
        public void IsAvailable_ValidData()
        {
            // Arrange
            int orderId;
            int id = 1;
            string vehicleName = "Toyota Camry";
            string engine = "2.5L 4-cylinder engine";
            TransmissionType transmissionType = TransmissionType.Automatic;
            FuelType fuelType = FuelType.Petrol;
            double fuelConsumption = 7.8;
            int rentalPrice = 70;
            int pledge = 1000;
            int numberOfPassengerSeats = 5;
            DateTime rentalStartDate = new DateTime(2023, 04, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 04, 20);
            int autoID = 1;

            // Act
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, numberOfPassengerSeats);
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            bool result = passengerCar.IsAvailable(autoID, rentalStartDate, dateOfEndOfLease, db);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAvailable_NullPassengerCar()
        {
            // Arrange
            int orderId;
            DateTime rentalStartDate = new DateTime(2023, 04, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 04, 20);
            int autoID = 1;
            int autoID1 = 2;
            string vehicleName = "Toyota Camry";
            string engine = "2.5L 4-cylinder engine";
            TransmissionType transmissionType = TransmissionType.Automatic;
            FuelType fuelType = FuelType.Petrol;
            double fuelConsumption = 7.8;
            int rentalPrice = 70;
            int pledge = 1000;
            int numberOfPassengerSeats = 5;

            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(autoID, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, numberOfPassengerSeats);
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            
            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => passengerCar.IsAvailable(autoID1, rentalStartDate, dateOfEndOfLease, db));
        }

        [TestMethod]
        public void IsAvailable_WrongDate()
        {
            // Arrange
            int orderId;
            DateTime rentalStartDate = new DateTime(2023, 06, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 06, 20);
            string deliveryAddress = "123 Main St";
            int autoID = 1;
            int orderId1 = 2;
            int id = 1;
            string vehicleName = "Toyota Camry";
            string engine = "2.5L 4-cylinder engine";
            TransmissionType transmissionType = TransmissionType.Automatic;
            FuelType fuelType = FuelType.Petrol;
            double fuelConsumption = 7.8;
            int rentalPrice = 70;
            int pledge = 1000;
            int numberOfPassengerSeats = 5;
            DateTime rentalStartDate2 = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
 
            DataBase db = new DataBase();
            PassengerCar passengerCar = new PassengerCar(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, numberOfPassengerSeats);
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            passengerCarRepository.Add(passengerCar);
            OrderRepository orderRepository = new OrderRepository(db);
            orderRepository.Add(new Orders(passengerCarRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId1, false));

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => passengerCar.IsAvailable(autoID, rentalStartDate2, dateOfEndOfLease2, db));
        }
    }
}
