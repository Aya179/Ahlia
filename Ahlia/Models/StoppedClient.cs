using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class StoppedClient
    {
        [Key]
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "المساهم")]
        public int? ClientId { get; set; }
        [Display(Name = "نوع التوقف")]
        public int? StoppedTypeId { get; set; }
        [Display(Name = "تاريخ التحويل لغبر مودع")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "تاريخ التحويل لمودع")]
        public DateTime? Enddate { get; set; }
        public bool? IsActive { get; set; }
        [Display(Name = "السبب")]
        public string? Reason { get; set; }
        [Display(Name = "صورة قرار التحويل لغير مودع")]
        public byte[]? Photo { get; set; }
        [Display(Name = "صورة قرار التحويل لمودع")]
        public byte[]? CancelImage { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Stopping? StoppedType { get; set; }
    }
}
