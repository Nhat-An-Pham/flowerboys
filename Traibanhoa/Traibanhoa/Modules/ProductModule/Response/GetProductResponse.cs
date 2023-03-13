using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traibanhoa.Modules.ProductModule.Response
{
    public class GetProductResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? Price { get; set; }
        public Guid TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
