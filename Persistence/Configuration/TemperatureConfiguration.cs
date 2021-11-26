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
    internal sealed class TemperatureConfiguration : IEntityTypeConfiguration<Temperature>
    {
        public void Configure(EntityTypeBuilder<Temperature> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.DateTime).HasColumnType("datetime2")
                .HasDefaultValueSql("getdate()")
                .HasPrecision(0)
                .IsRequired();

            builder.Property(t => t.CelsiusDegrees).IsRequired();

            builder.HasData(
                    //For Kharkiv
                    new Temperature
                    {
                        Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                        DateTime = new DateTime(2021, 10, 20),
                        CelsiusDegrees = 15,
                        CityId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                    },
                    new Temperature
                    {
                        Id = new Guid("7eb16423-225d-4ba4-8372-1ce2586fe765"),
                        DateTime = new DateTime(2021, 10, 21),
                        CelsiusDegrees = 14,
                        CityId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                    },
                    //For Kyiv
                    new Temperature
                    {
                        Id = new Guid("def22be0-5a3d-484c-9d34-d7f81322c971"),
                        DateTime = new DateTime(2021, 10, 20),
                        CelsiusDegrees = 17,
                        CityId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
                    },
                    new Temperature
                    {
                        Id = new Guid("1ae21e68-a465-4684-961c-1c2fe29c35c2"),
                        DateTime = new DateTime(2021, 10, 21),
                        CelsiusDegrees = 15,
                        CityId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
                    },
                    //For Lviv
                    new Temperature
                    {
                        Id = new Guid("f3017f31-ee17-4445-9bf8-0cd153258090"),
                        DateTime = new DateTime(2021, 10, 20),
                        CelsiusDegrees = 12,
                        CityId = new Guid("b2434fb3-c190-4402-8a53-933fed036753")
                    },
                    new Temperature
                    {
                        Id = new Guid("01430a7d-b7a6-469b-9618-8fb8217dcfc6"),
                        DateTime = new DateTime(2021, 10, 21),
                        CelsiusDegrees = 10,
                        CityId = new Guid("b2434fb3-c190-4402-8a53-933fed036753")
                    }
                );
        }
    }
}
