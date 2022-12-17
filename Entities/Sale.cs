using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Sale : Entity
    {
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int StatusId { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
