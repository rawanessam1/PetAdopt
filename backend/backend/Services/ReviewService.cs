using backend.DTOs.Reviews;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;

        public ReviewService(IReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        // Add Review
        public async Task<bool> AddReviewAsync(CreateReviewDto dto, int adopterId)
        {
            var review = new Review
            {
                AdopterId = adopterId,
                OwnerId = dto.OwnerId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.Now
            };

            await _reviewRepo.AddAsync(review);
            return true;
        }

        // Get Reviews
        public async Task<List<ReviewResponseDto>> GetReviewsAsync(int ownerId)
        {
            var reviews = await _reviewRepo.GetByOwnerIdAsync(ownerId);

            return reviews.Select(r => new ReviewResponseDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                AdopterName = r.Adopter.Name,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}
