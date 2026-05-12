namespace backend.DTOs.Pet
{
    public class PetResponseDto
    {
        public int Id { get; set; }
        public string PetName { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string HealthStatus { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string OwnerName { get; set; }
        public int OwnerId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}