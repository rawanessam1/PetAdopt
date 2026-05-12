using backend.DTOs.Requests;
using backend.Models;

namespace backend.Services
{
    public interface IRequestService
    {
        Task<bool> SendRequestAsync(CreateRequestDto dto, int adopterId);
        Task<List<RequestResponseDto>> GetRequestsForOwnerAsync(int ownerId);
        Task<List<RequestResponseDto>> GetRequestsForAdopterAsync(int adopterId);
        Task<List<RequestResponseDto>> GetAdopterHistoryAsync(int adopterId);
        Task<bool> AcceptRequestAsync(int requestId, int ownerId);
        Task<bool> RejectRequestAsync(int requestId, int ownerId);
        Task<adoptionRequest?> GetRequestByIdAsync(int id);
        Task<bool> HasAdoptedFromOwnerAsync(int adopterId, int ownerId);
    }
}