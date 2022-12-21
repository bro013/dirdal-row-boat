using Bodil.Database;
using Bodil.Models;
using Microsoft.EntityFrameworkCore;

namespace Bodil.Services.Database
{
    public class ReservationDbService : IReservationDataService
    {
        private readonly IDbContextFactory<ReservationContext> _dbContextFactory;
        private readonly ILogger<ReservationDbService> _logger;

        public ReservationDbService(IDbContextFactory<ReservationContext> contextFactory, ILogger<ReservationDbService> logger)
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
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reservation to database");
                throw;
            }
        }

        public async Task DeleteRevervationAsync(Reservation reservation)
        {
            try
            {
                if (reservation == null) return;
                using var context = await _dbContextFactory.CreateDbContextAsync();
                context.Reservations.Remove(reservation);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
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
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reservations from database");
                throw;
            }
        }
    }
}
