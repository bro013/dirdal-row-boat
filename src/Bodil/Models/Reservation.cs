using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace Bodil.Models
{
    public class Reservation : ITableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string PartitionKey
        {
            get => Start.ToString("yyyyMM");
            set { }
        }
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; } = ETag.All;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var reservation = obj as Reservation;
            return reservation.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
