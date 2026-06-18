using AutismEdu.API.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AutismEdu.API.Features.CommunicationCard.Create
{
    public class CreateCommunicationCardHandler : IRequestHandler<CreateCommunicationCardCommand, Guid>
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageHelper _imageHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCommunicationCardHandler(IUnitOfWork uow, IImageHelper imageHelper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _imageHelper = imageHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(CreateCommunicationCardCommand request, CancellationToken cancellationToken)
        {
            // Get current user ID from JWT
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
            Guid.TryParse(userIdStr, out var currentUserId);

            // Save image using ImageHelper
            string relativePath = await _imageHelper.SaveImageAsync(
                request.Image,
                subFolder: "CommunicationCards"
            );

            // Create entity with ownership tracking
            var card = new Models.CommunicationCard
            {
                Title = request.Title,
                Category = request.Category,
                ImageUrl = relativePath,
                CreatedBy = currentUserId != Guid.Empty ? currentUserId : null,
                SpecialistId = currentUserId != Guid.Empty ? currentUserId : null
            };

            await _uow.GetRepository<Models.CommunicationCard>().CreateAsync(card);
            await _uow.SaveChangesAsync();

            return card.Id;
        }
    }
}