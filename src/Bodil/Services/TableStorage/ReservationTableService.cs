using Azure.Data.Tables;
using Bodil.Models;

namespace Bodil.Services.TableStorage
{
    public class ReservationTableService : IReservationDataService
    {
        private readonly TableClient _tableClient;

        public ReservationTableService(TableClientFactory tableClientFactory)
        {
            _tableClient = tableClientFactory.GetTableClient("reservations");
            _tableClient.CreateIfNotExists();
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            reservation.Start = DateTime.SpecifyKind(reservation.Start, DateTimeKind.Utc);
            reservation.End = DateTime.SpecifyKind(reservation.End, DateTimeKind.Utc);
            await _tableClient.AddEntityAsync(reservation);
        }

        public async Task DeleteRevervationAsync(Reservation reservation) =>
            await _tableClient.DeleteEntityAsync(reservation.PartitionKey, reservation.RowKey);

        public async Task<IEnumerable<Reservation>> GetReservationsAsync(DateTime start, DateTime end)
        {
            var reservations = new List<Reservation>();
            var reservationTasks = new List<Task<IEnumerable<Reservation>>>();
            for (var dt = start; dt <= end; dt = dt.AddMonths(1))
                reservationTasks.Add(GetReservationsAsync(dt));
            var results = await Task.WhenAll(reservationTasks);
            return results?.SelectMany(result => result) ?? Enumerable.Empty<Reservation>();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync(DateTime dt)
        {
            var reservations = new List<Reservation>();
            var rowKey = dt.ToString("yyyyMM");
            var resultPages = _tableClient.QueryAsync<Reservation>(filter: $"PartitionKey eq '{rowKey}'");
            await foreach (var reservation in resultPages)
                reservations.Add(reservation);
            return reservations;
        }
    }
}
