using backend.DTOs.Pet;
using backend.Models;
using backend.Models.Enums;
using backend.Repositories;

namespace backend.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _repo;

        public PetService(IPetRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Pet>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Pet> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Pet> CreateAsync(CreatePetDto dto, int ownerId)
        {
            var pet = new Pet
            {
                PetName = dto.PetName,
                Age = dto.Age,
                Type = dto.Type,
                Breed = dto.Breed,
                Gender = dto.Gender,
                Description = dto.Description,
                Location = dto.Location,
                HealthStatus = dto.HealthStatus,
                OwnerId = ownerId,
                Status = PetStatus.PendingApproval,
                Images = dto.ImageUrls.Select(url => new PetImage { Url = url }).ToList()
            };

            await _repo.AddAsync(pet);
            return pet;
        }

        public async Task<bool> UpdateAsync(int id, UpdatePetDto dto, int ownerId)
        {
            var pet = await _repo.GetByIdAsync(id);
            if (pet == null || pet.OwnerId != ownerId)
                return false;

            pet.PetName = dto.PetName;
            pet.Age = dto.Age;
            pet.Type = dto.Type;
            pet.Breed = dto.Breed;
            pet.Gender = dto.Gender;
            pet.Description = dto.Description;
            pet.Location = dto.Location;
            pet.HealthStatus = dto.HealthStatus;

            pet.Images.Clear();
            foreach (var url in dto.ImageUrls)
            {
                pet.Images.Add(new PetImage { Url = url });
            }

            await _repo.UpdateAsync(pet);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int ownerId)
        {
            var pet = await _repo.GetByIdAsync(id);
            if (pet == null || pet.OwnerId != ownerId)
                return false;

            await _repo.DeleteAsync(pet);
            return true;
        }

        public async Task<List<Pet>> SearchAsync(string? type, string? location, string? breed, int? age)
        {
            return await _repo.SearchAsync(type, location, breed, age);
        }

        public async Task<bool> RequestAdoptionAsync(int petId)
        {
            var pet = await _repo.GetByIdAsync(petId);

            if (pet == null || pet.Status != PetStatus.Available) return false;

            pet.Status = PetStatus.AdoptionPending;
            await _repo.UpdateAsync(pet);
            return true;
        }

        public async Task<bool> ApprovePetAsync(int petId)
        {
            var pet = await _repo.GetByIdAsync(petId);

            if (pet == null || pet.Status != PetStatus.PendingApproval)
                return false;

            pet.Status = PetStatus.Available;
            await _repo.UpdateAsync(pet);
            return true;
        }

        public async Task<bool> RejectPetAsync(int petId)
        {
            var pet = await _repo.GetByIdAsync(petId);

            if (pet == null || pet.Status != PetStatus.PendingApproval)
                return false;

            pet.Status = PetStatus.Rejected;
            await _repo.UpdateAsync(pet);
            return true;
        }
    }
}