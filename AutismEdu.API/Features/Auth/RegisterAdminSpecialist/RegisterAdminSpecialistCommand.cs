using MediatR;

namespace AutismEdu.API.Features.Auth.RegisterAdminSpecialist
{
    public record RegisterAdminSpecialistCommand(RegisterAdminSpecialistDto RegisterDto) : IRequest<RegisterAdminSpecialistResponse>;
}
