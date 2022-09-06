using System.ComponentModel.DataAnnotations;

namespace DirdalRowBoat.Database.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Color { get; set; }
    }
}
