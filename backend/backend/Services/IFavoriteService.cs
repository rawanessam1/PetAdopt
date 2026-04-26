using backend.DTOs.Favorites;

namespace backend.Services
{
    public interface IFavoriteService
    {
        Task<bool> AddFavoriteAsync(AddFavoriteDto dto, int userId);
        Task<bool> RemoveFavoriteAsync(int petId, int userId);
        Task<List<FavoriteResponseDto>> GetFavoritesAsync(int userId);
    }
}
