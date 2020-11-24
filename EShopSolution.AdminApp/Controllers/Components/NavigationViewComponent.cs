using EShopSolution.AdminApp.Models;
using EShopSolution.ApiIntegration;
using EShopSolution.Ultilities.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageClientApi;

        public NavigationViewComponent(ILanguageApiClient languageClientApi)
        {
            _languageClientApi = languageClientApi;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languageClientApi.GetAll();

            var navigationVm = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSetting.DefaultLanguageId),
                Languages = languages.ResultObject
            };

            return View("Default", navigationVm);
        }
    }
}
