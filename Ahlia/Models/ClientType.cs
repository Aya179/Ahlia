using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class ClientType
    {
        public ClientType()
        {
            Clients = new HashSet<Client>();
        }
        [Display(Name = "المعرف")]
        public int TypeId { get; set; }
        [Display(Name = "النوع")]
        public string? TypeName { get; set; }
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
