using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class Employee
    {
        public Employee()
        {
            PaymentCheckerNavigations = new HashSet<Payment>();
            PaymentEditorNavigations = new HashSet<Payment>();
        }
        [Display(Name = "المعرف")]

        public int Id { get; set; }
        [Display(Name = "الاسم")]

        public string? EmployeeName { get; set; }
        [Display(Name = "الاسم")]

        public string? UserName { get; set; }
        [Display(Name = "كلمة السر")]

        public string? Password { get; set; }
        [Display(Name = "الايميل")]

        public string? Email { get; set; }
        [Display(Name = "رقم الموبايل")]

        public string? Mobile { get; set; }
        public string? RoleId { get; set; }
        public bool? IsDeleted { get; set; }
        [Display(Name = "المدينة")]

        public int? CityId { get; set; }
        [Display(Name = "العنوان")]

        public string? Address { get; set; }
        public string? EmpId { get; set; }


        [Display(Name = "المدينة")]


        public virtual City? City { get; set; }
        public virtual ICollection<Payment> PaymentCheckerNavigations { get; set; }
        public virtual ICollection<Payment> PaymentEditorNavigations { get; set; }
    }
}
