using Bodil.Database.Models;
using Bodil.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bodil.Services
{
    public class ReservationService
    {
        private readonly IDbContextFactory<ReservationContext> _dbContextFactory;
        private readonly ILogger<ReservationService> _logger;

        public List<Reservation> Reservations { get; private set; } = new();

        public ReservationService(IDbContextFactory<ReservationContext> contextFactory, ILogger<ReservationService> logger)
        {
            _dbContextFactory = contextFactory;
            _logger = logger;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            try
            {
                if (reservation == null) return;
                using var context = await _dbContextFactory.CreateDbContextAsync();
                context.Reservations.Add(reservation);
                Reservations.Add(reservation);
                await context.SaveChangesAsync();
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
                using var context = await _dbContextFactory.CreateDbContextAsync();
                context.Reservations.Remove(reservation);
                Reservations.Remove(reservation);
                await context.AddRangeAsync();
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
                using var context = await _dbContextFactory.CreateDbContextAsync();
                return await context.Reservations
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

        public async Task RequestNewReservationsAsync(DateTime start, DateTime end)
        {
            if (!HasRevervationsInInterval(start, end))
            {
                var resevations = await GetReservationsAsync(start, end);
                Reservations.AddRange(resevations);
            }
        }

        public Reservation? FindReservation(Guid userId, DateTime start, DateTime end) =>
            Reservations.Find(r => r.Start >= start && r.End <= end && r.UserId == userId);

        public bool IsReservationAvailable(Guid userId, DateTime start, DateTime end) =>
            FindReservation(userId, start, end) is null;

        private bool HasRevervationsInInterval(DateTime start, DateTime end) =>
            Reservations.Where(r => r.Start >= start && r.End <= end).Any();
    }
}
