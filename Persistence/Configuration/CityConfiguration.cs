using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired();
            builder.HasMany(c => c.Temperatures)
                .WithOne()
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(c => c.AvgTemperature).HasColumnType("decimal(5,2)");

            builder.HasData(
                    new City
                    {
                        Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                        Name = "Kharkiv",
                        AvgTemperature = 12,
                        MaxTemperature = 12,
                        MinTemperature = 11
                    },
                    new City
                    {
                        Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                        Name = "Kyiv",
                        AvgTemperature = 12,
                        MaxTemperature = 12,
                        MinTemperature = 11
                    },
                    new City
                    {
                        Id = new Guid("b2434fb3-c190-4402-8a53-933fed036753"),
                        Name = "Lviv",
                        AvgTemperature = 12,
                        MaxTemperature = 12,
                        MinTemperature = 11
                    }
                );
        }
    }
}
