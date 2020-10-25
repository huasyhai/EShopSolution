using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace EShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);

        Task<PagedResult<UserVm>> GetUsersPaging(GetUserPagingRequest request);
    }
}
