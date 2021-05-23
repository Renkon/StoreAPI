using StoreAPI.Core.Dto;
using StoreAPI.Core.Model.Payloads;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreAPI.Core.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<UserDto> CreateUserAsync(CreateUserPayload userPayload);

        Task<UserDto> UpdateUserAsync(int nationalId, UpdateUserPayload userPayload);

        Task DeleteUserAsync(int nationalId);

        Task<UserDto> GetUserAsync(int nationalId);

        Task<IEnumerable<UserDto>> GetUsersAsync();

        Task<IEnumerable<UserDto>> GetMoreThanAverageSpentUsersAsync();

        Task PerformPurchaseAsync(CreatePurchaseRecordPayload purchaseRecordPayload);
    }
}
