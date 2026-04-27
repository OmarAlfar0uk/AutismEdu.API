using MediatR;

namespace AutismEdu.API.Features.Children.Add
{
    public class CreateChildCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }   
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? Notes { get; set; }
    }
}
