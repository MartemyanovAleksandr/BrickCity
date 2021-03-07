using BrickCity.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrickCity.Models.DB
{
    public class BrickDB : DbContext
    {
        public DbSet<Plant> Plant { get; set; }
        public DbSet<House> House { get; set; }
        public DbSet<PlantsConsumption> PlantsConsumption { get; set; }
        public DbSet<HouseConsumption> HouseConsumption { get; set; }
       
        public BrickDB(DbContextOptions<BrickDB> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
