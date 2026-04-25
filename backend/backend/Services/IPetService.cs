using backend.DTOs.Pet;
using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IPetService
    {
        Task<List<Pet>> GetAllAsync();
        Task<Pet> GetByIdAsync(int id);
        Task<Pet> CreateAsync(CreatePetDto dto, int ownerId);
        Task<bool> UpdateAsync(int id, UpdatePetDto dto, int ownerId);
        Task<bool> DeleteAsync(int id, int ownerId);
        Task<List<Pet>> SearchAsync(string? type, string? location, string? breed, int? age);
        Task<bool> RequestAdoptionAsync(int petId);

        // ADMIN-ONLY METHODS
        Task<bool> ApprovePetAsync(int petId);
        Task<bool> RejectPetAsync(int petId);
    }
}