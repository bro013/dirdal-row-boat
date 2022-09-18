using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bodil.Database.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string RevervationColor { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
