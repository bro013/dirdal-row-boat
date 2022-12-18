using Bodil.Models;
using Bodil.Shared;
using MudBlazor;

namespace Bodil.Services
{
    public class ReservationService : IReservationService
    {
        private readonly List<Reservation> _reservations = new();
        private readonly IReservationDataService _reservationDataService;
        private readonly IDialogService _dialogService;

        public ReservationService(IReservationDataService reservationData, IDialogService dialogService)
        {
            _reservationDataService = reservationData;
            _dialogService = dialogService;
        }

        public async Task<List<Reservation>> GetReservationsAsync(DateTime start, DateTime end)
        {
            if (!HasRevervationsInInterval(start, end))
            {
                var reservations = await _reservationDataService.GetReservationsAsync(start, end);
                var newReservations = reservations.Except(_reservations).ToList();
                _reservations.AddRange(newReservations);
            }
            return _reservations;
        }

        public async Task AddReservation(AppUser user, DateTime start, DateTime end)
        {
            if (IsReservationAvailable(user.Id, start, end))
                await InsertReservationAsync(user, start, end);
            else
                await PromtDeleteRevervationAsync(user, start, end);
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            await _reservationDataService.DeleteRevervationAsync(reservation);
            _reservations.Remove(reservation);
        }

        private async Task PromtDeleteRevervationAsync(AppUser user, DateTime start, DateTime end)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = _dialogService.Show<ReservationDialog>("Revervasjon", options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var reservation = FindReservation(user.Id, start, end);
                if (reservation is not null)
                {
                    await DeleteReservation(reservation);
                }
            }
        }

        private async Task InsertReservationAsync(AppUser user, DateTime start, DateTime end)
        {
            var reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                Title = user.FullName,
                Start = start,
                End = end,
                UserId = user.Id,
            };
            await _reservationDataService.AddReservationAsync(reservation);
            _reservations.Add(reservation);
        }

        Reservation? FindReservation(Guid userId, DateTime start, DateTime end) =>
            _reservations.Find(r => r.Start >= start && r.End <= end && r.UserId == userId);

        bool IsReservationAvailable(Guid userId, DateTime start, DateTime end) =>
            FindReservation(userId, start, end) is null;

        bool HasRevervationsInInterval(DateTime start, DateTime end) =>
            _reservations.Where(r => r.Start >= start && r.End <= end).Any();

    }
}
