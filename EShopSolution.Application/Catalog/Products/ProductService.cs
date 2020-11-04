using EShopSolution.Application.Common;
using EShopSolution.Data.EF;
using EShopSolution.Data.Entities;
using EShopSolution.Ultilities.Exceptions;
using EShopSolution.ViewModels.Catalog.ProductImages;
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
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var productImage = new ProductImage()
            {

                ProductId = productId,
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
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
            if (request.ThumbnailImage != null)
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

            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var thumnailImages = _context.ProductImages.Where(x => x.ProductId == productId);

            foreach (var thumnailImage in thumnailImages)
            {
                await _storageService.DeleteFileAsync(thumnailImage.ImagePath);
            }

            _context.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductVm>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // 1. Select join 
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        //join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        //join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt };

            // 2. Filter

            if (!String.IsNullOrEmpty(request.KeyWord))
                query = query.Where(x => x.pt.Name.Contains(request.KeyWord));
            //if (request.CategoryIds != null && request.CategoryIds.Count > 0)
            //    query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));

            //3. Paging

            int TotalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                       .Take(request.PageSize)
                       .Select(x => new ProductVm()
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
            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = TotalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return pagedResult;

        }

        public async Task<ProductVm> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);

            var productTransaltion = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);

            if (product == null || productTransaltion == null)
                throw new EShopException($"Cannot find a product with id: {productId}");

            var productViewModel = new ProductVm()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTransaltion != null ? productTransaltion.Description : null,
                LanguageId = productTransaltion != null ? productTransaltion.LanguageId : null,
                Details = productTransaltion != null ? productTransaltion.Details : null,
                Name = productTransaltion != null ? productTransaltion.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTransaltion != null ? productTransaltion.SeoAlias : null,
                SeoTitle = productTransaltion != null ? productTransaltion.SeoTitle : null,
                SeoDescription = productTransaltion != null ? productTransaltion.SeoDescription : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };

            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null )
                throw new EShopException($"Cannot find a product with id: {imageId}");

            var productImageViewModel = new ProductImageViewModel()
            {
                Id = productImage.Id,
                DateCreated = productImage.DateCreated,
                ProductId = productImage.ProductId,
                ImagePath = productImage.ImagePath,

                Caption = productImage.Caption,

                IsDefault = productImage.IsDefault,

                SortOrder = productImage.SortOrder,
     
                FileSize = productImage.FileSize
             };

            return productImageViewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productImage = _context.ProductImages.Where(x => x.ProductId == productId);

            if (product == null || productImage == null) throw new EShopException($"Cannot find a product with id: {productId}");

            var thumbnailImage = await _context.ProductImages.Where(x => x.ProductId == productId)

                .Select(x => new ProductImageViewModel()
                {
                    Caption = x.Caption,

                    DateCreated = x.DateCreated,

                    ImagePath = x.ImagePath,

                    Id = x.Id,

                    ProductId = x.ProductId,

                    IsDefault = x.IsDefault,

                    FileSize = x.FileSize,

                    SortOrder = x.SortOrder

                }).ToListAsync();

            return thumbnailImage;
        }

        public Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null) throw new EShopException($"Cannot find a image of product with id: {imageId}");

            _context.ProductImages.Remove(productImage);
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

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new EShopException($"Cannot find an image with id: {imageId}");


            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Update(productImage);
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

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileNames = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileNames)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<PagedResult<ProductVm>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            // 1. Select join 
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            // 2. Filter

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
                query = query.Where(x => x.pic.CategoryId == request.CategoryId.Value);

            //3. Paging

            int TotalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                       .Take(request.PageSize)
                       .Select(x => new ProductVm()
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
            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = TotalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };

            return pagedResult;
        }
    }
}
