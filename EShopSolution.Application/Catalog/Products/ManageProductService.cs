using EShopSolution.Application.Common;
using EShopSolution.Data.EF;
using EShopSolution.Data.Entities;
using EShopSolution.Ultilities.Exceptions;
using EShopSolution.ViewModels.Catalog.Products;
using EShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService )
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImages(int productId, List<IFormFile> files)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            foreach (var file in files)
            {
                var productImage = new ProductImage()
                {
                    ProductId = productId,
                    Caption = "Thumbnail Image",
                    DateCreated = DateTime.Now,
                    ImagePath = await SaveFile(file),
                    IsDefault = true
                };

                _context.ProductImages.Add(productImage);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation() {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };
            // save image
            if(request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _context.Products.Add(product);

            return await _context.SaveChangesAsync(); 
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var thumnailImages = _context.ProductImages.Where(x => x.ProductId == productId);

            foreach(var thumnailImage in thumnailImages)
            {
                await _storageService.DeleteFileAsync(thumnailImage.ImagePath);
            }

            _context.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAlllPaging(GetManageProductPagingRequest request)
        {
            // 1. Select join 
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            // 2. Filter

            if (!String.IsNullOrEmpty(request.KeyWord))
                query = query.Where(x => x.pt.Name.Contains(request.KeyWord));
            if (request.CategoryIds.Count > 0)
                query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));

            //3. Paging

            int TotalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                       .Take(request.PageSize)
                       .Select(x => new ProductViewModel()
                       {
                           Id = x.p.Id,
                           Name = x.pt.Name,
                           DateCreated = x.p.DateCreated,
                           Description = x.pt.Description,
                           Details = x.pt.Details,
                           LanguageId = x.pt.LanguageId,
                           OriginalPrice = x.p.OriginalPrice,
                           Price = x.p.Price,
                           SeoAlias = x.pt.SeoAlias,
                           SeoDescription = x.pt.SeoDescription,
                           SeoTitle = x.pt.SeoTitle,
                           Stock = x.p.Stock,
                           ViewCount = x.p.ViewCount
                       }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = TotalRow,
                Items = data
            };

            return pagedResult;

        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productImage = _context.ProductImages.Where(x => x.ProductId == productId);

            if (product == null || productImage == null) throw new EShopException($"Cannot find a product with id: {productId}");

            var thumbnailImage = await _context.ProductImages.Where(x => x.ProductId == productId)

                .Select(x => new ProductImageViewModel()
                {
                     Id = x.Id,

                     FilePath = x.ImagePath,

                     IsDefault = x.IsDefault,

                     FileSize  = x.FileSize

                }).ToListAsync();

            return thumbnailImage;
        }

        public async Task<int> RemoveImages(int imageId)
        {
            var thumnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);

            if (thumnailImage == null) throw new EShopException($"Cannot find a image of product with id: {imageId}");

            _context.ProductImages.Remove(thumnailImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;

            // save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.IsDefault && x.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }


            return await _context.SaveChangesAsync();

        }

        public async Task<int> UpdateImages(int imageId, string caption, bool IsDefault)
        {
            var thumnailImage =  await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (thumnailImage == null) throw new EShopException($"Cannot find a image of product with id: {imageId}");

            thumnailImage.Caption = caption;
            thumnailImage.IsDefault = IsDefault;
            _context.ProductImages.Update(thumnailImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file) {
            var originalFileNames = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileNames)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
