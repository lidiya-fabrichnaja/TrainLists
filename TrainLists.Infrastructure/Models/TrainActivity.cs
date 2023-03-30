using System.Dynamic;
using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainLists.Infrastructure.Extensions;


namespace TrainLists.Infrastructure.Models
{
    public class TrainActivity : Entity
    {
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
        
        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Номер поезда
        /// </summary>
        /// <value></value>
        public int TrainNumber { get; set; }

        /// <summary>
        /// Номер состава
        /// </summary>
        /// <value></value>
        public int RailwayNumber { get; set; }

        /// <summary>
        /// индекс поезда
        /// </summary>
        /// <value></value>
        public string TrainIndexCombined { get; set; }  

        public int RouteId { get; set; }
 
        public string LastStationName { get; set; }
        public int LastOperationKey { get; set; }
        public DateTime? LastOperationDate { get; set; }

        public virtual Route Route { get; set; }

        public virtual List<TrainActivityDetail> TrainActivityDetails { get; set; } 

    }

    public class TrainActivityConfiguration : IEntityTypeConfiguration<TrainActivity>
    {
        public void Configure(EntityTypeBuilder<TrainActivity> builder)
        {
            builder.HasKey(x=>x.Id);

            builder.HasOne(x=>x.Route)
                .WithMany(x=>x.TrainActivities)
                .HasForeignKey(x=>x.RouteId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x=>x.CreatedAt)
                   .IsRequiredDateTimeDefault();

            builder.Property(x=>x.ModifiedAt)
                   .IsDateTime(); 

            builder.Property(x=>x.CreatedBy)
                   .IsRequiredString(50)
                   .HasDefaultValue("handinput");

            builder.Property(x=>x.ModifiedBy)
                   .IsString(50);

            builder.Property(x=>x.TrainNumber)
                   .IsRequired();
            
            builder.Property(x=>x.RailwayNumber)
                    .IsRequired();

            builder.Property(x=>x.TrainIndexCombined)
                    .IsRequiredString(20);   

            builder.Property(x=>x.LastOperationKey)
                    .IsRequired();

            builder.Property(x=>x.LastOperationDate)
                    .IsDateTime();

            
            builder.HasIndex(x=>x.TrainNumber, "IX_TrainActivity_TrainNumber");

            builder.HasIndex(x=>x.TrainIndexCombined, "IX_TrainActivity_TrainIndexCombined");

            builder.HasIndex(x=>x.LastOperationDate, "IX_TrainActivity_LastOperationDate");


        }
    }
}