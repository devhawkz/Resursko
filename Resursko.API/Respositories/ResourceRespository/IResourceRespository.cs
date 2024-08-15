using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.API.Respositories.ResourceRespository;

public interface IResourceRespository
{
    Task<ResourceResponse> CreateResource(Resource resource);
    Task<List<GetResourcesDTO>> GetAllResources();
    Task<ResourceResponse> UpdateResource(Resource resource, int id);

    Task<ResourceResponse> DeleteResource(int id);

}
