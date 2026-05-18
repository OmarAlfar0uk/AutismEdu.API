using System.Text.Json.Serialization;
using MediatR;

namespace AutismEdu.API.Features.Children.Add
{
    public class CreateChildCommand : IRequest<CreateChildResponseDto>
    {
        public Guid UserId { get; set; }
        public Guid? ParentId { get; set; }
        public string? EmailParent { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? Notes { get; set; }
        [JsonPropertyName("notes_diagnosis")]
        public string? NotesDiagnosis { get; set; }
    }

    public class CreateChildResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? Gender { get; set; }
        [JsonPropertyName("parent_id")]
        public Guid? ParentId { get; set; }
    }
}
