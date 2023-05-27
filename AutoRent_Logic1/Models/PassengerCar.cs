using AutoRent_Logic.Contexts;
using System;

namespace AutoRent_Logic.Models
{
    public class PassengerCar : Vehicle
    {
        private int numberOfPassengerSeats;
        private string img;
        public PassengerCar(int id, string vehicleName, string engine, TransmissionType transmissionType, 
            FuelType fuelType, double fuelConsumption, int rentalPrice, int pledge, int numberOfPassengerSeats) 
            : base(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice)
        {
            NumberOfPassengerSeats = numberOfPassengerSeats;
            Pledge = pledge;
        }
        public PassengerCar(int id, string vehicleName, string engine, TransmissionType transmissionType, 
            FuelType fuelType, double fuelConsumption, int rentalPrice, int pledge, int numberOfPassengerSeats, string img)
            : base(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice)
        {
            NumberOfPassengerSeats = numberOfPassengerSeats;
            Pledge = pledge;
            Img = img;
        }

        public PassengerCar() { }
        public static int Pledge { get; set; } = 500;
        public string Img
        {
            get { return img; }

            set
            {
                if (!string.IsNullOrEmpty(value))
                    img = value;
                else
                    throw new ArgumentException("You need to add an image");
            }
        }
        public int NumberOfPassengerSeats
        {
            get { return numberOfPassengerSeats; }

            set
            {
                if (value >= 1)
                    numberOfPassengerSeats = value;
                else
                    throw new ArgumentException("Number of passenger seats cannot be less than 1");
            }
        }

        public override bool IsAvailable(int id, DateTime rentalStartDate, DateTime dateOfEndOfLease, DataBase _context)
        {
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(_context);
            PassengerCar passengerCar = passengerCarRepository.FindByID(id);
            if (passengerCar == null)
            {
                throw new ArgumentException("Passenger car with the provided id does not exist.");
            }
            bool isAvailable = true;
            OrderRepository orderRepository = new OrderRepository(_context);
            WaitingListRepository waitingListRepository = new WaitingListRepository(_context);
            var bookingsOrder = orderRepository.GetAll();
            foreach (var booking in bookingsOrder)
            {
                if (booking.Car != null)
                {
                    if (booking.Car.Id == passengerCar.Id)
                    {
                        if (!(rentalStartDate >= booking.DateOfEndOfLease || dateOfEndOfLease <= booking.RentalStartDate))
                        {
                            isAvailable = false;
                            throw new ArgumentException("The car is rented for these dates. Choose another date");
                        }
                    }
                }
            }
            var bookingsWaiting = waitingListRepository.GetAll();
            foreach (var booking in bookingsWaiting)
            {
                if (booking.Car != null)
                {
                    if (booking.Car.Id == passengerCar.Id)
                    {
                        if (!(rentalStartDate >= booking.DateOfEndOfLease || dateOfEndOfLease <= booking.RentalStartDate))
                        {
                            isAvailable = false;
                            throw new ArgumentException("The car is rented for these dates. Choose another date");
                        }
                    }
                }
            }
            return isAvailable;
        }
    }
}
