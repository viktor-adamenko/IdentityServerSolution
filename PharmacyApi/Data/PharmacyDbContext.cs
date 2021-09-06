using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApi.Data
{
    public class PharmacyDbContext : DbContext
    {
        public DbSet<Pill> Pills { get; set; }

        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
