using AutoRent_Logic.Contexts;
using System;

namespace AutoRent_Logic.Models
{
    public abstract class Vehicle
    {
        private string vehicleName;
        private string engine;
        private TransmissionType transmissionType;
        private FuelType fuelType;
        private double fuelConsumption;
        private int rentalPrice;
        public Vehicle() { }
        
        public Vehicle(int id, string vehicleName, string engine, TransmissionType transmissionType, FuelType fuelType, double fuelConsumption, int rentalPrice)
        {
            Id = id;
            VehicleName = vehicleName;
            Engine = engine;
            TransmissionType = transmissionType;
            FuelType = fuelType;
            FuelConsumption = fuelConsumption;
            RentalPrice = rentalPrice;
        }
        
        public int Id { get; set; }
        public string VehicleName 
        {
            get
            {
                return vehicleName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Vehicle name cannot be null or empty");
                }
                vehicleName = value;
            }
        }
        public string Engine 
        {
            get
            {
                return engine;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Engine cannot be null or empty");
                }
                engine = value;
            }
        }
        public TransmissionType TransmissionType 
        {
            get { return transmissionType; }

            set
            {
                if (Convert.ToInt32(value) < 0 || Convert.ToInt32(value) > 2)
                {
                    throw new ArgumentException("Incorrect transmission type value");
                }
                transmissionType = value;
            }
        }
        public FuelType FuelType 
        {
            get { return fuelType; }

            set
            {
                if (Convert.ToInt32(value) < 0 || Convert.ToInt32(value) > 4)
                {
                    throw new ArgumentException("Incorrect fuel type value");
                }
                fuelType = value;
            }
        }
        public double FuelConsumption 
        {
            get { return fuelConsumption; }

            set
            {
                if (value >= 1)
                    fuelConsumption = value;
                else
                    throw new ArgumentException("Fuel consumption cannot be less than 1");
            }
        }

        public int RentalPrice
        {
            get { return rentalPrice; }

            set
            {
                if (value >= 1)
                    rentalPrice = value;
                else
                    throw new ArgumentException("Rental price cannot be less than 1");
            }
        }
        public static int CalculationOfTheCostOfRenting(int rentalPrice, int pledge, 
            DateTime leaseStartDate, DateTime theEndDateOfTheLease)
        {
            TimeSpan rentalPeriod = theEndDateOfTheLease - leaseStartDate;
            int numberOfDays = rentalPeriod.Days + 1;
            int totalRentalCost = rentalPrice * numberOfDays + pledge;
            return totalRentalCost;
        }

        public abstract bool IsAvailable(int id, DateTime rentalStartDate, DateTime dateOfEndOfLease, DataBase _context);       
    }
}