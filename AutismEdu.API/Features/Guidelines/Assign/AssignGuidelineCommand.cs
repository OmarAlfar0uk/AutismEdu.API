using System;
using MediatR;

namespace AutismEdu.API.Features.Guidelines.Assign
{
    public class AssignGuidelineCommand : IRequest<AssignGuidelineResult>
    {
        public Guid GuidelineId { get; set; }
        public Guid ChildId { get; set; }
        public Guid AssignedBy { get; set; }
    }

    public class AssignGuidelineResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
