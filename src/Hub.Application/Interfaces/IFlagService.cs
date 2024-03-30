using Hub.Domain.DTOs;
using Hub.Domain.Entities;

namespace Hub.Application.Interfaces;

public interface IFlagService
{
    Task<List<Flag>> GetAll();
    Task<Flag> GetById(string id);
    Task<int> GetCount();
    Task<Flag> Create(Flag flag);
    Task<Flag> Update(Flag flag);
    Task<Flag> Delete(string id, UserDTO user);
}
