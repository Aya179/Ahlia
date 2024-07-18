using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class Client
    {
        public Client()
        {
            BannedClients = new HashSet<BannedClient>();
            ClientStokcksMovementClients = new HashSet<ClientStokcksMovement>();
            ClientStokcksMovementNewClients = new HashSet<ClientStokcksMovement>();
            Payments = new HashSet<Payment>();
            Penefits = new HashSet<Penefit>();
        }
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "رقم المساهم")]
        public string clientnumber { get; set; }
        [Display(Name = "الاسم   ")]
        public string? FirstName { get; set; }
        [Display(Name = "اسم الأب")]
        public string? MiddleName { get; set; }
        [Display(Name = "الكنية")]
        public string? LastName { get; set; }
        [Display(Name = "اسم الأم")]
        public string? Mother { get; set; }
        [Display(Name = "رقم الوثيقة الرسمية")]
        
        [StringLength(11, MinimumLength = 11,ErrorMessage = "الرجاء ادخال رقم الوثيقة الرسمية بشكل صحيح")]
        [RegularExpression("[0-9]+", ErrorMessage = "الرجاء ادخال رقم الوثيقة الرسمية بشكل صحيح")]
        public string? NationalId { get; set; }
        [Display(Name = "نوع الوثيقة الرسمية")]
        public string? NationalIdType { get; set; }
        [Display(Name = "رقم  العمل")]

        // [RegularExpression(@"(^(09)[0-9]{8}$)|(^(9639)[0-9]{8}$)", ErrorMessage = " يرجى ادخل رقم بصيغة 09أو 9639xxxxxxxx")]
        //[RegularExpression(@"(^(09)[0-9]{8}$)", ErrorMessage = " يرجى ادخل رقم بصيغة 09xxxxxxxx")]
        [RegularExpression(@"^[+][0-9]{7,20}$", ErrorMessage = " يرجى إدخال رقم العمل بالصيغة الصحيحة بحيث يبدأ ب+")]
        public string? Mobile1 { get; set; }
        [Display(Name = "رقم الخليوي")]
        [RegularExpression(@"^[+][0-9]{7,20}$", ErrorMessage = " يرجى إدخال الرقم الخليوي بالصيغة الصحيحة بحيث يبدأ ب+")]
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
        [RegularExpression(@"^[+][0-9]{7,20}$", ErrorMessage = " يرجى إدخال رقم المنزل بالصيغة الصحيحة بحيث يبدأ ب+")]
        //[RegularExpression(@"^(09)[0-9]{8}$", ErrorMessage = "يرجى ادخل رقم بصيغة 09xxxxxxxx")]
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
        [Display(Name = "صورة الهوية الوجه الثاني")]
        public byte[]? IdcardPhoto { get; set; }
        [Display(Name = "الملاحظات")]
        public string? Notes { get; set; }
        public bool? IsAlive { get; set; }
        public bool? IsDeleted { get; set; }
        [Display(Name = "المدينة")]
        public int? CityId { get; set; }
        [Display(Name = "عدد الاسهم الفعالة")]
        public int? ActiveStocks { get; set; }
        [Display(Name = "عدد الاسهم الغير فعالة")]
        public int? NotactiveStocks { get; set; }

        public virtual City? City { get; set; }
        public virtual ClientType? ClientType { get; set; }
        public virtual ICollection<BannedClient> BannedClients { get; set; }
        public virtual ICollection<ClientStokcksMovement> ClientStokcksMovementClients { get; set; }
        public virtual ICollection<ClientStokcksMovement> ClientStokcksMovementNewClients { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Penefit> Penefits { get; set; }
    }
}
