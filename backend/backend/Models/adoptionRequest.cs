using backend.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class adoptionRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public Pet Pet { get; set; }

        [ForeignKey("User")]
        public int AdopterId { get; set; }
        public User Adopter { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        // Pending - Accepted - Rejected

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
