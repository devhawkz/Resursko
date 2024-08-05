using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.DTOs.ResourceDTO;
public class ResourceRequest
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }
}
