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
    public class HomeController : Controller
    {
        private EventInfoContext db = new EventInfoContext();

        // GET: EventInfoes
        public ActionResult Index()
        {
            DeleteRecords(); // Deletes all records in the database
            List<EventInfo> techEvents = CreateObjects.GetTechEvents(); // Returns objects relating to the tech events (including weather forecasts)
            AddRecords(techEvents); // Populates the database with the newly created objects.
            return View(db.EventInfo.ToList());
        }

        private void AddRecords(List<EventInfo> techEvents)
        {
            for (int i = 0; i < techEvents.Count; i++)
            {
                db.EventInfo.Add(techEvents[i]);
            }
            db.SaveChanges();
        }

        private void DeleteRecords()
        {
            foreach (var record in db.EventInfo)
            {
                db.EventInfo.Remove(record);
            }
            db.SaveChanges();
        }
    }
}
