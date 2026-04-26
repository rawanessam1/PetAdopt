using backend.DTOs.Requests;

namespace backend.Services
{
    public interface IRequestService
    {
        Task<bool> SendRequestAsync(CreateRequestDto dto, int adopterId);
        Task<bool> AcceptRequestAsync(int requestId, int ownerId);
        Task<bool> RejectRequestAsync(int requestId, int ownerId);
        Task<List<RequestResponseDto>> GetRequestsForOwnerAsync(int ownerId);
    }
}
