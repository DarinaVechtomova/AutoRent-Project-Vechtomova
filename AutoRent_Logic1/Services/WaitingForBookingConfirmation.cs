using AutoRent_Logic.Models;
using System;

namespace AutoRent_Logic.Services
{
    public class WaitingForBookingConfirmation
    {
        private DateTime rentalStartDate;
        private DateTime dateOfEndOfLease;
        private string deliveryAddress;
        private int orderId;
        public PassengerCar Car { get; set; }
        public Truck TruckCar { get; set; }
        public bool IsPaid { get; set; } = false;
        public RegisteredUser RUser { get; set; }
        public string Status { get; set; }
        public DateTime RentalStartDate
        {
            get { return rentalStartDate; }

            set
            {
                if (value < DateTime.Now)
                {
                    throw new ArgumentException("Rental start date cannot be in the past.");
                }
                rentalStartDate = value;
            }
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
                else if (value < DateTime.Now)
                {
                    throw new ArgumentException("Rental end date cannot be in the past.");
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
        public string RentalStartDateWithoutTime
        {
            get { return rentalStartDate.ToString("dd/MM/yyyy"); }            
        }
        public string DateOfEndOfLeaseWithoutTime
        {
            get { return dateOfEndOfLease.ToString("dd/MM/yyyy"); }            
        }
        public WaitingForBookingConfirmation() 
        { }       
        public WaitingForBookingConfirmation(PassengerCar car, RegisteredUser user, DateTime rentalStartDate, DateTime dateOfEndOfLease, string deliveryAddress, int orderId, bool isPaid)
        {
            Car = car;
            RUser = user;
            RentalStartDate = rentalStartDate;
            DateOfEndOfLease = dateOfEndOfLease;
            DeliveryAddress = deliveryAddress;
            OrderId = orderId;
            IsPaid = isPaid;
        }
        public WaitingForBookingConfirmation(Truck car, RegisteredUser user, DateTime rentalStartDate, DateTime dateOfEndOfLease, string deliveryAddress, int orderId, bool isPaid)
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