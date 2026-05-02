using MediatR;

namespace AutismEdu.API.Features.Patients.Create
{
    public class CreatePatientCommand : IRequest<CreatePatientResponse>
    {
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string FocusArea { get; set; } = default!;
        public List<string>? SkillTags { get; set; }
        public string? EmailParent { get; set; }
    }

    public class CreatePatientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? EmailParent { get; set; }
        public string Message { get; set; } = default!;
    }
}
