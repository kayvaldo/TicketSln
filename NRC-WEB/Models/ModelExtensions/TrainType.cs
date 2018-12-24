using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRC_WEB.Models 
{
    public partial class TrainType
    {
        public string GetTravelClassSeatCount(TravelClass travelClass)
        {
            string seatCount = "0";
            try
            {
                var match = TrainTypeTravelClasses.Where(x => x.TravelClassId == travelClass.Id).Single();
                seatCount = match == null ? "0" : match.NumberOfSeats.ToString();
                return seatCount;
            }
            catch (Exception)
            {
                return "None";
            }
        }
    }
}