using MediatR;

namespace AutismEdu.API.Features.Children.Update
{
    public class UpdateChildCommand : IRequest
    {
        public Guid Id { get; set; }       
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? Notes { get; set; }
    }
}
