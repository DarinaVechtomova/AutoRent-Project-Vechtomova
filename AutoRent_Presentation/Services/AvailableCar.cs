using AutoRent_Logic.Contexts;
using System;

namespace AutoRent_Presentation.Services
{
    public class AvailableCar
    {
        public bool isAvailableCar(int id, DataBase db)
        {            
            bool isAvailable = true;
            OrderRepository orderRepository = new OrderRepository(db);
            WaitingListRepository waitingListRepository = new WaitingListRepository(db);
            var bookingsOrder = orderRepository.GetAll();
            foreach (var booking in bookingsOrder)
            {
                if (booking.TruckCar != null && booking.TruckCar.Id == id)
                {
                    if (booking.DateOfEndOfLease >= DateTime.Today)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                else if(booking.Car != null && booking.Car.Id == id)
                {
                    if (booking.DateOfEndOfLease >= DateTime.Today)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            var bookingsWaiting = waitingListRepository.GetAll();
            foreach (var booking in bookingsWaiting)
            {
                if (booking.TruckCar != null && booking.TruckCar.Id == id)
                {
                    if (booking.DateOfEndOfLease >= DateTime.Today)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                else if (booking.Car != null && booking.Car.Id == id)
                {
                    if (booking.DateOfEndOfLease >= DateTime.Today)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            return isAvailable;
        }
    }
}
