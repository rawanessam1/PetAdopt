using backend.Models.Enums;

namespace backend.DTOs.Requests
{
    public class RequestResponseDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
