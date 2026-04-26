namespace backend.DTOs.Reviews
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string AdopterName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
