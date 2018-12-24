using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRC_WEB.Models
{
    public partial class Schedule
    {

        public string GetFromStationName()
        {
            try
            {
                var fromStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.From)).Single();
                return fromStation.Station.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetToStationName()
        {
            try
            {
                var toStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.To)).Single();
                return toStation.Station.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetPriceAndSeats(TravelClass travelClass, DateTime date)
        {
            var info = "NA";

            try
            {
                var scheduleTravelClass = ScheduleTravelClasses.Where(x => x.TravelClassId == travelClass.Id).SingleOrDefault();
                if (scheduleTravelClass != null)
                {
                    info = string.Format("NGN {0}({1} seat(s) left)", scheduleTravelClass.Price, scheduleTravelClass.GetRemainingSeats(date));
                }
                
            }
            catch (Exception ex)
            {

            }

            return info;
        }

        public ScheduleTravelClass GetScheduleTravelClass(int travelClassId)
        {
            var scheduleTravelClass = ScheduleTravelClasses.Where(x => x.TravelClassId == travelClassId).SingleOrDefault();
            return scheduleTravelClass;
        }

        public int GetScheduleTravelClassId(int travelClassId)
        {
            var scheduleTravelClass = ScheduleTravelClasses.Where(x => x.TravelClassId == travelClassId).SingleOrDefault();
            int id = scheduleTravelClass == null ? 0 : scheduleTravelClass.Id;
            return id;
        }

        public string GetTimeOfDay()
        {
            var noon = new TimeSpan(12, 0, 0);
            string period = TimeOfDay < noon ? "AM" : "PM";
            var time = string.Format("{0}:{1} {2}", TimeOfDay.Value.Hours.ToString().PadLeft(2,'0'), TimeOfDay.Value.Minutes.ToString().PadLeft(2, '0'), period);
            return time;
        }

        public string FullDisplayNameWithStation
        {
            get
            {
                var fromStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.From)).Single().Station;
                var toStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.To)).Single().Station;
                var fromCity = fromStation.City;
                var toCity = toStation.City;

                var fullName = string.Format("{0} ({1}) - {2} ({3})", fromCity.Name, fromStation.Name, toCity.Name, toStation.Name);
                return fullName;
            }
       

        }

        public string FullDisplayName
        {
            get
            {
                var fromStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.From)).Single().Station;
                var toStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.To)).Single().Station;
                var fromCity = fromStation.City;
                var toCity = toStation.City;

                var fullName = string.Format("{0} - {1}", fromCity.Name, toCity.Name);
                return fullName;
            }
        }


        //public String FromStation
        //{
        //    get
        //    {
        //        var o = ScheduleStations;
        //        var fromStationx = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.From)).ToList();
        //        var fromStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.From)).Single();
        //        return fromStation.Station.Name;
        //    }
        //}

        //public String ToStation
        //{
        //    get
        //    {
        //        var fromStationx = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.To)).ToList();
        //        var toStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.To)).Single();
        //        return toStation.Station.Name;
        //    }
        //}

        //public int FromStationId
        //{
        //    get
        //    {
        //        var fromStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.From)).Single();
        //        return fromStation.Station.Id;
        //    }
        //}

        //public int ToStationId
        //{
        //    get
        //    {
        //        var toStation = ScheduleStations.Where(x => x.Location == Convert.ToInt32(Location.To)).Single();
        //        return toStation.Station.Id;
        //    }
        //}
    }
}