using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.System.Users;
using System;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest requet);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserVm>> GetById(Guid id);

        Task<ApiResult<bool>> DeleteUser(Guid Id);
    }
}
