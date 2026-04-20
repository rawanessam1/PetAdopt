using backend.Data;
using backend.Models;

namespace backend.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly AppDbContext _context;

        public PetRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Pet> GetAll()
        {
            return _context.Pets.ToList();
        }

        public Pet GetById(int id)
        {
            return _context.Pets.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Pet pet)
        {
            _context.Pets.Add(pet);
            _context.SaveChanges();
        }

        public void Update(Pet pet)
        {
            _context.Pets.Update(pet);
            _context.SaveChanges();
        }

        public void Delete(Pet pet)
        {
            _context.Pets.Remove(pet);
            _context.SaveChanges();
        }
        public List<Pet> Search(string? type, string? location,string? breed, int? age)
        {
            var query = _context.Pets.AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type.ToLower() == type.ToLower());

            if (!string.IsNullOrEmpty(location))
                query = query.Where(p => p.Location.ToLower() == location.ToLower());

            if (!string.IsNullOrEmpty(breed))
                query = query.Where(p => p.Breed.ToLower().Contains(breed.ToLower()));

            if (age.HasValue)
                query = query.Where(p => p.Age == age);

            return query.ToList();
        }

    }
}