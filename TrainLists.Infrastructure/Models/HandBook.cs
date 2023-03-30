using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainLists.Infrastructure.Extensions;

namespace TrainLists.Infrastructure.Models
{
    public class HandBook : Entity
    {
        public string Type { get; set; }
        
        public int Key { get; set; }

        public string Value { get; set; }

    }

    public class HandBookConfiguration : IEntityTypeConfiguration<HandBook>
    {
        public void Configure(EntityTypeBuilder<HandBook> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Type).IsRequiredString(25);
            builder.Property(x=>x.Key).IsRequired();
            builder.Property(x=>x.Value).IsRequiredString(128);

            builder.HasIndex(x=>x.Type, "IX_HandBooks_Type");
            
            builder.HasIndex(x=> new {x.Type, x.Key}, "IX_HandBooks_TypeKey").IsUnique();

            


           
        }
    }
}