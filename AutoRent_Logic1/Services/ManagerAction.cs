using AutoRent_Logic.Contexts;

namespace AutoRent_Logic.Services
{
    public class ManagerAction
    {
        private DataBase _context;
        public ManagerAction(DataBase context)
        {
            _context = context;
        }
        public bool ConfirmCarRental(int orderID)
        {            
            WaitingListRepository waitingListRepository = new WaitingListRepository(_context);
            WaitingForBookingConfirmation carBookingConfirmation = waitingListRepository.FindByID(orderID);
            OrderRepository orderRepository = new OrderRepository(_context);

            if (carBookingConfirmation != null)
            {
                if (carBookingConfirmation.Car != null)
                {
                    Orders order = new Orders(carBookingConfirmation.Car, carBookingConfirmation.RUser, carBookingConfirmation.RentalStartDate, carBookingConfirmation.DateOfEndOfLease, carBookingConfirmation.DeliveryAddress, carBookingConfirmation.OrderId, carBookingConfirmation.IsPaid);
                    order.Status = "Confirmed";
                    orderRepository.Add(order);
                }
                if (carBookingConfirmation.TruckCar != null)
                {
                    Orders order = new Orders(carBookingConfirmation.TruckCar, carBookingConfirmation.RUser, carBookingConfirmation.RentalStartDate, carBookingConfirmation.DateOfEndOfLease, carBookingConfirmation.DeliveryAddress, carBookingConfirmation.OrderId, carBookingConfirmation.IsPaid);
                    order.Status = "Confirmed";
                    orderRepository.Add(order);
                }
                waitingListRepository.Remove(carBookingConfirmation);
                return true;
            }
            return false;
        }

        public bool CancelCarRental(int orderID)
        {
            WaitingListRepository waitingListRepository = new WaitingListRepository(_context);
            WaitingForBookingConfirmation carBookingConfirmation = waitingListRepository.FindByID(orderID);

            if (carBookingConfirmation != null)
            {
                waitingListRepository.Remove(carBookingConfirmation);
                return true;
            }
            return false;
        }
    }
}
