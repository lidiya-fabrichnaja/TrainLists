using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainLists.Infrastructure.Extensions;

namespace TrainLists.Infrastructure.Models
{
    public class TrainActivityDetail : Entity
    {
        public int? InvoiceId { get; set; }

        public int? TrainActivityId { get; set; }

        public int CarNumber { get; set; }

        public int PositionInTrain { get; set; }

        public int FreightEtsngKey { get; set; }

        public int FreightTotalWeightKg { get; set; }

        public Invoice Invoice { get; set; }

        public TrainActivity TrainActivity { get; set; }

    }

    public class TrainActivityDetailConfiguration : IEntityTypeConfiguration<TrainActivityDetail>
    {
        public void Configure(EntityTypeBuilder<TrainActivityDetail> builder)
        {
            builder.HasKey(x=>x.Id);

            builder.HasOne(x=>x.TrainActivity)
                    .WithMany(x=>x.TrainActivityDetails)
                    .HasForeignKey(x=>x.TrainActivityId)
                    .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(x=>x.Invoice)
                    .WithMany(x=>x.TrainActivityDetails)
                    .HasForeignKey(x=>x.InvoiceId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x=>x.CarNumber)
                    .IsRequired();
            
            builder.Property(x=>x.PositionInTrain)
                    .IsRequired();

            builder.Property(x=>x.FreightEtsngKey)
                    .IsRequired();
            
            builder.Property(x=>x.FreightTotalWeightKg)
                    .IsRequired();

            builder.HasIndex(x=>x.CarNumber, "IX_TrainActivityDetails_CarNumber");

           
        }
    }
}