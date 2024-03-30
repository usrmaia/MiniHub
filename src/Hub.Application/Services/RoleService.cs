using Hub.Domain.DTOs;
using Hub.Domain.Exceptions;
using Hub.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Hub.Domain.Repositories;
using System.Net;

namespace Hub.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public Task<List<RoleDTO>> Get()
    {
        throw new NotImplementedException();
    }

    public Task<RoleDTO> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityRole> Create(IdentityRole role)
    {
        if (await _roleRepository.ExistsByName(role.Name!))
            throw new AppException("Função já existe", HttpStatusCode.Conflict);

        return await _roleRepository.Add(role);
    }

    public async Task<IdentityRole> Update(IdentityRole role)
    {
        if (_roleRepository.IsDefault(role.Name!))
            throw new AppException("Não é possível alterar uma função padrão", HttpStatusCode.BadRequest);

        if (!await _roleRepository.ExistsById(role.Id))
            throw new AppException("Função não encontrada", HttpStatusCode.NotFound);

        if (await _roleRepository.ExistsByName(role.Name!))
            throw new AppException("Função já existe", HttpStatusCode.Conflict);

        return await _roleRepository.Update(role);
    }

    public async Task<bool> Delete(string id)
    {
        if (!await _roleRepository.ExistsById(id))
            throw new AppException("Função não encontrada", HttpStatusCode.NotFound);

        var role = await _roleRepository.GetById(id);

        if (_roleRepository.IsDefault(role.Name!))
            throw new AppException("Não é possível excluir uma função padrão", HttpStatusCode.BadRequest);

        await _roleRepository.Remove(role);

        return true;
    }
}
