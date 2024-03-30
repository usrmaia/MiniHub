using Hub.Domain.Entities;

namespace Hub.Domain.Repositories;

public interface IFileFlagRepository : IBaseRepository<FileFlag>
{
    Task<bool> ExistsByFileAndFlag(string fileId, string flagId);
}
