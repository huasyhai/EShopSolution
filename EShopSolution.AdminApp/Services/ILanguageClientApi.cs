using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Services
{
    public interface ILanguageClientApi
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}
