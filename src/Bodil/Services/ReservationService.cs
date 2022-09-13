using Bodil.Database.Models;
using Bodil.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bodil.Services
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
            try
            {
                _db.Reservations.Add(reservation);
                await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error adding reservation to database.\n{ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<List<Reservation>> GetReservationsAsync(DateTime start, DateTime end)
        {
            try
            {
                return await _db.Reservations
                    .Where(res => res.Start >= start && res.End <= end)
                    .ToListAsync();

            }
            catch(Exception ex)
            {
                _logger.LogError("Error fetching reservations from database.\n{ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}
