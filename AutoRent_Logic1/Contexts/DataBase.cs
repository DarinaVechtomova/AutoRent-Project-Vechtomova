using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using System.Collections.Generic;

namespace AutoRent_Logic.Contexts
{
    public class DataBase
    {        
        public List<Truck> TruckList { get; set; } 
        public List <PassengerCar> PassengerCarList {get; set; }
        public List <Reviews> ReviewsList { get; set; }
        public List <RegisteredUser> RegisteredList { get; set; }
        public List<Orders> OrdersList { get; set; } 
        public List <WaitingForBookingConfirmation> WaitingForConfirmationList { get; set; } 
        public List <Admin> AdminsList { get; set; }
        public DataBase() 
        {
            TruckList = new List<Truck>();
            PassengerCarList = new List<PassengerCar>();
            ReviewsList = new List<Reviews>();
            RegisteredList = new List<RegisteredUser>();
            OrdersList = new List<Orders>();
            WaitingForConfirmationList = new List<WaitingForBookingConfirmation>();
            AdminsList = new List<Admin>();
        }      
    }
}
