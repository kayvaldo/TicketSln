using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRC_WEB.Models
{
    public partial class ScheduleTravelClass
    {
        public int GetNumberOfRemainingSeats(DateTime date)
        {
            //var bookingsForDate = Bookings.Where(x => x.Date.Value == date).ToList();
            //int bookingsCount = bookingsForDate.Sum(x => x.NumberOfTickets.Value);
            //int seatsLeft = NumberOfSeats - bookingsCount;
            //return seatsLeft;

            int bookingsCount = Tickets.Where(x => x.Booking.Date.Value == date).Count();
            int seatsLeft = NumberOfSeats - bookingsCount;
            return seatsLeft;
        }

       public string GetRemainingSeats(DateTime date)
        {
            string result = string.Empty;
            int seatsLeft = GetNumberOfRemainingSeats(date);
            result = seatsLeft > 0 ? seatsLeft.ToString() : "Fully Booked";
            return result;
           
        }
    }
}