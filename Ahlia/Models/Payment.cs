using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class Payment
    {
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "تاريخ الدفع ")]
        public DateTime? Paymentdate { get; set; }
        [Display(Name = "المبلغ")]
        public decimal? Amount { get; set; }
        [Display(Name = "المساهم")]
        public int? ClientId { get; set; }
        [Display(Name = "المدينة")]
        public int? CityId { get; set; }
        [Display(Name = "اسم الفرع")]
        public string? BranchName { get; set; }
        [Display(Name = "جهة الدفع")]
        public string? PayementFor { get; set; }
        [Display(Name = "المحرر")]
        public int? Editor { get; set; }
        [Display(Name = "المدقق")]
        public int? Checker { get; set; }

        [Display(Name = "رقم الوصل")]
        public string? CheckNumber { get; set; }
        [Display(Name = "حساب البنك")]
        public string? BankAccount { get; set; }
        [Display(Name = "تاريخ الوصل")]
        public DateTime? CheckDate { get; set; }
        [Display(Name = "اسم المستقبل")]
        public string? ReceiverName { get; set; }
        [Display(Name = "رقم التسليم")]
        public string? ReceiverNumber { get; set; }
        [Display(Name = "تاريخ التسليم")]
        public DateTime? RecieveDate { get; set; }
        
        public string? ReceiverResidance { get; set; }
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPayed { get; set; }
        [Display(Name = "رقم البطاقة")]

        public int? CardNumber { get; set; }

        public virtual Employee? CheckerNavigation { get; set; }
        public virtual City? City { get; set; }
        public virtual Client? Client { get; set; }
        public virtual Employee? EditorNavigation { get; set; }
    }
}
