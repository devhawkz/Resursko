using Mapster;
using Resursko.API.Respositories.ResourceRespository;
using Resursko.Domain.DTOs.ResourceDTO;
using System.ComponentModel.Design;

namespace Resursko.API.Services.ResourceService;

public class ServiceResource(IResourceRespository resourceRespository) : IServiceResoruce
{
    public async Task<ResourceResponse> CreateResource(ResourceRequest request)
    {
        var resource = request.Adapt<Resource>();
        var result = await resourceRespository.CreateResource(resource);

        return result;
    }

    public async Task<List<Resource>> GetAllResources()
    {
        var result = await resourceRespository.GetAllResources();
        if (result.Count == 0)
            return new List<Resource>();

        return result;
    }

    public async Task<ResourceResponse> UpdateResource(ResourceRequest request, int id)
    {
        var resource = request.Adapt<Resource>();
        var result = await resourceRespository.UpdateResource(resource, id);

        return result;
    }

    public async Task<ResourceResponse> DeleteResource(int id)
    {
        var result = await resourceRespository.DeleteResource(id);
        return result;
    }
}
