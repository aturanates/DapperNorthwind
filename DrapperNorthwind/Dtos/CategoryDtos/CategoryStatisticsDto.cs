using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperNorthwind.Dtos.CategoryDtos
{
    public class CategoryStatisticsDto
    {
        public int TotalCategories { get; set; }

        public int TotalProducts { get; set; }

        public decimal AverageProductsPerCategory { get; set; }

        public decimal TotalInventoryValue { get; set; }

        public string MostPopularCategory { get; set; }

        public string LeastPopularCategory { get; set; }

        public decimal AverageProductPrice { get; set; }

        public int CategoriesWithoutProducts { get; set; }
    }
}
