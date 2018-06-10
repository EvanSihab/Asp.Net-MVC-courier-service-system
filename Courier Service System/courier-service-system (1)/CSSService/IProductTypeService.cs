using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public interface IProductTypeService : IService<ProductType>
    {
        int UpdateProductType(ProductType pt);
        List<ProductType> GetAllTypeForCheck(ProductType productType);
    }
}
