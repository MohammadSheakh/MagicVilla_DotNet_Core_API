using MagicVilla.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Api.Database
{
    public class ApplicationDbContext : DbContext
    {
        // constructor
        public ApplicationDbContext(
            // dependency injection 
            DbContextOptions<ApplicationDbContext> options
            ):base(options) // base class e pass korlam .. 
        {}

        // individual dbset
        public DbSet<Villa> Villas { get; set; } // name will be table name 
        public DbSet<VillaNumber> VillaNumbers { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                // here we can seed with new villa record 
                new Villa()
                {
                    Id = 3,
                    Name = "Royal Villa",
                    Details = "Details of Royal Villa ",
                    ImageUrl = "",
                    Occupancy = 4,
                    Rate = 200,  
                    Sqft = 200,
                    Amenity= "",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Royal Villa 2",
                    Details = "Details of Royal Villa 2",
                    ImageUrl = "",
                    Occupancy = 4,
                    Rate = 200,
                    Sqft = 200,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                }
                );
        }
    }
}
