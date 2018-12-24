using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NRC_WEB.Models;

namespace NRC_WEB.Controllers
{
    public class TrainTypesController : Controller
    {
        private NrcDbContext db = new NrcDbContext();

        // GET: TrainTypes
        public ActionResult Index()
        {
            ViewBag.TravelClasses = db.TravelClasses.AsEnumerable<TravelClass>();
            return View(db.TrainTypes.ToList());
        }

        // GET: TrainTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainType trainType = db.TrainTypes.Find(id);
            if (trainType == null)
            {
                return HttpNotFound();
            }
            return View(trainType);
        }

        // GET: TrainTypes/Create
        public ActionResult Create()
        {
            var travelClasses = db.TravelClasses.AsEnumerable<TravelClass>();
            ViewBag.TravelClasses = travelClasses;
            return View();
        }

        // POST: TrainTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Capacity,IsActive")] TrainType trainType, List<TrainTypeTravelClass> trainTypeTravelClasses)
        {
            if (ModelState.IsValid)
            {
                //check capacity
                if (trainTypeTravelClasses != null)
                {
                    int travelClassesCount = trainTypeTravelClasses.Sum(x => x.NumberOfSeats).Value;
                    if (travelClassesCount > trainType.Capacity)
                    {
                        ModelState.AddModelError("Capacity", "The sum of travel class seats should not exceed the capacity");
                        var travelClasses = db.TravelClasses.AsEnumerable<TravelClass>();
                        ViewBag.TravelClasses = travelClasses;
                        return View(trainType);

                    }
                }
                db.TrainTypes.Add(trainType);
                db.SaveChanges();

                //Set TrainTypeId in trainTypeTravelClasses
                foreach (var item in trainTypeTravelClasses)
                {
                    item.TrainTypeId = trainType.Id;
                }

                db.TrainTypeTravelClasses.AddRange(trainTypeTravelClasses);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(trainType);
        }

        // GET: TrainTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainType trainType = db.TrainTypes.Find(id);
            if (trainType == null)
            {
                return HttpNotFound();
            }
            return View(trainType);
        }

        // POST: TrainTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Capacity,IsActive")] TrainType trainType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainType);
        }

        // GET: TrainTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainType trainType = db.TrainTypes.Find(id);
            if (trainType == null)
            {
                return HttpNotFound();
            }
            return View(trainType);
        }

        // POST: TrainTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainType trainType = db.TrainTypes.Find(id);
            //db.TrainTypes.Remove(trainType);
            db.Entry(trainType).State = EntityState.Modified;
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

        public PartialViewResult TrainTypeTravelClassInfo(int trainTypeId)
        {
            var trainType = db.TrainTypes.Find(trainTypeId);
            if (trainType != null)
            {
                return PartialView("_TrainTypeTravelClassInfo", trainType);
            }

            return null;
        }



    }
}
