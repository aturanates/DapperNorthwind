﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrapperNorthwind.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        //public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        //public short UnitsOnOrder { get; set; }
        //public short ReorderLevel { get; set; }
        //public bool Discontinued { get; set; }
        public int CategoryId { get; set; }
    }
}
