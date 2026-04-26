using backend.Models;

namespace backend.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<List<Review>> GetByOwnerIdAsync(int ownerId);

    }
}
