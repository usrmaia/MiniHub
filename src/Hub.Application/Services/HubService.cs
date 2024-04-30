using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Filters;
using Hub.Domain.Repositories;

namespace Hub.Application.Services;

public class HubService : IHubService
{
    private readonly IHubRepository _hubRepository;

    public HubService(IHubRepository hubRepository) =>
        _hubRepository = hubRepository;

    public Task<Items> Query(ItemFilter filter) =>
        _hubRepository.Query(filter);
}
