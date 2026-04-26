using backend.Models;

namespace backend.Repositories
{
    public interface IRequestRepository : IGenericRepository<adoptionRequest>
    {
        Task<List<adoptionRequest>> GetByOwnerIdAsync(int ownerId);
        Task<List<adoptionRequest>> GetByPetIdAsync(int petId);
    }
}
