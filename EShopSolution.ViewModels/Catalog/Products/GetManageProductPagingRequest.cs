using EShopSolution.ViewModels.Common;
using System.Collections.Generic;

namespace EShopSolution.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string KeyWord { get; set; }

        public List<int> CategoryIds { get; set; }

        public string LanguageId { get; set; }
    }
}
