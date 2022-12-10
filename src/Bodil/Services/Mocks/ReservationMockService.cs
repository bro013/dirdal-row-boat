using Bodil.Database.Models;

namespace Bodil.Services.Mocks
{
    public class ReservationMockService : IReservationService
    {
        private List<Reservation> _reservations = new();

        public Task AddReservationAsync(Reservation reservation)
        {
            _reservations.Add(reservation);
            return Task.CompletedTask;
        }

        public Task DeleteRevervationAsync(Reservation reservation)
        {
            _reservations.Remove(reservation);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Reservation>> GetReservationsAsync(DateTime start, DateTime end)
        {
            return Task.FromResult(_reservations.AsEnumerable());
        }
    }
}
