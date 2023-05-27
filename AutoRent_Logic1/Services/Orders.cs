using AutoRent_Logic.Models;
using System;

namespace AutoRent_Logic.Services
{
    public class Orders
    {
        private DateTime rentalStartDate;
        private DateTime dateOfEndOfLease;
        private string deliveryAddress;
        private int orderId;
        private string status;

        public PassengerCar Car { get; set; }
        public Truck TruckCar { get; set; }
        public bool IsPaid { get; set; } = false;
        public RegisteredUser RUser { get; set; }
        
        public DateTime RentalStartDate
        {
            get { return rentalStartDate; }

            set { rentalStartDate = value; }
        }
        public DateTime DateOfEndOfLease
        {
            get { return dateOfEndOfLease; }

            set
            {
                if (value <= RentalStartDate)
                {
                    throw new ArgumentException("Rental end date cannot be greater or equal to the end of lease date.");
                }              
                dateOfEndOfLease = value;
            }
        }
        public string DeliveryAddress
        {
            get { return deliveryAddress; }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Delivery address cannot be null or empty");
                }
                deliveryAddress = value;
            }
        }
        public int OrderId
        {
            get { return orderId; }

            set { orderId = value; }
        }

        public string VehicleName
        {
            get
            {
                if (Car != null)
                {
                    return Car.VehicleName;
                }
                else if (TruckCar != null)
                {
                    return TruckCar.VehicleName;
                }
                else
                {
                    return null;
                }
            }
        }
        public int VehicleId
        {
            get
            {
                if (Car != null)
                {
                    return Car.Id;
                }
                else if (TruckCar != null)
                {
                    return TruckCar.Id;
                }
                else
                {
                    return -1;
                }
            }
        }
        public string Status
        {
            get { return status; }

            set
            {
                if (RentalStartDate.Date <= DateTime.Now.Date)
                {
                    status = "Done";
                }
                else
                {
                    status = value;
                }
            }
        }

        public string RentalStartDateWithoutTime
        {
            get { return rentalStartDate.ToString("dd/MM/yyyy"); }
        }
        public string DateOfEndOfLeaseWithoutTime
        {
            get { return dateOfEndOfLease.ToString("dd/MM/yyyy"); }
        }
        public Orders() { }       
        public Orders(PassengerCar car, RegisteredUser user, DateTime rentalStartDate, DateTime dateOfEndOfLease, string deliveryAddress, int orderId, bool isPaid)
        {
            Car = car;
            RUser = user;
            RentalStartDate = rentalStartDate;
            DateOfEndOfLease = dateOfEndOfLease;
            DeliveryAddress = deliveryAddress;
            OrderId = orderId;
            IsPaid = isPaid;
        }
        public Orders(Truck car, RegisteredUser user, DateTime rentalStartDate, DateTime dateOfEndOfLease, string deliveryAddress, int orderId, bool isPaid)
        {
            TruckCar = car;
            RUser = user;
            RentalStartDate = rentalStartDate;
            DateOfEndOfLease = dateOfEndOfLease;
            DeliveryAddress = deliveryAddress;
            OrderId = orderId;
            IsPaid = isPaid;
        }
    }
}
