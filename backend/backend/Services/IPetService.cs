using backend.DTOs.Pet;
using backend.Models;

namespace backend.Services
{
    public interface IPetService
    {
        List<Pet> GetAll();
        Pet GetById(int id);
        Pet Create(CreatePetDto dto, int ownerId);
        bool Update(int id, UpdatePetDto dto, int ownerId);
        bool Delete(int id, int ownerId);
        List<Pet> Search(string? type, string? location,string?breed, int? age);
        bool RequestAdoption(int petId);
    }

}