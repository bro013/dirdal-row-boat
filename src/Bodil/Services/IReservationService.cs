using Bodil.Database.Models;

namespace Bodil.Services
{
    public interface IReservationService
    {
        Task AddReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetReservationsAsync(DateTime start, DateTime end);
        Task DeleteRevervationAsync(Reservation reservation);
    }
}
