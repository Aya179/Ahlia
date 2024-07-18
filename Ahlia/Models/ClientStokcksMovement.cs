using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class ClientStokcksMovement
    {
        [Display(Name = "رقم عملية النقل")]
        public int Id { get; set; }
        [Display(Name = "المساهم")]
        public int? ClientId { get; set; }
        [Display(Name = "النوع")]
        public int? MovementTypeId { get; set; }
        [Display(Name = "عدد الأسهم المنقولة")]
        public int? Amount { get; set; }
        [Display(Name = "االمساهم الجديد")]
        public int? NewClientId { get; set; }
        [Display(Name = "تاريخ النقل")]
        public DateTime? MovementDate { get; set; }
        [Display(Name = "الصورة")]
        public string? ContractImage { get; set; }
        [Display(Name = "سبب النقل")]
        public string? Reason { get; set; }
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
        public bool? IsApproved { get; set; }

        public virtual Client? Client { get; set; }
        public virtual StocksMovement? MovementType { get; set; }
        public virtual Client? NewClient { get; set; }
    }
}
