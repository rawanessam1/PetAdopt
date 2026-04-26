using backend.DTOs.Favorites;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favRepo;
        private readonly IPetRepository _petRepo;

        public FavoriteService(IFavoriteRepository favRepo, IPetRepository petRepo)
        {
            _favRepo = favRepo;
            _petRepo = petRepo;
        }

        // Add Favorite
        public async Task<bool> AddFavoriteAsync(AddFavoriteDto dto, int userId)
        {
            var pet = await _petRepo.GetByIdAsync(dto.PetId);
            if (pet == null) return false;

            var existing = await _favRepo.GetByUserAndPetAsync(userId, dto.PetId);
            if (existing != null) return false; // already added

            var favorite = new Favorite
            {
                UserId = userId,
                PetId = dto.PetId
            };

            await _favRepo.AddAsync(favorite);
            return true;
        }

        // Remove Favorite
        public async Task<bool> RemoveFavoriteAsync(int petId, int userId)
        {
            var favorite = await _favRepo.GetByUserAndPetAsync(userId, petId);
            if (favorite == null) return false;

            await _favRepo.DeleteAsync(favorite);
            return true;
        }

        // Get Favorites
        public async Task<List<FavoriteResponseDto>> GetFavoritesAsync(int userId)
        {
            var favorites = await _favRepo.GetByUserIdAsync(userId);

            return favorites.Select(f => new FavoriteResponseDto
            {
                PetId = f.PetId,
                PetName = f.Pet.PetName,
                Breed = f.Pet.Breed,
                Location = f.Pet.Location
            }).ToList();
        }
    }
}
