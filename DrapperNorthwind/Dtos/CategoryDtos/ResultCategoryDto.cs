using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DapperNorthwind.Dtos.CategoryDtos
{
    public class ResultCategoryDto
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        // Ek bilgiler için hesaplanmış özellikler
        public int ProductCount { get; set; }

        public decimal TotalValue { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModified { get; set; }

        // Display için formatlı özellikler
        public string FormattedValue => TotalValue.ToString("C2");

        public string DisplayText => $"{CategoryName} ({ProductCount} ürün)";
    }
}