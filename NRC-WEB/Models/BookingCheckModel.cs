using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRC_WEB.Models
{
    public class BookingCheckModel
    {
        public int ScheduleTravelClassId { get; set; }
        public int NumberOfTickets { get; set; }
        public bool Checked { get; set; }
    }
}