using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.API.Services.ResourceService;

public interface IServiceResoruce
{
    Task<ResourceResponse> CreateResource(ResourceRequest request);
    Task<List<Resource>> GetAllResources();

    Task<ResourceResponse> UpdateResource(ResourceRequest request, int id);

    Task<ResourceResponse> DeleteResource(int id);
}
