using Resursko.Domain.DTOs.ResourceDTO;
using System.Runtime.InteropServices;

namespace Resursko.API.Respositories.ResourceRespository;

public class ResourceRespository(DataContext context) : IResourceRespository
{
    public async Task<ResourceResponse> CreateResource(Resource resource)
    {
        var result = await CheckNameInDatabase(resource.Name!);
        if (!result.IsSuccessful)
            return result;

        context.Resources.Add(resource);
        await context.SaveChangesAsync();
        return result;
    }

    private async Task<ResourceResponse> CheckNameInDatabase(string name)
    {
        var resource = await context.Resources.FirstOrDefaultAsync(r => r.Name == name);
        return resource is null? new ResourceResponse(true) : new ResourceResponse(false, $"Resource with name {name} already exists in database");
    }
}
