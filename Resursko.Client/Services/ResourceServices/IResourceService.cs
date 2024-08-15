using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.Client.Services.ResourceServices;

public interface IResourceService
{
    Task<List<GetResourcesDTO>> GetAllResources();
    Task<ResourceResponse> CreateResource(ResourceRequest request);
    Task<ResourceResponse> UpdateResource(ResourceRequest request, int id);
    Task<ResourceResponse> DeleteResource(int id);
}
