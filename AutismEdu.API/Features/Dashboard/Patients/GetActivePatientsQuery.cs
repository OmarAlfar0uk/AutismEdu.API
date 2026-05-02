using MediatR;

namespace AutismEdu.API.Features.Dashboard.Patients
{
    public class GetActivePatientsQuery : IRequest<List<ActivePatientDto>>
    {
        public int Limit { get; set; } = 10;
    }
}
