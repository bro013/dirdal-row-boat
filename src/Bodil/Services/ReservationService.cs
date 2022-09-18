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

        public List<Reservation> CurrentReservations { get; set; } = new();

        public ReservationService(ReservationContext db, ILogger<ReservationService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            try
            {
                if (reservation == null) return;
                _db.Reservations.Add(reservation);
                CurrentReservations.Add(reservation);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reservation to database");
                throw;
            }
        }

        public async Task RemoveRevervationAsync(Reservation reservation)
        {
            try
            {
                _db.Reservations.Remove(reservation);
                CurrentReservations.Remove(reservation);
                await _db.AddRangeAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error removing reservation");
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync(DateTime start, DateTime end)
        {
            try
            {
                return await _db.Reservations
                    .Where(res => res.Start >= start && res.End <= end)
                    .Include(res => res.User)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reservations from database");
                throw;
            }
        }


        public bool IsReservationAvailable(Guid userId, DateTime start, DateTime end) =>
            CurrentReservations.Find(r => r.Start >= start && r.End <= end && r.UserId == userId) is null;

        public bool HasRevervationsInInterval(DateTime start, DateTime end) =>
            CurrentReservations.Where(r => r.Start >= start && r.End <= end).Any();
    }
}
