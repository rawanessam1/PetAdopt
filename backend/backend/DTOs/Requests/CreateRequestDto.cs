using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Requests
{
    public class CreateRequestDto
    {
        [Required]
        public int PetId { get; set; }
        public string? AdoptionHistory { get; set; } 
    }
    
}
