using EShopSolution.ViewModels.Common;

namespace EShopSolution.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {

        public int? CategoryId { get; set; }

        public string LanguageId { get; set; }

    }
}
