using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AutismEdu.API.Features.Children.Add
{
    public class CreateChildHandler : IRequestHandler<CreateChildCommand, CreateChildResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateChildHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateChildResponseDto> Handle(CreateChildCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();
            var parentId = await ResolveParentIdAsync(request);
            var parentEmail = request.EmailParent;

            if (string.IsNullOrWhiteSpace(parentEmail))
            {
                var parent = await _userManager.FindByIdAsync(parentId.ToString());
                parentEmail = parent?.Email;
            }

            var child = new ChildProfile
            {
                UserId = parentId,
                Name = request.Name,
                Age = request.Age,
                Notes = request.NotesDiagnosis ?? request.Notes,
                ParentEmail = parentEmail
            };

            await repo.CreateAsync(child);
            await _unitOfWork.SaveChangesAsync();

            return new CreateChildResponseDto
            {
                Id = child.Id,
                Name = child.Name,
                Age = child.Age,
                Gender = request.Gender,
                ParentId = child.UserId
            };
        }

        private async Task<Guid> ResolveParentIdAsync(CreateChildCommand request)
        {
            if (request.ParentId.HasValue && request.ParentId.Value != Guid.Empty)
                return request.ParentId.Value;

            if (request.UserId != Guid.Empty)
                return request.UserId;

            if (!string.IsNullOrWhiteSpace(request.EmailParent))
            {
                var parent = await _userManager.FindByEmailAsync(request.EmailParent);
                if (parent is not null)
                    return parent.Id;
            }

            var user = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user?.IsInRole("parent") == true && Guid.TryParse(userIdClaim, out var parentId))
                return parentId;

            throw new BadHttpRequestException("Parent could not be resolved from parentId, emailParent, userId, or authenticated parent context.");
        }
    }
}
