using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class Banning
    {
        public Banning()
        {
            BannedClients = new HashSet<BannedClient>();
        }
        [Display(Name = "المعرف")]

        public int Id { get; set; }
        [Display(Name = "النوع")]
        public string? BannedType { get; set; }

        public virtual ICollection<BannedClient> BannedClients { get; set; }
    }
}
