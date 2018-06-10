using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
   public interface IProductTypeRepository : IRepository<ProductType>
    {
        int UpdateProductType(ProductType pt);

        List<ProductType> GetAllTypeForCheck(ProductType productType);
    }
}
