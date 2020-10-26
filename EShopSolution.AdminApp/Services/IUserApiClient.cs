using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest requet);

        Task<PagedResult<UserVm>> GetUsersPagings(GetUserPagingRequest request);

        Task<bool> RegisterUser(RegisterRequest registerRequest);
    }
}
