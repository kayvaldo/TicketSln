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
    public class TravelClassesController : Controller
    {
        private NrcDbContext db = new NrcDbContext();

        // GET: TravelClasses
        public ActionResult Index()
        {
            return View(db.TravelClasses.ToList());
        }

        // GET: TravelClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelClass travelClass = db.TravelClasses.Find(id);
            if (travelClass == null)
            {
                return HttpNotFound();
            }
            return View(travelClass);
        }

        // GET: TravelClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TravelClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive")] TravelClass travelClass)
        {
            if (ModelState.IsValid)
            {
                db.TravelClasses.Add(travelClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(travelClass);
        }

        // GET: TravelClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelClass travelClass = db.TravelClasses.Find(id);
            if (travelClass == null)
            {
                return HttpNotFound();
            }
            return View(travelClass);
        }

        // POST: TravelClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Active")] TravelClass travelClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travelClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(travelClass);
        }

        // GET: TravelClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelClass travelClass = db.TravelClasses.Find(id);
            if (travelClass == null)
            {
                return HttpNotFound();
            }
            return View(travelClass);
        }

        // POST: TravelClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TravelClass travelClass = db.TravelClasses.Find(id);

            //delete Schedule travel classes
            // db.ScheduleTravelClasses.RemoveRange(travelClass.ScheduleTravelClasses);

            //delete train type travel class
            //db.TrainTypeTravelClasses.RemoveRange(travelClass.TrainTypeTravelClasses);

            //db.SaveChanges();

            //db.TravelClasses.Remove(travelClass);

            travelClass.IsActive = false;
            db.Entry(travelClass).State = EntityState.Modified;
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
