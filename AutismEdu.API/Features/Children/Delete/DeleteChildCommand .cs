using MediatR;

namespace AutismEdu.API.Features.Children.Delete
{
    public class DeleteChildCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
