using AutismEdu.API.Features.Children;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetChildrenByParentQuery : IRequest<IEnumerable<ChildDto>>
    {
        public Guid UserId { get; set; }   
    }
}
