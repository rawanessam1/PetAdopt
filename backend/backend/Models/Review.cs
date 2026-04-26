using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int AdopterId { get; set; }
        public User Adopter { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public int Rating { get; set; } // 1 - 5
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
