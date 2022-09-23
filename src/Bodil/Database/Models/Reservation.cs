using System.ComponentModel.DataAnnotations;

namespace Bodil.Database.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var reservation = obj as Reservation;
            return reservation.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
