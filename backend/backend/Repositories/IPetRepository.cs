using backend.Models;

namespace backend.Repositories
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        Task<List<Pet>> SearchAsync(string? type, string? location, string? breed, int? age);
        Task<List<Pet>> GetPendingPetsAsync();
        Task<List<Pet>> GetAvailablePetsSortedByAgeAsync();
    }
}