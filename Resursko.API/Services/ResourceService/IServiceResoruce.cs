using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.API.Services.ResourceService;

public interface IServiceResoruce
{
    Task<ResourceResponse> CreateResource(CreateResourceRequest request);
}
