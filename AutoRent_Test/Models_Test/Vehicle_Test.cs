using AutoRent_Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoRent_Test.Models_Test
{
    [TestClass]
    public class Vehicle_Test
    {
        [TestMethod]
        public void CalculationOfTheCostOfRenting_ValidData()
        {
            // Arrange
            int rentalPrice = 120;
            int pledge = 1000;
            DateTime rentalStartDate = new DateTime(2023, 04, 17);
            DateTime dateOfEndOfLease = new DateTime(2023, 04, 23);

            // Act        
            int result = Vehicle.CalculationOfTheCostOfRenting(rentalPrice, pledge, rentalStartDate, dateOfEndOfLease);

            // Assert
            Assert.AreEqual(1840, result);
        }
    }
}
