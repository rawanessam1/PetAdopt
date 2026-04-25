using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Pet
{
    public class UpdatePetDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string PetName { get; set; }
        [Required]
        [Range(0, 30, ErrorMessage = "Age must be between 0 and 30")]
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string HealthStatus { get; set; }
        public string Location { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}