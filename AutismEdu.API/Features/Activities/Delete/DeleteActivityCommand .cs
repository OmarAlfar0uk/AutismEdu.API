using MediatR;

namespace AutismEdu.API.Features.Activities.Delete
{
    public class DeleteActivityCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
