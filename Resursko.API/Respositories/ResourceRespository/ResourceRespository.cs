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

    public async Task<List<Resource>> GetAllResources() => await context.Resources.ToListAsync();

    public async Task<ResourceResponse> UpdateResource(Resource resource, int id)
    {
        var oldResource = await context.Resources.FindAsync(id);
        if (oldResource is null)
            return new ResourceResponse(false, $"Resource with id: {id} doesn't exist");

        oldResource.Name = resource.Name;
        oldResource.Description = resource.Description;
        await context.SaveChangesAsync();

        return new ResourceResponse(true);

    }

    private async Task<ResourceResponse> CheckNameInDatabase(string name)
    {
        var resource = await context.Resources.FirstOrDefaultAsync(r => r.Name == name);
        return resource is null? new ResourceResponse(true) : new ResourceResponse(false, $"Resource with name {name} already exists in database");
    }
}
