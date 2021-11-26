using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public sealed class WeatherDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Temperature> Temperatures { get; set; }

        public WeatherDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherDbContext).Assembly);
        }
    }
}
