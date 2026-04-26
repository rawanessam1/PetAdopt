using backend.Models.Enums;
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
        public UserRole Role { get; set; }
        public bool IsApproved { get; set; }

        public List<Pet> Pets { get; set; }

        public List<adoptionRequest> adoptionRequests { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
