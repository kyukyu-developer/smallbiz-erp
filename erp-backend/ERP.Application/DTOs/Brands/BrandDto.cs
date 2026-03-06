using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.DTOs.Brands
{
    public class BrandDto
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool Active { get; set; }


    }
}
