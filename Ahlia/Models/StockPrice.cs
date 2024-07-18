using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class StockPrice
    {
        public StockPrice()
        {
            Penefits = new HashSet<Penefit>();
        }
        [Display(Name = "المعرف")]

        public int Id { get; set; }
        [Display(Name = "ربح السهم")]
        public decimal? Shareprice { get; set; }
        [Display(Name = "التاريخ")]
        public DateTime? Sharedate { get; set; }
        [Display(Name = "السنة")]
        public int? Year { get; set; }
        [Display(Name = "صورة القرار")]
        public byte[]? ContractImage { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Penefit> Penefits { get; set; }
    }
}
