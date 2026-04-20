using Microsoft.Extensions.Diagnostics.HealthChecks;
using backend.DTOs.Pet;
using backend.Models;
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

        public List<Pet> GetAll()
        {
            return _repo.GetAll();
        }

        public Pet GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Pet Create(CreatePetDto dto, int ownerId)
        {
            var pet = new Pet
            {
                PetName = dto.PetName,
                Age = dto.Age,
                Type = dto.Type,
                Gender = dto.Gender,
                Description = dto.Description,
                Location = dto.Location,
                ImageUrl = dto.ImageUrl,
                HealthStatus = dto.HealthStatus, 
                OwnerId = ownerId,
                Status = "Pending"
            };

            _repo.Add(pet);
            return pet;
        }

        public bool Update(int id, UpdatePetDto dto, int ownerId)
        {
            var pet = _repo.GetById(id);
            if (pet == null || pet.OwnerId != ownerId)
                return false;

            pet.PetName = dto.PetName;
            pet.Age = dto.Age;
            pet.Type = dto.Type;
            pet.Gender = dto.Gender;
            pet.Description = dto.Description;
            pet.Location = dto.Location;
            pet.ImageUrl = dto.ImageUrl;
            pet.HealthStatus = dto.HealthStatus;

            _repo.Update(pet);
            return true;
        }

        public bool Delete(int id, int ownerId)
        {
            var pet = _repo.GetById(id);
            if (pet == null || pet.OwnerId != ownerId)
                return false;

            _repo.Delete(pet);
            return true;
        }
        public List<Pet> Search(string? type, string? location,string?breed,int? age)
        {
            return _repo.Search(type, location,breed, age);
        }
        public bool RequestAdoption(int petId)
        {
            var pet = _repo.GetById(petId);
            if (pet == null || pet.Status != "Available") return false;

            pet.Status = "Pending"; // Change status as per requirements
            _repo.Update(pet);
            return true;
        }
    }
}