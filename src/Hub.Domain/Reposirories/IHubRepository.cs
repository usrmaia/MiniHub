using Hub.Domain.DTOs;
using Hub.Domain.Filters;

namespace Hub.Domain.Repositories;

public interface IHubRepository
{
    Task<Items> Query(ItemFilter filter);
}
