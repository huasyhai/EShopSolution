using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.Ultilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShopSolution.Application.Ultilities.Slides
{
    public interface ISlideService
    {
        Task<List<SlideVm>> GetAll();
    }
}
