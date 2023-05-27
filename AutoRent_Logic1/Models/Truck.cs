using AutoRent_Logic.Contexts;
using System;

namespace AutoRent_Logic.Models
{
    public class Truck : Vehicle
    {
        private double cargoItemVolume;
        private string img;

        public Truck(int id, string vehicleName, string engine, TransmissionType transmissionType, FuelType fuelType, double fuelConsumption, int rentalPrice, int pledge, double cargoItemVolume) 
            : base(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice)
        {
            CargoItemVolume = cargoItemVolume;
            Pledge = pledge;
        }
        public Truck(int id, string vehicleName, string engine, TransmissionType transmissionType, FuelType fuelType, double fuelConsumption, int rentalPrice, int pledge, double cargoItemVolume, string img)
            : base(id, vehicleName, engine, transmissionType, fuelType, fuelConsumption, rentalPrice)
        {
            CargoItemVolume = cargoItemVolume;
            Pledge = pledge;
            Img = img;
        }
        public Truck() { }
        public static int Pledge { get; set; } = 1000;
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
        public double CargoItemVolume 
        {
            get { return cargoItemVolume; }

            set
            {
                if (value >= 1)
                    cargoItemVolume = value;
                else
                    throw new ArgumentException("Cargo item volume cannot be less than 1");
            }
        }

        public override bool IsAvailable(int id, DateTime rentalStartDate, DateTime dateOfEndOfLease, DataBase _context)
        {
            TruckRepository truckRepository = new TruckRepository(_context);
            Truck truck = truckRepository.FindByID(id);
            if (truck == null)
            {
                throw new ArgumentException("Truck with the provided id does not exist.");
            }
            bool isAvailable = true;
            OrderRepository orderRepository = new OrderRepository(_context);
            WaitingListRepository waitingListRepository = new WaitingListRepository(_context);
            var bookingsOrder = orderRepository.GetAll();
            foreach (var booking in bookingsOrder)
            {
                if (booking.TruckCar != null)
                {
                    if (booking.TruckCar.Id == truck.Id)
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
                if (booking.TruckCar != null)
                {
                    if (booking.TruckCar.Id == truck.Id)
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
