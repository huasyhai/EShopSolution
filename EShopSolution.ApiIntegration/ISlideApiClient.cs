using EShopSolution.ViewModels.Ultilities.Slides;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShopSolution.ApiIntegration
{
    public interface ISlideApiClient
    {
        Task<List<SlideVm>> GetAll();
    }
}
