using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.WebApi.Extentions
{
    public static class EntityExtensions
    {
        public static bool IsObjectNull(this object entity)
        {
            return entity == null;
        }
    }
}
