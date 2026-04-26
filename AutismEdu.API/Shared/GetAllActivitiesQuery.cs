using AutismEdu.API.Features.Activities;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetAllActivitiesQuery : IRequest<IEnumerable<ActivityDto>>
    {
    }
}
