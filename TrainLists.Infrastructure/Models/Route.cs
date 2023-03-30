using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainLists.Infrastructure.Extensions;

namespace TrainLists.Infrastructure.Models
{
    public class Route : Entity
    {
        public string From { get; set; }
        public string To { get; set; }
        public virtual List<TrainActivity> TrainActivities { get; set; }
        
    }

    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.From).IsString(128);
            builder.Property(x=>x.To).IsString(128);

        }
    }
}