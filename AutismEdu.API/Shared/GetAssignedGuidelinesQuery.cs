using System;
using System.Collections.Generic;
using AutismEdu.API.Features.Guidelines;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetAssignedGuidelinesQuery(Guid ChildId, string BaseUrl) : IRequest<List<AssignedGuidelineDto>>;
}
