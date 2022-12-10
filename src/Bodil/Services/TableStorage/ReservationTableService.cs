using Azure.Data.Tables;
using Bodil.Models;
using System.Text.Json;

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
            var rowKey = end.ToString("yyyyMM");
            var resultPages = _tableClient.QueryAsync<Reservation>(filter: $"PartitionKey eq '{rowKey}'");
            await foreach (var reservation in resultPages) reservations.Add(reservation);
            return reservations;
        }
    }
}
