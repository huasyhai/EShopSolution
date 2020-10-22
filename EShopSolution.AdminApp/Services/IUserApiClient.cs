using EShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest requet);
    }
}
