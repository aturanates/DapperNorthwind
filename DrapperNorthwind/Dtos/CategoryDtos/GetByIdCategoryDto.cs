using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DapperNorthwind.Dtos.CategoryDtos
{
    public class GetByIdCategoryDto
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public int ProductCount { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal TotalInventoryValue { get; set; }

        public string MostExpensiveProduct { get; set; }

        public string CheapestProduct { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
