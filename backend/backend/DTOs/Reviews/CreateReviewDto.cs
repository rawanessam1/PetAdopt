using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Reviews
{
    public class CreateReviewDto
    {
        [Required]
        public int OwnerId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Comment { get; set; }
    }
}
