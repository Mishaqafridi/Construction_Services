using Construction_Admin_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construction_Admin_Service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<ContractorQuotation> ContractorQuotations { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nationality>().HasData(
                new Nationality { Id = 1, Nationality_Name = "Emirati" },
                new Nationality { Id = 2, Nationality_Name = "Egyptian" },
                new Nationality { Id = 3, Nationality_Name = "Iranian" },
                new Nationality { Id = 4, Nationality_Name = "Pakistani" },
                new Nationality { Id = 5, Nationality_Name = "Jordanian" },
                new Nationality { Id = 6, Nationality_Name = "Kuwaiti" },
                new Nationality { Id = 7, Nationality_Name = "Libyan" },
                new Nationality { Id = 8, Nationality_Name = "Indian" }

            );
            modelBuilder.Entity<Experience>().HasData(
                new Experience { Id = 1, Detail = "Warehouse associate" },
                new Experience { Id = 2, Detail = "Assistant counselor" },
                new Experience { Id = 3, Detail = "Accountant" },
                new Experience { Id = 4, Detail = "HR" },
                new Experience { Id = 5, Detail = "Business Analyst" },
                new Experience { Id = 6, Detail = "Document Controller" },
                new Experience { Id = 7, Detail = "Software Developer" },
                new Experience { Id = 8, Detail = "Other" }

            );
            modelBuilder.Entity<User>().Property(user => user.Role).HasDefaultValue("user");
        }

    }
}
