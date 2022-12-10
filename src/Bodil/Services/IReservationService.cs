using Bodil.Models;

namespace Bodil.Services
{
    public interface IReservationService
    {
        Task AddReservation(User user, DateTime start, DateTime end);
        Task DeleteReservation(Reservation reservation);
        Task GetReservationsAsync(DateTime start, DateTime end);
    }
}
