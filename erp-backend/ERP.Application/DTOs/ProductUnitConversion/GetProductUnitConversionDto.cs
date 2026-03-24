using ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.DTOs.ProductUnitConversion
{
    public class GetProductUnitConversionDto
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string FromUnitName { get; set; }

        public string ToUnitName { get; set; }

        public decimal Factor { get; set; }

  
    }
}
