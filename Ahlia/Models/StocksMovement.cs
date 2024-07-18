using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public partial class StocksMovement
    {
        public StocksMovement()
        {
            ClientStokcksMovements = new HashSet<ClientStokcksMovement>();
        }
        [Display(Name = "المعرف")]
        public int Id { get; set; }
        [Display(Name = "النوع")]
        public string? MovementType { get; set; }
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        public virtual ICollection<ClientStokcksMovement> ClientStokcksMovements { get; set; }
    }

}
