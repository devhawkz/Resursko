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

    public async Task<List<GetResourcesDTO>> GetAllResources()
    {
        return await context.Resources
            .Select(r => new GetResourcesDTO(r.Id, r.Name!, r.Description!))
            .ToListAsync();
    }

    public async Task<ResourceResponse> UpdateResource(Resource resource, int id)
    {
        var dbResource = await context.Resources.FindAsync(id);
        if (dbResource is null)
            return new ResourceResponse(false, $"Resource with id: {id} doesn't exist");

        dbResource.Name = resource.Name;
        dbResource.Description = resource.Description;
        await context.SaveChangesAsync();

        return new ResourceResponse(true);

    }

    public async Task<ResourceResponse> DeleteResource(int id)
    {
        var dbResource = await context.Resources.FindAsync(id);
        if (dbResource is null)
            return new ResourceResponse(false, $"Resource with id: {id} doesn't exist");

        context.Resources.Remove(dbResource);
        await context.SaveChangesAsync();

        return new ResourceResponse(true);

    }

    private async Task<ResourceResponse> CheckNameInDatabase(string name)
    {
        var resource = await context.Resources.FirstOrDefaultAsync(r => r.Name == name);
        return resource is null ? new ResourceResponse(true) : new ResourceResponse(false, $"Resource with name {name} already exists in database");
    }

}    
