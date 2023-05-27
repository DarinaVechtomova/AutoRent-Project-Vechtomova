using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoRent_Logic.Models;
using AutoRent_Logic;

namespace AutoRent_Test.Models_Test
{
    [TestClass]
    public class Truck_Test
    {        
        [TestMethod]
        public void IsAvailable_ValidData()
        {
            // Arrange
            int orderId;
            int id = 1;
            string vehicleName = "Volvo FH";
            string engine = "Diesel";
            TransmissionType transmissionType = TransmissionType.Manual;
            FuelType fuelType = FuelType.Diesel;
            double fuelConsumption = 35.7;
            int rentalPrice = 120;
            int pledge = 1000;
            int cargoItemVolume = 90;
            DateTime rentalStartDate = new DateTime(2023, 04, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 04, 20);
            int autoID = 1;
            // Act
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            Truck truck = new Truck(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, cargoItemVolume);
            truckRepository.Add(truck);

            bool result = truck.IsAvailable(autoID, rentalStartDate, dateOfEndOfLease, db);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAvailable_NullTruck()
        {
            // Arrange
            int orderId;
            string vehicleName = "Volvo FH";
            string engine = "Diesel";
            TransmissionType transmissionType = TransmissionType.Manual;
            FuelType fuelType = FuelType.Diesel;
            double fuelConsumption = 35.7;
            int rentalPrice = 120;
            int pledge = 1000;
            int cargoItemVolume = 90;
            DateTime rentalStartDate = new DateTime(2023, 04, 15);
            DateTime dateOfEndOfLease = new DateTime(2023, 04, 20);
            int autoID = 1;
            int autoID1 = 2;
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            Truck truck = new Truck(autoID, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, cargoItemVolume);
            truckRepository.Add(truck);

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => truck.IsAvailable(autoID1, rentalStartDate, dateOfEndOfLease, db));
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
            string vehicleName = "Volvo FH";
            string engine = "Diesel";
            TransmissionType transmissionType = TransmissionType.Manual;
            FuelType fuelType = FuelType.Diesel;
            double fuelConsumption = 35.7;
            int rentalPrice = 120;
            int pledge = 1000;
            int cargoItemVolume = 90;
            DateTime rentalStartDate2 = new DateTime(2023, 06, 17);
            DateTime dateOfEndOfLease2 = new DateTime(2023, 06, 23);
            DateTime birthday = new DateTime(1986, 05, 17);
            RegisteredUser user = new RegisteredUser("test@gmail.com", "Stepan Shvets", birthday, "+380502233445", "passvOrd7", 1);
            DataBase db = new DataBase();
            TruckRepository truckRepository = new TruckRepository(db);
            Truck truck = new Truck(autoID, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, cargoItemVolume);
            truckRepository.Add(truck);
            OrderRepository orderRepository = new OrderRepository(db);          
            orderRepository.Add(new Orders(truckRepository.FindByID(1), user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId1, false));

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => truck.IsAvailable(autoID, rentalStartDate2, dateOfEndOfLease2, db));
        }
    }
}
