using backend.Models;

namespace backend.Repositories
{
    public interface IPetRepository
    {
        List<Pet> GetAll();
        Pet GetById(int id);
        void Add(Pet pet);
        void Update(Pet pet);
        void Delete(Pet pet);
        List<Pet> Search(string? type, string? location, string? breed, int? age);
    }
}