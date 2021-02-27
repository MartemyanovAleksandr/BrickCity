using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models
{
    public class BrickDB : DbContext
    {
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Consumption> Consumption { get; set; }
        public DbSet<Weather> Weather { get; set; }
        public DbSet<Price> BrickPrice { get; set; }

        public BrickDB(DbContextOptions<BrickDB> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
