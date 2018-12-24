using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NRC_WEB.Models;
using System.Text;
using NRC_WEB.Utility;

namespace NRC_WEB.Controllers
{
    public class BookingsController : Controller
    {
        private NrcDbContext db = new NrcDbContext();

        // GET: Bookings
        public ActionResult Index()
        {
            //var bookings = db.Bookings.Include(b => b.PaymentMethod).Include(b => b.ScheduleTravelClass);
            var bookings = db.Bookings.Include(b => b.PaymentMethod);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            //ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name");
            //ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Name");
            var stations = new SelectList(db.Stations.Where(r => r.IsActive && r.City.IsActive).AsEnumerable(), "Id", "Name");
            ViewBag.Stations = stations;
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerName,Email,Phone,PaymentMethodId,scheduleTravelClassId,Date,BoardingStationId")] Booking booking,
            int? scheduleTravelClassId, int? numberOfTickets, List<BookingCheckModel> bookingCheckModels)
        {
            var checkedModels = bookingCheckModels.Where(x => x.Checked);

            //check number of available tickets
            if (ModelState.IsValid)
            {
                StringBuilder errorMessage = new StringBuilder();

                foreach (var item in checkedModels)
                {
                    ScheduleTravelClass scheduleTravelClass = db.ScheduleTravelClasses.Find(item.ScheduleTravelClassId);
                    if (scheduleTravelClass != null)
                    {
                        int remainingSeats = scheduleTravelClass.GetNumberOfRemainingSeats(booking.Date.Value);
                        if (item.NumberOfTickets > remainingSeats)
                        {
                            string message = string.Format("Booking exceeded number of available seats for {0} class. Only {1} seat(s) left.", scheduleTravelClass.TravelClass.Name, remainingSeats);
                           // ModelState.AddModelError("", message);
                            errorMessage.AppendLine(message); 
                        }

                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = errorMessage.ToString();
                    ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods.Where(x => x.IsActive), "Id", "Name", booking.PaymentMethodId);
                    ViewBag.ScheduleTravelClassId = new SelectList(db.Schedules.Where(x => x.IsActive), "Id", "Name", booking.ScheduleTravelClassId);
                    var stationList = new SelectList(db.Stations.Where(x => x.IsActive).AsEnumerable(), "Id", "Name");
                    ViewBag.Stations = stationList;
                    return View(booking);
                }
               
            }

            if (ModelState.IsValid)
            {
                booking.NumberOfTickets = checkedModels.Sum(x => x.NumberOfTickets);
                db.Bookings.Add(booking);
                db.SaveChanges();

                foreach (var item in checkedModels)
                {
                    for (int i = 0; i < item.NumberOfTickets; i++)
                    {
                        var ticket = new Ticket()
                        {
                            BookingId = booking.Id,
                            ScheduleTravelClassId = item.ScheduleTravelClassId,
                            TicketNumber = Guid.NewGuid().ToString()
                        };

                        db.Tickets.Add(ticket);
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Details", new { id = booking.Id });
            }

            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods.Where(x => x.IsActive), "Id", "Name", booking.PaymentMethodId);
            ViewBag.ScheduleTravelClassId = new SelectList(db.Schedules.Where(x => x.IsActive), "Id", "Name", booking.ScheduleTravelClassId);
            var stations = new SelectList(db.Stations.Where(x => x.IsActive && x.City.IsActive).AsEnumerable(), "Id", "Name");
            ViewBag.Stations = stations;
            return View(booking);
        }


        // GET: Bookings/Create
        public ActionResult CreateBooking()
        {
            var cities = new SelectList(db.Cities.Where(r => r.IsActive).AsEnumerable(), "Id", "Name");
            ViewBag.Cities = cities;
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBooking([Bind(Include = "Id,CustomerName,Email,Phone,PaymentMethodId,scheduleTravelClassId,Date,BoardingStationId")] Booking booking,
            int? scheduleTravelClassId, int? numberOfTickets, List<BookingCheckModel> bookingCheckModels)
        {
            var checkedModels = bookingCheckModels.Where(x => x.Checked);

            //check number of available tickets
            if (ModelState.IsValid)
            {
                StringBuilder errorMessage = new StringBuilder();

                foreach (var item in checkedModels)
                {
                    ScheduleTravelClass scheduleTravelClass = db.ScheduleTravelClasses.Find(item.ScheduleTravelClassId);
                    if (scheduleTravelClass != null)
                    {
                        int remainingSeats = scheduleTravelClass.GetNumberOfRemainingSeats(booking.Date.Value);
                        if (item.NumberOfTickets > remainingSeats)
                        {
                            string message = string.Format("Booking exceeded number of available seats for {0} class. Only {1} seat(s) left.", scheduleTravelClass.TravelClass.Name, remainingSeats);
                            // ModelState.AddModelError("", message);
                            errorMessage.AppendLine(message);
                        }

                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = errorMessage.ToString();
                    ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods.Where(x => x.IsActive), "Id", "Name", booking.PaymentMethodId);
                    ViewBag.ScheduleTravelClassId = new SelectList(db.Schedules.Where(x => x.IsActive), "Id", "Name", booking.ScheduleTravelClassId);
                    var stationList = new SelectList(db.Stations.Where(x => x.IsActive).AsEnumerable(), "Id", "Name");
                    ViewBag.Stations = stationList;
                    return View(booking);
                }

            }

            if (ModelState.IsValid)
            {
                booking.NumberOfTickets = checkedModels.Sum(x => x.NumberOfTickets);
                db.Bookings.Add(booking);
                db.SaveChanges();

                AzureStoreManager manager = new AzureStoreManager(); 
                foreach (var item in checkedModels)
                {
                    for (int i = 0; i < item.NumberOfTickets; i++)
                    {
                        var ticket = new Ticket()
                        {
                            BookingId = booking.Id,
                            ScheduleTravelClassId = item.ScheduleTravelClassId,
                            TicketNumber = Guid.NewGuid().ToString()
                        };

                        manager.SaveBarCodeToCloud(ticket);
                        db.Tickets.Add(ticket);
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Details", new { id = booking.Id });
            }

            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods.Where(x => x.IsActive), "Id", "Name", booking.PaymentMethodId);
            ViewBag.ScheduleTravelClassId = new SelectList(db.Schedules.Where(x => x.IsActive), "Id", "Name", booking.ScheduleTravelClassId);
            var cities = new SelectList(db.Cities.Where(x => x.IsActive).AsEnumerable(), "Id", "Name");
            ViewBag.Cities = cities;
            return View(booking);
        }



        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods.Where(x => x.IsActive), "Id", "Name", booking.PaymentMethodId);
            ViewBag.ScheduleTravelClassId = new SelectList(db.Schedules.Where(x => x.IsActive), "Id", "Name", booking.ScheduleTravelClassId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerName,Email,Phone,PaymentMethodId,ScheduleId,Date,BoardingStationId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods.Where(x => x.IsActive), "Id", "Name", booking.PaymentMethodId);
            ViewBag.ScheduleTravelClassId = new SelectList(db.Schedules.Where(x => x.IsActive), "Id", "Name", booking.ScheduleTravelClassId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Tickets.RemoveRange(booking.Tickets);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
