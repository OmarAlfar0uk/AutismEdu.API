using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace AutismEdu.API.Features.Patients.Create
{
    public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, CreatePatientResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailKitEmailService _emailService;

        public CreatePatientHandler(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor, IMailKitEmailService emailService)
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<CreatePatientResponse> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var specialistId))
            {
                throw new UnauthorizedAccessException("Specialist ID not found in token.");
            }

            var childProfile = new ChildProfile
            {
                Name = request.Name,
                Age = request.Age,
                UserId = specialistId, // Specialist is the owner in this context, or it could be unassigned parent
                FocusArea = request.FocusArea,
                SkillTags = request.SkillTags != null ? JsonSerializer.Serialize(request.SkillTags) : "[]",
                Status = PatientStatus.InProgress,
                ParentEmail = request.EmailParent
            };

            var repo = _uow.GetRepository<ChildProfile>();
            await repo.CreateAsync(childProfile);
            await _uow.SaveChangesAsync();

            // Send invite email if email is provided
            string message = "Patient created successfully.";
            if (!string.IsNullOrEmpty(request.EmailParent))
            {
                try
                {
                    string subject = "Invitation to join AutismEdu";
                    string body = $"Hello,\n\nYou have been invited to join AutismEdu to track {request.Name}'s progress. Please register on our platform.";
                    await _emailService.SendEmailAsync(request.EmailParent, subject, body);
                    message = "Patient created successfully and invitation email sent.";
                }
                catch (Exception)
                {
                    // Log email failure, but patient is created
                    message = "Patient created successfully, but failed to send invitation email.";
                }
            }

            return new CreatePatientResponse
            {
                Id = childProfile.Id,
                Name = childProfile.Name,
                EmailParent = childProfile.ParentEmail,
                Message = message
            };
        }
    }
}
