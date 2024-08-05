using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.API.Respositories.ResourceRespository;

public interface IResourceRespository
{
    Task<ResourceResponse> CreateResource(Resource resource);
    Task<List<Resource>> GetAllResources();

    Task<ResourceResponse> UpdateResource(Resource resource, int id);

}
