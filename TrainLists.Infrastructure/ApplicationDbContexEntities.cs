using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainLists.Infrastructure.Models;

namespace TrainLists.Infrastructure
{
    public partial class ApplicationDbContext
    {
        public DbSet<HandBook> HandBooks { get; set; }

        public DbSet<Invoice>  Invoices { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<TrainActivity> TrainActivities { get; set; }

        public DbSet<TrainActivityDetail> TrainActivityDetails { get; set; }
    }
}