using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI.Popups;

namespace AutoRent_Presentation.Services
{
    public class DataBaseManager
    {
        public static bool isLoaded { get; set; } = false;
        //Зберігання бази даних у json
        public void SaveDataBase(DataBase db) 
        {
            string usersPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "users.json");
            string orderPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "orders.json");
            string passengerCarsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passengerCars.json");
            string reviewsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "reviews.json");
            string truckPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "trucks.json");
            string waitingForConfirmationPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "waitingForConfirmation.json");
            string adminPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "admins.json");
            OrderRepository orderRepository = new OrderRepository(db);
            PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            ReviewsContext reviewsContext = new ReviewsContext();
            TruckRepository truckRepository = new TruckRepository(db);
            WaitingListRepository waitingListRepository = new WaitingListRepository(db);
            AdminsContext adminsContext = new AdminsContext();
            orderRepository.SaveData(db.OrdersList, orderPath);
            passengerCarRepository.SaveData(db.PassengerCarList, passengerCarsPath);
            registeredRepository.SaveData(db.RegisteredList, usersPath);
            reviewsContext.SaveData(db.ReviewsList, reviewsPath);
            truckRepository.SaveData(db.TruckList, truckPath);
            waitingListRepository.SaveData(db.WaitingForConfirmationList, waitingForConfirmationPath);
            adminsContext.SaveData(db.AdminsList, adminPath);
        }

        //Зчитування бази даних з json файлу
        public async void UploadDataBase(DataBase db) 
        {
            try
            {
                string usersPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "users.json");
                string orderPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "orders.json");
                string passengerCarsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passengerCars.json");
                string reviewsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "reviews.json");
                string truckPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "trucks.json");
                string waitingForConfirmationPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "waitingForConfirmation.json");
                string adminPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "admins.json");
                OrderRepository orderRepository = new OrderRepository(db);
                PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
                RegisteredRepository registeredRepository = new RegisteredRepository(db);
                ReviewsContext reviewsContext = new ReviewsContext();
                TruckRepository truckRepository = new TruckRepository(db);
                WaitingListRepository waitingListRepository = new WaitingListRepository(db);
                AdminsContext adminsContext = new AdminsContext();
                List<Orders> orders = orderRepository.ReadFromJson(orderPath);
                List<PassengerCar> passengerCars = passengerCarRepository.ReadFromJson(passengerCarsPath);
                List<RegisteredUser> registeredUsers = registeredRepository.ReadFromJson(usersPath);
                List<Reviews> reviews = reviewsContext.ReadFromJson(reviewsPath);
                List<Truck> trucks = truckRepository.ReadFromJson(truckPath);
                List<WaitingForBookingConfirmation> waitingForConfirmation = waitingListRepository.ReadFromJson(waitingForConfirmationPath);
                List<Admin> admins = adminsContext.ReadFromJson(adminPath);
                // Ініціалізуємо дані в базі даних
                db.TruckList.AddRange(trucks);
                db.PassengerCarList.AddRange(passengerCars);
                db.OrdersList.AddRange(orders);
                db.RegisteredList.AddRange(registeredUsers);
                db.ReviewsList.AddRange(reviews);
                db.WaitingForConfirmationList.AddRange(waitingForConfirmation);
                db.AdminsList.AddRange(admins);
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
    }
}
