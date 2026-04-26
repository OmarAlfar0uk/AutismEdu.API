using AutismEdu.API.Features.Children;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetChildByIdQuery : IRequest<ChildDto?>
    {
        public Guid Id { get; set; }
    }
}
