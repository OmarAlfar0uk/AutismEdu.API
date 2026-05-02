using MediatR;

namespace AutismEdu.API.Features.Guidelines.Delete
{
    public class DeleteGuidelineCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
