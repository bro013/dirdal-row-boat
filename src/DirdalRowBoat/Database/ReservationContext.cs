using DirdalRowBoat.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DirdalRowBoat.Database
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
