using Hub.Domain.Entities;

namespace Hub.Domain.Repositories;

public interface IFlagRepository : IBaseRepository<Flag>
{
    Task<bool> ExistByName(string name);
}
