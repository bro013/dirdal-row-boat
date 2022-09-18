using System.ComponentModel.DataAnnotations;

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
    }
}
