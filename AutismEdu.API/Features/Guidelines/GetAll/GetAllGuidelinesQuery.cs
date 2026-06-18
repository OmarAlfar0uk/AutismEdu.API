using MediatR;

namespace AutismEdu.API.Features.Guidelines.GetAll
{
    public class GetAllGuidelinesQuery : IRequest<GetAllGuidelinesResponse>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 6;

        public Guid? CurrentUserId { get; set; }

        public string? CurrentRole { get; set; }
    }

    public class GetAllGuidelinesResponse
    {
        public List<GuidelineDto> Guidelines { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
