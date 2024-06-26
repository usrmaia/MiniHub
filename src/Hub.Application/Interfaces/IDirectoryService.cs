﻿using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Filters;

namespace Hub.Application.Interfaces;

public interface IDirectoryService
{
    Task<QueryResult<DirectoryE>> Query(DirectoryFilter filter);
    Task<List<DirectoryE>> GetAll();
    Task<DirectoryE> GetById(string id);
    Task<int> GetCount();
    Task<DirectoryE> Create(DirectoryE directory);
    Task<DirectoryE> Update(DirectoryE directory);
    Task<DirectoryE> Delete(string id, UserDTO user);
}
