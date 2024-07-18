using Nest;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public class selectModel
    {
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "رقم المساهم")]
        public string clientnumber { get; set; }

        [Display(Name = "الاسم")]
        public string? FirstName { get; set; }
        [Display(Name = "اسم الأب")]
        public string? MiddleName { get; set; }
        [Display(Name = "الكنية")]
        public string? LastName { get; set; }
        [Display(Name = "اسم الأم")]
        public string? Mother { get; set; }
        [Display(Name = "الرقم الوطني")]
        public string? NationalId { get; set; }
        [Display(Name = "الجنسية")]
        public string? NationalIdType { get; set; }
        [Display(Name = "رقم العمل")]
        public string? Mobile1 { get; set; }
        [Display(Name = "رقم هاتف ثان")]
        public string? Mobile2 { get; set; }
        [Display(Name = "العنوان")]
        public string? AddressDomicil { get; set; }
        [Display(Name = "عنوان العمل")]
        public string? AddressWork { get; set; }
        [Display(Name = "عنوان السكن")]
        public string? OriginalAddress { get; set; }
        [Display(Name = "الجنسية")]
        public string? Nationality { get; set; }
        [Display(Name = "حالة المساهم")]
        public string? ClientStatus { get; set; }
        [Display(Name = "تفعيل")]
        public bool? IsActive { get; set; }
        [Display(Name = "رقم المنزل")]
        public string? HomePhone { get; set; }
        [Display(Name = "الفاكس")]
        public string? Fax { get; set; }
        [Display(Name = "الخانة")]
        public string? Khana { get; set; }
        [Display(Name = "تاريخ الميلاد")]
        public DateTime? Birthdate { get; set; }
        [Display(Name = "مدينة الولادة")]
        public string? Birthcity { get; set; }
        [Display(Name = "نوع المساهم")]
        public int? ClientTypeId { get; set; }
        [Display(Name = "حساب البنك")]
        public string? BankAccount { get; set; }
        [Display(Name = "صورة الهوية")]
        public byte[]? Idphoto { get; set; }
        [Display(Name = "صورة2")]
        public byte[]? IdcardPhoto { get; set; }
        [Display(Name = "الملاحظات")]
        public string? Notes { get; set; }
        public bool? IsAlive { get; set; }
        public bool? IsDeleted { get; set; }
        [Display(Name = "المدينة")]
        public int? CityId { get; set; }
        [Display(Name = "الرصيد")]
        public decimal? total { get; set; }
        [Display(Name = "عدد الاسهم الفعالة")]
        public int? ActiveStocks { get; set; }
        [Display(Name = "عدد الاسهم الغير فعالة")]
        public int? NotactiveStocks { get; set; }
        public int ClientId{ get; set; }

        [Display(Name = "إجمالي الدفعات")]
        public decimal? totalPayment { get; set; }
        [Display(Name = "إجمالي الأرباح")]
        public decimal? totalPenifet { get; set; }
        [Display(Name = "عدد الاسهم ")]
        public int? TotalStocks { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Paymentdate { get; set; }
        public decimal ?totalbeforpayments { get; set; }
       


    }
}
