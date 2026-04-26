using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Favorites
{
    public class AddFavoriteDto
    {
        [Required]
        public int PetId { get; set; }
    }
}
