namespace Resursko.Domain.DTOs.ResourceDTO;

public record class ResourceResponse(bool IsSuccessful, string? ErrorMessage = null);
