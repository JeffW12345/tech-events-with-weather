﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WeatherProject.Models
{
    public class EventInfoContext : DbContext
    {
        public DbSet<EventInfo> EventInfo { get; set; }
    }
}