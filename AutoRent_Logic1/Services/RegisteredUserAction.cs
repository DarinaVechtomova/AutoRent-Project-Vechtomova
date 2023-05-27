using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Presentation.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Popups;

namespace AutoRent_Logic.Services
{
    public class RegisteredUserAction
    {
        public delegate void SuccessOrderHandler(object sender, string message);
        public delegate void SuccessPayHandler(object sender, string message);
        public event SuccessOrderHandler OrderNotify;
        public event SuccessPayHandler SuccessPayNotify;

        private DataBase _context;
        public RegisteredUserAction(DataBase context)
        {
            _context = context;
        }

        public bool Authorize(string email, string password)
        {
            List<RegisteredUser> RegistredList = _context.RegisteredList.ToList();
            bool isRegisteregUser = RegistredList.Any(u => u.Email == email && u.Password == password);           
            if (isRegisteregUser)
            {
                User user = RegistredList.FirstOrDefault(u => u.Email == email && u.Password == password);                
                UserAuthentication.ID = user.Id;
                UserAuthentication.IsAuthentication = true;
                
            }
            else if (_context.AdminsList.Any(u => u.Email == email && u.Password == password))
            {
                AdminAuthentication.IsAuthenticated = true;
            }
            return isRegisteregUser;
        }

        public bool MakeCarRental(int autoID, DateTime rentalStartDate, DateTime dateOfEndOfLease, string deliveryAddress, int userId, out int orderId)
        {
            PassengerCarRepository passegerCarRepository = new PassengerCarRepository(_context);
            TruckRepository truckRepository = new TruckRepository(_context);
            PassengerCar passengerCar = passegerCarRepository.FindByID(autoID);
            Truck truck = truckRepository.FindByID(autoID);
            RegisteredRepository registeredRepository = new RegisteredRepository(_context);
            RegisteredUser user = new RegisteredUser();
            user = registeredRepository.FindByID(userId);
            int maxOrderIdW = 0, maxOrderIdO = 0;
            if (_context.WaitingForConfirmationList.Count != 0)
            {
                maxOrderIdW = _context.WaitingForConfirmationList.Max(w => w.OrderId);
            }
            if (_context.OrdersList.Count != 0)
            {
                maxOrderIdO = _context.OrdersList.Max(w => w.OrderId);
            }
            int maxOrderId = 0;
            if (maxOrderIdW > maxOrderIdO)
            {
                maxOrderId = maxOrderIdW;
            }
            else if (maxOrderIdO > maxOrderIdW)
            {
                maxOrderId = maxOrderIdO;
            }
            orderId = maxOrderId + 1;
            Vehicle vehicle = new PassengerCar();
            if (passengerCar != null && vehicle.IsAvailable(autoID, rentalStartDate, dateOfEndOfLease, _context))
            {               
                WaitingListRepository waitingRepository = new WaitingListRepository(_context);
                WaitingForBookingConfirmation waitingForBookingConfirmation = new WaitingForBookingConfirmation(passengerCar, user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId, false);
                waitingForBookingConfirmation.Status = "Created";
                waitingRepository.Add(waitingForBookingConfirmation);
                OrderNotify?.Invoke(this, $"Order {orderId} has been successfully created for {passengerCar.VehicleName}.");
                return true;
            }
            vehicle = new Truck();
            if (truck != null && vehicle.IsAvailable(autoID, rentalStartDate, dateOfEndOfLease, _context))
            {
                WaitingListRepository waitingRepository = new WaitingListRepository(_context);
                WaitingForBookingConfirmation waitingForBookingConfirmation = new WaitingForBookingConfirmation(truck, user, rentalStartDate, dateOfEndOfLease, deliveryAddress, orderId, false);
                waitingForBookingConfirmation.Status = "Created";
                waitingRepository.Add(waitingForBookingConfirmation);
                OrderNotify?.Invoke(this, $"Order {orderId} has been successfully created for {truck.VehicleName}.");
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PayForYourOrder(int orderID, string cardNumber, string cardExpirationDateStr, string cvv)
        {
            bool isTrueCVV = Regex.IsMatch(cvv, @"^\d{3}$");
            bool isTrueCardNum = Regex.IsMatch(cardNumber, @"^\d{16}$");
            WaitingListRepository waitingRepository = new WaitingListRepository(_context);
            WaitingForBookingConfirmation waitingCar = waitingRepository.FindByID(orderID);
            if (!isTrueCVV)
            {
                throw new ArgumentException("Invalid input format for the CVV. Please enter 3 char");
            }
            if (!isTrueCardNum)
            {
                throw new ArgumentException("Invalid input format for the card number. Please enter 16 char");
            }
            DateTime cardExpirationDate;
            if (!DateTime.TryParseExact(cardExpirationDateStr, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out cardExpirationDate))
            {
                throw new ArgumentException("Invalid input format for the card expiration date. Please enter the month and year separated by '/'");
            }
            if (DateTime.Compare(cardExpirationDate, DateTime.Now) < 0)
            {
                throw new ArgumentException("Your card has expired. Please try another card or choose the payment on delivery option");
            }           
            if (waitingCar != null && isTrueCardNum && isTrueCVV && waitingCar.IsPaid == false)
            {
                waitingCar.IsPaid = true;
                waitingRepository.UpData(waitingCar);
                SuccessPayNotify?.Invoke(this, $"Order {orderID} has been successfully paid.");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LeaveResponse(string name, string feedback)
        {
            try
            {
                var reviews = new Reviews(name, feedback)
                {
                     Name = name,
                     Feedback = feedback
                };
                _context.ReviewsList.Add(reviews);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Orders> GetOrderHistory(int userId)
        {
            List<Orders> orderHistory = new List<Orders>();

            WaitingListRepository waitingRepository = new WaitingListRepository(_context);
            OrderRepository orderRepository = new OrderRepository(_context);
            RegisteredRepository userRepository = new RegisteredRepository(_context);

            User user = userRepository.FindByID(userId);
            List<WaitingForBookingConfirmation> ordersWaiting = waitingRepository.GetAll().Where(o => o.RUser.Id == user.Id).ToList();
            List<Orders> orders = orderRepository.GetAll().Where(o => o.RUser.Id == user.Id).ToList();
            if (ordersWaiting.Any())
            {
                foreach (var waitingOrder in ordersWaiting)
                {
                    Orders order = new Orders()
                    {
                        Car = waitingOrder.Car,
                        TruckCar = waitingOrder.TruckCar,
                        IsPaid = waitingOrder.IsPaid,
                        RUser = waitingOrder.RUser,
                        RentalStartDate = waitingOrder.RentalStartDate,
                        DateOfEndOfLease = waitingOrder.DateOfEndOfLease,
                        DeliveryAddress = waitingOrder.DeliveryAddress,
                        OrderId = waitingOrder.OrderId,
                        Status = waitingOrder.Status,
                    };

                    orderHistory.Add(order);
                }
            }
            if (orders.Any())
            {
                orderHistory.AddRange(orders);
            }           
            return orderHistory;
        }

        public async void DisplayMessage(object sender, string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}