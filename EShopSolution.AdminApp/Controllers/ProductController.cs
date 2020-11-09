using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopSolution.AdminApp.Services;
using EShopSolution.Ultilities.Constant;
using EShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace EShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSetting.DefaultLanguageId);

            var request = new GetManageProductPagingRequest()
            {
                KeyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
                CategoryId = categoryId
            };
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll(languageId);

            ViewBag.categories = categories.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            var data = await _productApiClient.GetPagings(request);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> Create ([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

    }
}