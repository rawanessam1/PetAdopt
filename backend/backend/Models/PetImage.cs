using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class PetImage
    {
        public int Id { get; set; }
        public string Url { get; set; }

        [ForeignKey("Pet")]
        public int PetId { get; set; }

        public Pet Pet { get; set; } 
    }
}