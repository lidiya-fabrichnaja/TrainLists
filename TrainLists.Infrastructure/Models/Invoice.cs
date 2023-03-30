using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainLists.Infrastructure.Extensions;

namespace TrainLists.Infrastructure.Models
{
    public class Invoice : Entity
    {
        public string Number { get; set; }

        public virtual List<TrainActivityDetail> TrainActivityDetails { get; set; }
    }

    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Number)
                .IsRequiredString(50);

            builder.HasIndex(x=>x.Number, "IX_Invoice_Number");
        }
    }
}