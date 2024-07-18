using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class City
    {
        public City()
        {
            Clients = new HashSet<Client>();
            Employees = new HashSet<Employee>();
            Payments = new HashSet<Payment>();
        }
        [Display(Name = "المعرف")]

        public int CityId { get; set; }
        [Display(Name = "اسم المدينة")]
        public string? CityName { get; set; }
        [Display(Name = "البلد")]
        public string? Country { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
