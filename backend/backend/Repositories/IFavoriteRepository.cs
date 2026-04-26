using backend.Models;

namespace backend.Repositories
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        Task<List<Favorite>> GetByUserIdAsync(int userId);
        Task<Favorite> GetByUserAndPetAsync(int userId, int petId);
    }
}
