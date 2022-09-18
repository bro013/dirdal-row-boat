using System.ComponentModel.DataAnnotations;

namespace Bodil.Database.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RevervationColor { get; set; }
    }
}
