namespace backend.DTOs.Pet
{
    public class UpdatePetDto
    {
        public string PetName { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }

        public string HealthStatus { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
    }
}