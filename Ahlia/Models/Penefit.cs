using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class Penefit
    {
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "المساهم")]
        public int? ClientId { get; set; }
        [Display(Name = "سعر السهم")]
        public int? PriceId { get; set; }
        [Display(Name = "المبلغ الإجمالي")]
        public decimal? CompleteAmount { get; set; }

        public virtual Client? Client { get; set; }
        public virtual StockPrice? Price { get; set; }
    }
}
