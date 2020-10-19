using System.Threading.Tasks;
using EShopSolution.Application.Catalog.Products;
using EShopSolution.ViewModels.Catalog.ProductImages;
using EShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _mangeProductService;
        public ProductsController(IPublicProductService publicProducService, IManageProductService manageProductService)
        {
            _publicProductService = publicProducService;
            _mangeProductService = manageProductService;
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

        // http: localhost: port/products?pageSize=10&pageIndex=1&CategoryId=
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetPaging (string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create ( [FromForm] ProductCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _mangeProductService.Create(request);

            if (productId == 0)
                    return BadRequest();

            var product = await _mangeProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);

        }

        [HttpPut]
        public async Task<IActionResult> Update ([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _mangeProductService.Update(request);

            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete ("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _mangeProductService.Delete(productId);

            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _mangeProductService.UpdatePrice(productId, newPrice);

            if (isSuccessful)
                return Ok();

            return BadRequest();
        }

        //image

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _mangeProductService.AddImage(productId, request);

            if (imageId == 0)
                return BadRequest();

            var image = await _mangeProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);

        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _mangeProductService.UpdateImage(imageId, request);

            if (image == 0)
                return BadRequest();

            return Ok();

        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _mangeProductService.RemoveImage(imageId);

            if (image == 0)
                return BadRequest();

            return Ok();

        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var productImage = await _mangeProductService.GetImageById(imageId);
            if (productImage == null)
                return BadRequest();
            return Ok(productImage);
        }


    }
}