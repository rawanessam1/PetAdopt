using backend.DTOs.Reviews;

namespace backend.Services
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(CreateReviewDto dto, int adopterId);
        Task<List<ReviewResponseDto>> GetReviewsAsync(int ownerId);
    }
}
