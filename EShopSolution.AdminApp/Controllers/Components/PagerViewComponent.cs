using EShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShopSolution.AdminApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync (PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
