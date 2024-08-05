using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.API.Respositories.ResourceRespository;

public interface IResourceRespository
{
    Task<ResourceResponse> CreateResource(Resource resource);

}
