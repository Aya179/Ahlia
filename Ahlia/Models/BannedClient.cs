using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class BannedClient
    {
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "المساهم")]
        public int? ClientId { get; set; }
        [Display(Name = "نوع الحجز")]
        public int? BannedTypeId { get; set; }
        [Display(Name = "تاريخ  الحجز")]
        public DateTime? Startdate { get; set; }
        [Display(Name = "تاريخ إلغاء الحجز")]
        public DateTime? Enddate { get; set; }
        public bool? IsActive { get; set; }
        [Display(Name = "سبب الحجز")]
        public string? Reason { get; set; }
        [Display(Name = "صورة قرار الحجز")]
        public byte[]? Photo { get; set; }
        [Display(Name = "صورة قرار إلغاء الحجز")]
        public byte[]? CancelImage { get; set; }
        [Display(Name = "ملاحظات")]
        public string? OrderedBy { get; set; }

        public virtual Banning? BannedType { get; set; }
        public virtual Client? Client { get; set; }
    }
}
