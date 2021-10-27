using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherProject.Models
{
    public class CreateObjects
    {
        internal static List<EventInfo> GetTechEvents()
        {
            List<EventInfo> techEvents = GetEventsWithoutWeather();
            AddWeather(techEvents);
            return techEvents;
        }

        private static void AddWeather(List<EventInfo> eventsWithoutWeather)
        {
            foreach (var techEvent in eventsWithoutWeather)
            {
                if (techEvent.IsRemote)
                {
                    techEvent.WeatherDescription = "N/A - Virtual event";
                    continue;
                }
                if(techEvent.City == "United Kingdom")
                {
                    techEvent.WeatherDescription = "City unknown, so no forecast";
                    continue;
                }
                string url = GetURLForLongAndLat(techEvent.Longtitude, techEvent.Latitude);
                // Base forecast on latitude and longitude rather than city where possible
                if (String.IsNullOrEmpty(techEvent.Latitude) || String.IsNullOrEmpty(techEvent.Longtitude))
                {
                    url = GetUrlForCity(techEvent.City);
                }
                string json = new System.Net.WebClient().DownloadString(url); // Gets JSON
                var objs = JsonConvert.DeserializeObject<Rootobject>(json); // Deserialises JSON
                // If no forecast returned.
                if(objs.cod == "404")
                {
                    techEvent.WeatherDescription = "No forecast available";
                    continue;
                }
                foreach (var weather_obj in objs.list)
                {
                    // Get time and date of forecast in local time (events are listed in local time).
                    int year = Convert.ToInt32(weather_obj.dt_txt.Substring(0, 4));
                    int month = Convert.ToInt32(weather_obj.dt_txt.Substring(5, 2));
                    int day = Convert.ToInt32(weather_obj.dt_txt.Substring(8, 2));
                    int hour = Convert.ToInt32(weather_obj.dt_txt.Substring(11, 2));
                    int min = Convert.ToInt32(weather_obj.dt_txt.Substring(14, 2));
                    DateTime timeOfWeather = new DateTime(year, month, day, hour, min, 0);
                    DateTime tellCompilerWeatherIsUTC = DateTime.SpecifyKind(timeOfWeather, DateTimeKind.Utc);
                    DateTime weatherLocalTime = tellCompilerWeatherIsUTC.ToLocalTime();
                    // Weather forecasts are listed from earliest to latest.
                    // The first weather forecast after the event is used (there are forecasts every 3 hours).
                    if (DateTime.Compare(weatherLocalTime, techEvent.EventDate) < 0)
                    {
                        continue;
                    }
                    else
                    {
                        techEvent.WeatherDescription = char.ToUpper(weather_obj.weather[0].description[0]) + weather_obj.weather[0].description.Substring(1); // Start of forecast capitalised.
                        break;
                    }
                }
            }
        }

        private static string GetURLForLongAndLat(string longtitude, string latitude)
        {
            return "http://api.openweathermap.org/data/2.5/forecast?lat=" + latitude + "&lon=" + longtitude + "&appid=e713f513019bae6baa220783378d7945";
        }

        private static string GetUrlForCity(string city)
        {
            return "http://api.openweathermap.org/data/2.5/forecast?q=" + city + ",GB&appid=e713f513019bae6baa220783378d7945";
        }

        private static List<EventInfo> GetEventsWithoutWeather()
        {
            string url = "https://opentechcalendar.co.uk/api1/events.json";
            string json = new System.Net.WebClient().DownloadString(url); // Gets JSON
            var objs = JsonConvert.DeserializeObject<Root>(json); // Deseralises JSON and creates objects
            objs.data = ProcessEntriesWithoutCity(objs.data);
            var listOfObjs = objs.data.OrderBy(a => a.areas[0].title).ToList();
            List<EventInfo> eventsWithoutWeather = new List<EventInfo>();
            foreach (var entry in listOfObjs)
            {
                if (entry.cancelled || entry.country.title != "United Kingdom" || entry.deleted)
                {
                    continue;
                }
                EventInfo obj = new EventInfo();
                obj.EventDate = new DateTime(Convert.ToInt32(entry.start.yearlocal), Convert.ToInt32(entry.start.monthlocal), Convert.ToInt32(entry.start.daylocal),
                    Convert.ToInt32(entry.start.hourlocal), Convert.ToInt32(entry.start.minutelocal), 0);
                // Removes from consideration anything more than 5 days from now.
                if (obj.EventDate > DateTime.Now.AddDays(5))
                {
                    continue;
                }
                obj.Summary = entry.summary;
                var theDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                obj.URL = "https://opentechcalendar.co.uk/event/" + entry.slugforurl;
                obj.City = entry.areas[0].title;
                obj.IsRemote = entry.is_virtual;
                obj.TimeOfEvent = entry.start.hourlocal + ":" + entry.start.minutelocal;
                if (Convert.ToInt32(entry.start.hourlocal) < 12)
                {
                    obj.TimeOfEvent += " am";
                }
                // To avoid null exception errors if longtitude and/or latitude are missing.
                if (entry.venue is null || String.IsNullOrEmpty(entry.venue.lat))
                {
                    obj.Latitude = "";
                }
                else
                {
                    obj.Latitude = entry.venue.lat;
                }
                if (entry.venue is null || String.IsNullOrEmpty(entry.venue.lng))
                {
                    obj.Longtitude = "";
                }
                else
                {
                    obj.Longtitude = entry.venue.lng;
                }
                eventsWithoutWeather.Add(obj);
            }
            return eventsWithoutWeather;
        }

        private static Datum[] ProcessEntriesWithoutCity(Datum[] toCheck)
        {
            foreach (var entry in toCheck)
            {
                // If areas[] array is null
                if (entry.areas == null)
                {
                    Area area = new Area();
                    area.title = "United Kingdom";
                    Area [] areas = { area};
                    entry.areas = areas;
                    continue;
                }
                // If no city data in areas[] array
                if (String.IsNullOrEmpty(entry.areas[0].title))
                {
                    entry.areas[0].title = "United Kingdom";
                }
            }
            return toCheck;
        }
    }
}