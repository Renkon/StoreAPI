using StoreAPI.Core.Dto;
using StoreAPI.Core.Model.Payloads;
using System.Threading.Tasks;

namespace StoreAPI.Core.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<UserDto> CreateUserAsync(CreateUserPayload userPayload);

        Task<UserDto> UpdateUserAsync(int nationalId, UpdateUserPayload userPayload);
    }
}
