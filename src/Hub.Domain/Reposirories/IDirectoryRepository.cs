using Hub.Domain.Entities;
using Hub.Domain.Filters;

namespace Hub.Domain.Repositories;

public interface IDirectoryRepository : IBaseRepository<DirectoryE>
{
    Task<QueryResult<DirectoryE>> Query(DirectoryFilter filter);
    Task<bool> ExistsByNameAndParentId(string name, string? parentId);
    Task<List<string>> GetChildrensNameById(string id);
    Task<List<string>> GetParentsNameById(string id);
}
