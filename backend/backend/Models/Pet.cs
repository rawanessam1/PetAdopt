using backend.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PetName { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public List<PetImage> Images { get; set; } = new List<PetImage>();
        public PetStatus Status { get; set; } = PetStatus.PendingApproval;
        public string HealthStatus { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; } 

        public User user { get; set; }

        public List<adoptionRequest> adoptionRequests { get; set; }
        public List<Favorite> Favorites { get; set; }
    }
}