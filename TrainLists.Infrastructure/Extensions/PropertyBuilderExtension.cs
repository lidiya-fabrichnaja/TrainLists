using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrainLists.Infrastructure.Extensions
{

    public static class PropertyBuilderExtension
    {
        /// <summary>
        ///  Настройка обязательного строкового поля
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static PropertyBuilder<string> IsRequiredString(this PropertyBuilder<string> builder, int maxLength){
            
            return builder
                .IsUnicode(false)
                .HasMaxLength(maxLength)
                .IsRequired();
        }

        /// <summary>
        /// Настройка необязательного строкового поля
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static PropertyBuilder<string> IsString(this PropertyBuilder<string> builder, int maxLength)
        {
            return builder
                .IsUnicode(false)
                .HasMaxLength(maxLength);
        }



        public static PropertyBuilder<DateTime?> IsDateTime(this PropertyBuilder<DateTime?> builder)
        {
            return builder
            .HasColumnType("datetime");
        }

        public static PropertyBuilder<DateTime?> IsDateTimeDefault(this PropertyBuilder<DateTime?> builder)
        {
            return builder
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        }

        public static PropertyBuilder<DateTime> IsRequiredDateTimeDefault(this PropertyBuilder<DateTime> builder)
        {
            return builder
            .HasDefaultValueSql("(getdate())")
            .IsRequired()
            .HasColumnType("datetime");
        }

        public static PropertyBuilder<byte?> IsByteDefaultValueZero(this PropertyBuilder<byte?> builder)
        {
            return builder
            .HasDefaultValueSql("((0))");
        }
        public static PropertyBuilder<short?> IsShortDefaultValueZero(this PropertyBuilder<short?> builder)
        {
            return builder
            .HasDefaultValueSql("((0))");
        }
        public static PropertyBuilder<int?> IsIntDefaultValueZero(this PropertyBuilder<int?> builder)
        {
            return builder
            .HasDefaultValueSql("((0))");
        }

        public static PropertyBuilder<string> IsStringDefaultValueNewGuid(this PropertyBuilder<string> builder)
        {
            return builder
            .HasDefaultValueSql("(newid())");
        }


    }
}