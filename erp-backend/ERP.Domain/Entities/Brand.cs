

using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class Brand : AuditableEntity
    {

        public string Name { get; set; } = string.Empty;      
        public string description { get; set; } = string.Empty;     
  
    }
}

