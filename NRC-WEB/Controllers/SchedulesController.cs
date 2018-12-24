using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NRC_WEB.Models;
using NRC_WEB.Utility;

namespace NRC_WEB.Controllers
{
    public class SchedulesController : Controller
    {
        private NrcDbContext db = new NrcDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.TrainType).AsEnumerable();
            return View(schedules);
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule); 
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.TrainTypeId = new SelectList(db.TrainTypes.Where(x => x.IsActive), "Id", "Name");
            var daysOfWeek = GetSelectListModel(db.DayOfTheWeeks.ToList());
            ViewBag.DaysOfWeek = daysOfWeek;
            var stations = new SelectList(db.Stations.Where(x => x.IsActive && x.City.IsActive).ToList(), "Id", "Name");
            ViewBag.Stations = stations;
            return View();
        }

      
        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DurationInMinutes,TrainTypeId,TimeOfDay,IsActive")] Schedule schedule,
            List<ScheduleTravelClass> scheduleTravelClasses, int[] daysOfWeek, int[] stations)
        {
            if (ModelState.IsValid)
            {
                //Check seat count
                TrainType trainType = db.TrainTypes.Find(schedule.TrainTypeId);
                int scheduleSeatCount = scheduleTravelClasses.Sum(x => x.NumberOfSeats);

                foreach (var scheduleTravelClass in scheduleTravelClasses)
                {
                    TrainTypeTravelClass trainTypeTravelClass =
                        trainType.TrainTypeTravelClasses.Where(x => x.TravelClassId == scheduleTravelClass.TravelClassId).Single();
                    if (scheduleTravelClass.NumberOfSeats > trainTypeTravelClass.NumberOfSeats)
                    {
                        ModelState.AddModelError("", "The number of seats specified exceeds the specified maximum capacity");
                        ViewBag.TrainTypeId = new SelectList(db.TrainTypes, "Id", "Name");
                        var daysOfWeekList = GetSelectListModel(db.DayOfTheWeeks.ToList());
                        ViewBag.DaysOfWeek = daysOfWeekList;
                        var stationsList = new SelectList(db.Stations.ToList(), "Id", "Name");
                        ViewBag.Stations = stationsList;
                        return View(schedule);
                    }
                }
                

                db.Schedules.Add(schedule);
                db.SaveChanges();

                //Create ScheduleTravelClasses
                if (scheduleTravelClasses != null && scheduleTravelClasses.Any())
                {
                    foreach (var item in scheduleTravelClasses)
                    {
                        item.ScheduleId = schedule.Id;
                    }

                    db.ScheduleTravelClasses.AddRange(scheduleTravelClasses);
                }

                //Create ScheduleDaysOfTheWeek
                if (daysOfWeek.Any())
                {
                   List<ScheduleDayOfTheWeek> scheduleDayOfTheWeek = new List<ScheduleDayOfTheWeek>();
                    foreach (var item in daysOfWeek)
                    {
                        scheduleDayOfTheWeek.Add( new ScheduleDayOfTheWeek() { DayOfTheWeekId = item, ScheduleId = schedule.Id, IsActive = true });
                    }

                    db.ScheduleDayOfTheWeeks.AddRange(scheduleDayOfTheWeek);
                }

                //Create ScheduleStations
                ScheduleStation fromSheduleStation = new ScheduleStation() { ScheduleId = schedule.Id, StationId = stations[0], Location = Convert.ToInt32( Location.From) };
                ScheduleStation toScheduleStation = new ScheduleStation() { ScheduleId = schedule.Id, StationId = stations[1], Location = Convert.ToInt32( Location.To) };
                db.ScheduleStations.Add(fromSheduleStation);
                db.ScheduleStations.Add(toScheduleStation);


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TrainTypeId = new SelectList(db.TrainTypes.Where(x => x.IsActive), "Id", "Name", schedule.TrainTypeId);
            var daysOfTheWeek = GetSelectListModel(db.DayOfTheWeeks.ToList());
            ViewBag.DaysOfWeek = daysOfWeek;
            var stationList = new SelectList(db.Stations.Where(x => x.IsActive && x.City.IsActive).ToList(), "Id", "Name");
            ViewBag.Stations = stationList;
            return View(schedule);
        }


        public ActionResult CreateSchedule()
        {
            ViewBag.TrainTypeId = new SelectList(db.TrainTypes.Where(x => x.IsActive), "Id", "Name");
            var daysOfWeek = GetSelectListModel(db.DayOfTheWeeks.ToList());
            ViewBag.DaysOfWeek = daysOfWeek;
            var cities = new SelectList(db.Cities.Where(x => x.IsActive).ToList(), "Id", "Name");
            ViewBag.Cities = cities;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchedule([Bind(Include = "Id,Name,DurationInMinutes,TrainTypeId,TimeOfDay,IsActive")] Schedule schedule,
           List<ScheduleTravelClass> scheduleTravelClasses, int[] daysOfWeek, int[] stations)
        {
            if (ModelState.IsValid)
            {
                //Check seat count
                TrainType trainType = db.TrainTypes.Find(schedule.TrainTypeId);
                int scheduleSeatCount = scheduleTravelClasses.Sum(x => x.NumberOfSeats);

                foreach (var scheduleTravelClass in scheduleTravelClasses)
                {
                    TrainTypeTravelClass trainTypeTravelClass =
                        trainType.TrainTypeTravelClasses.Where(x => x.TravelClassId == scheduleTravelClass.TravelClassId).Single();
                    if (scheduleTravelClass.NumberOfSeats > trainTypeTravelClass.NumberOfSeats)
                    {
                        ModelState.AddModelError("", "The number of seats specified exceeds the specified maximum capacity");
                        ViewBag.TrainTypeId = new SelectList(db.TrainTypes, "Id", "Name");
                        var daysOfWeekList = GetSelectListModel(db.DayOfTheWeeks.ToList());
                        ViewBag.DaysOfWeek = daysOfWeekList;
                        var stationsList = new SelectList(db.Stations.ToList(), "Id", "Name");
                        ViewBag.Stations = stationsList;
                        return View(schedule);
                    }
                }


                db.Schedules.Add(schedule);
                db.SaveChanges();

                //Create ScheduleTravelClasses
                if (scheduleTravelClasses != null && scheduleTravelClasses.Any())
                {
                    foreach (var item in scheduleTravelClasses)
                    {
                        item.ScheduleId = schedule.Id;
                    }

                    db.ScheduleTravelClasses.AddRange(scheduleTravelClasses);
                }

                //Create ScheduleDaysOfTheWeek
                if (daysOfWeek.Any())
                {
                    List<ScheduleDayOfTheWeek> scheduleDayOfTheWeek = new List<ScheduleDayOfTheWeek>();
                    foreach (var item in daysOfWeek)
                    {
                        scheduleDayOfTheWeek.Add(new ScheduleDayOfTheWeek() { DayOfTheWeekId = item, ScheduleId = schedule.Id, IsActive = true });
                    }

                    db.ScheduleDayOfTheWeeks.AddRange(scheduleDayOfTheWeek);
                }

                //Create ScheduleStations
                ScheduleStation fromSheduleStation = new ScheduleStation() { ScheduleId = schedule.Id, StationId = stations[0], Location = Convert.ToInt32(Location.From) };
                ScheduleStation toScheduleStation = new ScheduleStation() { ScheduleId = schedule.Id, StationId = stations[1], Location = Convert.ToInt32(Location.To) };
                db.ScheduleStations.Add(fromSheduleStation);
                db.ScheduleStations.Add(toScheduleStation);


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TrainTypeId = new SelectList(db.TrainTypes.Where(x => x.IsActive), "Id", "Name", schedule.TrainTypeId);
            var daysOfTheWeek = GetSelectListModel(db.DayOfTheWeeks.ToList());
            ViewBag.DaysOfWeek = daysOfWeek;
            var cities = new SelectList(db.Cities.Where(x => x.IsActive).ToList(), "Id", "Name");
            ViewBag.Cities = cities;
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }

            ViewBag.TrainTypeId = new SelectList(db.TrainTypes.Where(x => x.IsActive), "Id", "Name", schedule.TrainTypeId);
            var daysOfWeek = GetSelectListModel(db.DayOfTheWeeks.ToList());
            ViewBag.DaysOfWeek = daysOfWeek;
            var selectedDays = schedule.ScheduleDayOfTheWeeks.Where(x => x.IsActive).Select(x => x.DayOfTheWeekId).AsEnumerable();
            var selectedItems = String.Join(",", selectedDays);
            ViewBag.SelectedItems = selectedItems;
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DurationInMinutes,TrainTypeId,TimeOfDay,IsActive")] Schedule schedule, int[] daysOfWeek,
            List<ScheduleTravelClass> scheduleTravelClasses)
        {
            List<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            if (ModelState.IsValid)
            {
                //Create ScheduleDaysOfTheWeek
                if (daysOfWeek.Any())
                {
                    List<ScheduleDayOfTheWeek> scheduleDayOfTheWeek = new List<ScheduleDayOfTheWeek>();
                    var scheduleDays = db.ScheduleDayOfTheWeeks.Where(x => x.ScheduleId == schedule.Id).AsEnumerable();

                    //Set all existing to inactive then activate the new selections
                    foreach (var item in scheduleDays)
                    {
                        item.IsActive = false; 
                    }

                    foreach (var item in daysOfWeek)
                    {
                        var day = scheduleDays.Where(x => x.DayOfTheWeekId == item).SingleOrDefault();
                        if (day != null)
                        {
                            day.IsActive = true;
                            db.Entry(day).State = EntityState.Modified;
                        }
                        else
                        {
                            ScheduleDayOfTheWeek newScheduleDayOfWeek = new ScheduleDayOfTheWeek() { DayOfTheWeekId = item, ScheduleId = schedule.Id, IsActive = true };
                            db.ScheduleDayOfTheWeeks.Add(newScheduleDayOfWeek);
                        }

                    }

                }

                if (scheduleTravelClasses.Any())
                {
                    foreach (var item in scheduleTravelClasses)
                    {
                        var scheduleTravelClass = db.ScheduleTravelClasses.Find(item.Id);
                        if (scheduleTravelClass != null)
                        {
                            scheduleTravelClass.NumberOfSeats = item.NumberOfSeats;
                            scheduleTravelClass.Price = item.Price;
                            db.Entry(scheduleTravelClass).State = EntityState.Modified;
                        }
                    }
                }

                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TrainTypeId = new SelectList(db.TrainTypes.Where(x => x.IsActive), "Id", "Name", schedule.TrainTypeId);
            var daysOfWeeks = GetSelectListModel(db.DayOfTheWeeks.ToList());
            ViewBag.DaysOfWeek = daysOfWeeks;
            var selectedDays = schedule.ScheduleDayOfTheWeeks.Where(x => x.IsActive).Select(x => x.Id).AsEnumerable();
            var selectedItems = String.Join(",", selectedDays);
            ViewBag.SelectedItems = selectedItems;
            var originalSchedule = db.Schedules.Find(schedule.Id);
            return View(originalSchedule);
        }

        public PartialViewResult GetScheduleTravelClassEdit(Schedule schedule)
        {
            TrainType trainType = schedule.TrainType;
            ViewBag.ScheduleTravelClasses = schedule.ScheduleTravelClasses.AsEnumerable();

            return PartialView("_ScheduleTravelClassEdit", trainType);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            //remove schedule days of the week
            //db.ScheduleDayOfTheWeeks.RemoveRange(schedule.ScheduleDayOfTheWeeks);

            //remove schedule stations
            //db.ScheduleStations.RemoveRange(schedule.ScheduleStations);

            //remove schedule travel classes
            //db.ScheduleTravelClasses.RemoveRange(schedule.ScheduleTravelClasses);
            //db.SaveChanges();

            //db.Schedules.Remove(schedule);

            schedule.IsActive = false;
            db.Entry(schedule).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult GetSchedules(int[] stations, DateTime date)
        {
            try
            {
                ViewBag.Date = date;
                if (date < DateTime.Today)
                {
                    return PartialView("_getSchedules", new List<ScheduleTravelClass>());
                }

                int fromId = stations[0];
                int toId = stations[1];
                int from = Convert.ToInt32(Location.From);
                int to = Convert.ToInt32(Location.To);
                DayOfTheWeek day = db.DayOfTheWeeks.Where(x => x.Name.Equals(date.DayOfWeek.ToString())).Single();
                //var schedules = db.Schedules.Where(x => x.FromStationId == stations[0] && x.ToStationId == stations[1]).ToList();
                // var schedules = db.Schedules.Where(x => x.FromStationId == stations[0] && x.ToStationId == stations[1]).ToList();
                 var schedules = db.Schedules.Where(x => x.ScheduleStations.Any(y => y.StationId == fromId && y.Location == from) 
                 && x.ScheduleStations.Any(y => y.StationId == toId && y.Location == to)).ToList();
                var daySchedules = schedules.Where(x => x.ScheduleDayOfTheWeeks.Any(y => y.DayOfTheWeek == day)).ToList();
                List<ScheduleTravelClass> scheduleTravelClasses = new List<ScheduleTravelClass>();
                foreach (var item in daySchedules)
                {
                    scheduleTravelClasses.AddRange(item.ScheduleTravelClasses);
                }

                return PartialView("_getSchedules", scheduleTravelClasses);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public PartialViewResult GetSchedulesForBooking(int[] stations, DateTime date)
        {
            try
            {
                ViewBag.Date = date;
                if (date < DateTime.Today)
                {
                    return PartialView("_getBookingSchedules", new List<ScheduleTravelClass>());
                }

                int fromId = stations[0];
                int toId = stations[1];
                int from = Convert.ToInt32(Location.From);
                int to = Convert.ToInt32(Location.To);
                DayOfTheWeek day = db.DayOfTheWeeks.Where(x => x.Name.Equals(date.DayOfWeek.ToString())).Single();
                var paymentMethods = new SelectList(db.PaymentMethods.Where(x => x.IsActive).AsEnumerable(), "Id", "Name");
                ViewBag.PaymentMethodId = paymentMethods;
        
                var schedules = db.Schedules.Where(x => x.IsActive && x.ScheduleStations.Any(y => y.StationId == fromId && y.Location == from)
                && x.ScheduleStations.Any(y => y.StationId == toId && y.Location == to)).ToList();
                var daySchedules = schedules.Where(x => x.ScheduleDayOfTheWeeks.Any(y => y.DayOfTheWeek == day)).ToList();
                List<ScheduleTravelClass> scheduleTravelClasses = new List<ScheduleTravelClass>();
                foreach (var item in daySchedules)
                {
                    scheduleTravelClasses.AddRange(item.ScheduleTravelClasses);
                }

                return PartialView("_getBookingSchedules", scheduleTravelClasses);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public PartialViewResult GetSchedulesForBookingByCity(int[] cities, DateTime date)
        {
            try
            {
                ViewBag.Date = date;
                if (date < DateTime.Today)
                {
                    return PartialView("_getBookingSchedules", new List<ScheduleTravelClass>());
                }

                int fromId = cities[0];
                int toId = cities[1];
                int from = Convert.ToInt32(Location.From);
                int to = Convert.ToInt32(Location.To);
                DayOfTheWeek day = db.DayOfTheWeeks.Where(x => x.Name.Equals(date.DayOfWeek.ToString())).Single();
                var paymentMethods = new SelectList(db.PaymentMethods.Where(x => x.IsActive).AsEnumerable(), "Id", "Name");
                ViewBag.PaymentMethodId = paymentMethods;

                var schedules = db.Schedules.Where(x => x.IsActive && x.ScheduleStations.Any(y => y.Station.CityId == fromId && y.Location == from)
                && x.ScheduleStations.Any(y => y.Station.CityId == toId && y.Location == to)).ToList();
                var daySchedules = schedules.Where(x => x.ScheduleDayOfTheWeeks.Any(y => y.DayOfTheWeek == day)).ToList();
                List<ScheduleTravelClass> scheduleTravelClasses = new List<ScheduleTravelClass>();
                foreach (var item in daySchedules)
                {
                    scheduleTravelClasses.AddRange(item.ScheduleTravelClasses);
                }

                ViewBag.TravelClasses = db.TravelClasses.Where(x => x.IsActive).ToList();
                ViewBag.BoardingStationId = new SelectList(db.Stations.Where(x => x.CityId == fromId), "Id", "Name");
                //return PartialView("_getBookingSchedulesByCity", scheduleTravelClasses);
                return PartialView("_getBookingSchedulesByCity", schedules);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private List<SelectListItem> GetSelectListModel(List<DayOfTheWeek> daysOfWeek)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (daysOfWeek == null || !daysOfWeek.Any())
            {
                return selectList;
            }

            foreach (var day in daysOfWeek)
            {
                selectList.Add(new SelectListItem { Text = day.Name, Value = day.Id.ToString() });
            }

            return selectList;
        }
    }
}
