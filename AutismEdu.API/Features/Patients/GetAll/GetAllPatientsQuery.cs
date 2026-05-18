using System.Text.Json.Serialization;
using MediatR;

namespace AutismEdu.API.Features.Patients.GetAll
{
    public class GetAllPatientsQuery : IRequest<GetAllPatientsResponse>
    {
        public string? Status { get; set; } // all|in_progress|scheduled|needs_review
        public string? AgeRange { get; set; } // e.g., "5-10"
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        internal Guid? ParentUserId { get; set; }
        internal string? ParentEmail { get; set; }
        internal bool IsSpecialist { get; set; }
    }

    public class GetAllPatientsResponse
    {
        public List<PatientDto> Patients { get; set; } = new();
        public int Total { get; set; }
        [JsonPropertyName("active_count")]
        public int ActiveCount { get; set; }
        [JsonPropertyName("reviews_pending")]
        public int ReviewsPending { get; set; }
        [JsonPropertyName("sessions_today")]
        public int SessionsToday { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
