using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeatherProject.Models;

namespace WeatherProject.Controllers
{
    public class EventInfoesController : Controller
    {
        private EventInfoContext db = new EventInfoContext();

        // GET: EventInfoes
        public ActionResult Index()
        {
            DeleteRecords();
            List<EventInfo> techEvents = CreateObjects.GetTechEvents();
            AddRecords(techEvents);
            return View(db.EventInfo.ToList());
        }

        public void AddRecords(List<EventInfo> techEvents)
        {
            foreach (var techEvent in techEvents)
            {
                db.EventInfo.Add(techEvent);
            }
            db.SaveChanges();
        }

        // GET: EventInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventInfo eventInfo = db.EventInfo.Find(id);
            if (eventInfo == null)
            {
                return HttpNotFound();
            }
            return View(eventInfo);
        }

        // GET: EventInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Summary,Description,EventDate,HourOfEvent,MinOfEvent,WeatherDescription")] EventInfo eventInfo)
        {
            if (ModelState.IsValid)
            {
                db.EventInfo.Add(eventInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventInfo);
        }

        // GET: EventInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventInfo eventInfo = db.EventInfo.Find(id);
            if (eventInfo == null)
            {
                return HttpNotFound();
            }
            return View(eventInfo);
        }

        // POST: EventInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Summary,Description,EventDate,HourOfEvent,MinOfEvent,WeatherDescription")] EventInfo eventInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventInfo);
        }

        // GET: EventInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventInfo eventInfo = db.EventInfo.Find(id);
            if (eventInfo == null)
            {
                return HttpNotFound();
            }
            return View(eventInfo);
        }

        // POST: EventInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventInfo eventInfo = db.EventInfo.Find(id);
            db.EventInfo.Remove(eventInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void DeleteRecords()
        {
            foreach (var record in db.EventInfo)
            {
                db.EventInfo.Remove(record);
            }
            db.SaveChanges();
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
