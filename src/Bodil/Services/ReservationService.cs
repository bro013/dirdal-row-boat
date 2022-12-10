using Bodil.Database.Models;
using Bodil.Shared;
using MudBlazor;

namespace Bodil.Services
{
    public class ReservationService : IReservationService
    {
        public List<Reservation> Reservations { get; private set; } = new List<Reservation>();

        private readonly IReservationDataService _reservationDataService;
        private readonly IDialogService _dialogService;

        public ReservationService(IReservationDataService reservationData, IDialogService dialogService)
        {
            _reservationDataService = reservationData;
            _dialogService = dialogService;
        }

        public async Task GetReservationsAsync(DateTime start, DateTime end)
        {
            if (!HasRevervationsInInterval(start, end))
            {
                var reservations = await _reservationDataService.GetReservationsAsync(start, end);
                var newReservations = Reservations.Except(reservations);
                Reservations.AddRange(newReservations);
            }
        }

        public async Task AddReservation(User user, DateTime start, DateTime end)
        {
            if (IsReservationAvailable(user.Id, start, end))
                await InsertReservationAsync(user, start, end);
            else
                await PromtDeleteRevervationAsync(user, start, end);
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            await _reservationDataService.DeleteRevervationAsync(reservation);
            Reservations.Remove(reservation);
        }

        private async Task PromtDeleteRevervationAsync(User user, DateTime start, DateTime end)
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

        private async Task InsertReservationAsync(User user, DateTime start, DateTime end)
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
            Reservations.Add(reservation);
        }

        Reservation? FindReservation(Guid userId, DateTime start, DateTime end) =>
            Reservations.Find(r => r.Start >= start && r.End <= end && r.UserId == userId);

        bool IsReservationAvailable(Guid userId, DateTime start, DateTime end) =>
            FindReservation(userId, start, end) is null;

        bool HasRevervationsInInterval(DateTime start, DateTime end) =>
            Reservations.Where(r => r.Start >= start && r.End <= end).Any();

    }
}
