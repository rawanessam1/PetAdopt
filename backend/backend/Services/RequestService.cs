using backend.DTOs.Requests;
using backend.Repositories;
using backend.Models;
using backend.Models.Enums;

namespace backend.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepo;
        private readonly IPetRepository _petRepo;

        public RequestService(IRequestRepository requestRepo, IPetRepository petRepo)
        {
            _requestRepo = requestRepo;
            _petRepo = petRepo;
        }

        // Send Request
        public async Task<bool> SendRequestAsync(CreateRequestDto dto, int adopterId)
        {
            var pet = await _petRepo.GetByIdAsync(dto.PetId);

            if (pet == null || pet.Status != PetStatus.Available)
                return false;

            var request = new adoptionRequest
            {
                PetId = dto.PetId,
                AdopterId = adopterId,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.Now
            };

            await _requestRepo.AddAsync(request);
            return true;
        }

        // Accept Request
        public async Task<bool> AcceptRequestAsync(int requestId, int ownerId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null) return false;

            var pet = await _petRepo.GetByIdAsync(request.PetId);

            if (pet == null || pet.OwnerId != ownerId)
                return false;

            //Accept the request
            request.Status = RequestStatus.Accepted;

            // Change pet status
            pet.Status = PetStatus.Adopted;

            await _petRepo.UpdateAsync(pet);
            await _requestRepo.UpdateAsync(request);

            return true;
        }

        // Reject Request
        public async Task<bool> RejectRequestAsync(int requestId, int ownerId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null) return false;

            var pet = await _petRepo.GetByIdAsync(request.PetId);

            if (pet == null || pet.OwnerId != ownerId)
                return false;

            request.Status = RequestStatus.Rejected;

            await _requestRepo.UpdateAsync(request);
            return true;
        }

        // Get Requests for Owner
        public async Task<List<RequestResponseDto>> GetRequestsForOwnerAsync(int ownerId)
        {
            var requests = await _requestRepo.GetByOwnerIdAsync(ownerId);

            return requests.Select(r => new RequestResponseDto
            {
                Id = r.Id,
                PetId = r.PetId,
                PetName = r.Pet.PetName,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

    }
}
