using Bodil.Models;
using Microsoft.EntityFrameworkCore;

namespace Bodil.Database
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = Guid.Parse("e38f3987-e112-4dc7-a024-9322855ddee1"),
                FirstName = "Bjørn",
                LastName = "Rosland",
                Email = "bjoernrosland@gmail.com",
                PhoneNumber = "+4799247917",
                Color = "aqua"
            });
        }
    }
}
