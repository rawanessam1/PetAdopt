using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }

        public List<Pet> Pets { get; set; }
    }
}
