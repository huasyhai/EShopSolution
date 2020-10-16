using System.Threading.Tasks;
using EShopSolution.Application.Catalog.Products;
using EShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _mangeProductService;
        public ProductController(IPublicProductService publicProducService, IManageProductService manageProductService)
        {
            _publicProductService = publicProducService;
            _mangeProductService = manageProductService;
        }

        // http: localhost: port/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllByLanguageId (string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
        }

        // http: localhost: port/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _mangeProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest();
            return Ok(product);
        }

        // http: localhost: port/product/public-paging
        [HttpGet("public-paging")]
        public async Task<IActionResult> GetGetAllByCategoryId ([FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create ( [FromForm] ProductCreateRequest request)
        {
            var productId = await _mangeProductService.Create(request);

            if (productId == 0)
                    return BadRequest();

            var product = await _mangeProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);

        }

        [HttpPut]
        public async Task<IActionResult> Update ([FromForm] ProductUpdateRequest request)
        {
            var affectedResult = await _mangeProductService.Update(request);

            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _mangeProductService.Delete(id);

            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var isSuccessful = await _mangeProductService.UpdatePrice(id, newPrice);

            if (isSuccessful)
                return Ok();

            return BadRequest();
        }
    }
}