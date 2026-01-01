using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Entities;

public class Token : EntityBase
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? AssignedTo { get; set; }
}
