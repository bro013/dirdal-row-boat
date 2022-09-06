using DirdalRowBoat.Database;
using DirdalRowBoat.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DirdalRowBoat.Services
{
    public class ReservationService
    {
        private readonly ReservationContext _db;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ReservationContext db, ILogger<ReservationService> logger)
        {
            _db = db;
            _logger = logger;   
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetReservationsAsync(DateTime start, DateTime end)
        {
            return await _db.Reservations
                .Where(res => res.Start >= start && res.End <= end)
                .ToListAsync();
        }
    }
}
