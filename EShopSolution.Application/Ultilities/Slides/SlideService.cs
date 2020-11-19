using EShopSolution.Data.EF;
using EShopSolution.ViewModels.Common;
using EShopSolution.ViewModels.Ultilities.Slides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopSolution.Application.Ultilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly EShopDbContext _context;
        public SlideService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<SlideVm>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
            .Select(x => new SlideVm()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Url = x.Url,
                Image = x.Image
            }).ToListAsync();

            return slides;
        }
    }
}
