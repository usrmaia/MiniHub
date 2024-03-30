using Hub.Domain.Entities;

namespace Hub.Domain.Repositories;

public interface IDirectoryRepository : IBaseRepository<DirectoryE>
{
    Task<bool> ExistsByNameAndParentId(string name, string? parentId);
    Task<List<string>> GetChildrensNameById(string id);
    Task<List<string>> GetParentsNameById(string id);
}
