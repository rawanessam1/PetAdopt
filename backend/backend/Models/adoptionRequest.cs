using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class adoptionRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }
        public string AdopterId { get; set; }
        public string Status { get; set; } // Pending, Accepted, Rejected
        public DateTime CreatedAt { get; set; }
    }
}
