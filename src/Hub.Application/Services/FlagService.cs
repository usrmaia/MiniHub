using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using System.Net;

namespace Hub.Application.Services;

public class FlagService : IFlagService
{
    private readonly IFlagRepository _flagRepository;
    private readonly IUserRepository _userRepository;

    public FlagService(IFlagRepository flagRepository, IUserRepository userRepository)
    {
        _flagRepository = flagRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Flag>> GetAll() =>
        await _flagRepository.GetAll();

    public async Task<Flag> GetById(string id)
    {
        if (!await _flagRepository.ExistsById(id))
            throw new AppException("Flag não encontrada", HttpStatusCode.NotFound);

        return await _flagRepository.GetById(id);
    }

    public async Task<int> GetCount() =>
        await _flagRepository.GetCount();

    public async Task<Flag> Create(Flag flag)
    {
        if (string.IsNullOrWhiteSpace(flag.Name))
            throw new AppException("Nome da flag é obrigatório", HttpStatusCode.BadRequest);

        if (await _flagRepository.ExistByName(flag.Name))
            throw new AppException("Flag já existe", HttpStatusCode.BadRequest);

        if (!await _userRepository.ExistsById(flag.UserId))
            throw new AppException("Usuário não encontrado", HttpStatusCode.BadRequest);

        return await _flagRepository.Add(flag);
    }

    public async Task<Flag> Update(Flag newFlag)
    {
        if (!await _flagRepository.ExistsById(newFlag.Id))
            throw new AppException("Flag não encontrada", HttpStatusCode.NotFound);

        var oldFlag = await _flagRepository.GetById(newFlag.Id!);

        if (oldFlag.UserId != newFlag.UserId)
            throw new AppException("Usuário não possui permissão para alterar esta flag", HttpStatusCode.Forbidden);

        if (oldFlag.Name != newFlag.Name && await _flagRepository.ExistByName(newFlag.Name))
            throw new AppException("Flag já existe", HttpStatusCode.BadRequest);

        if (oldFlag.UserId != newFlag.UserId)
            throw new AppException("Usuário não pode ser alterado", HttpStatusCode.BadRequest);

        oldFlag.Name = newFlag.Name;
        oldFlag.Description = newFlag.Description;

        return await _flagRepository.Update(oldFlag);
    }

    public async Task<Flag> Delete(string id, UserDTO user)
    {
        if (!await _flagRepository.ExistsById(id))
            throw new AppException("Flag não encontrada", HttpStatusCode.NotFound);

        var flag = await _flagRepository.GetById(id);

        if (flag.UserId != user.Id)
            throw new AppException("Usuário não possui permissão para deletar esta flag", HttpStatusCode.Forbidden);

        return await _flagRepository.RemoveById(id);
    }
}
