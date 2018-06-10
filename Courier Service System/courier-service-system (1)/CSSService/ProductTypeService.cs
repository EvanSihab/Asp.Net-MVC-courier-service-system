using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public class ProductTypeService : Service<ProductType>,IProductTypeService
    {
        private DataContext context;
        ProductTypeRepository productTypeRepository = new ProductTypeRepository();
        public ProductTypeService() { this.context = DataContext.getInstance(); }

        public int UpdateProductType(ProductType pt)
        {
            return productTypeRepository.UpdateProductType(pt);
        }
        public List<ProductType> GetAllTypeForCheck(ProductType productType)
        {
            return productTypeRepository.GetAllTypeForCheck(productType);
        }
    }
}
