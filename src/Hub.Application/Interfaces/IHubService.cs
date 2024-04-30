using Hub.Domain.DTOs;
using Hub.Domain.Filters;

namespace Hub.Application.Interfaces;

public interface IHubService
{
    Task<Items> Query(ItemFilter filter);
}
