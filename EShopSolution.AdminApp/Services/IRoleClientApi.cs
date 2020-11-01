using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Services
{
    public interface IRoleClientApi
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
