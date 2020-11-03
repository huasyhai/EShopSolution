using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Services
{
    public class RoleClientApi : BaseClientApi, IRoleClientApi
    {

        public RoleClientApi(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor )
        {

        }
        public async Task<ApiResult<List<RoleVm>>> GetAll()
        {

            return await GetAsync<ApiResult<List<RoleVm>>>("/api/roles");
        }
    }
}
