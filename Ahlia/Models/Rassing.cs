using System.ComponentModel.DataAnnotations;

namespace Ahlia.Models
{
    public class Rassing
    {
        public int id { get; set; }
        [Display(Name = "مقدار الزبادة")]
        public float? Amount { get; set; }
        [Display(Name = "صورة القرار")]
        public byte[]? ContractImag { get; set; }
        [Display(Name = "نوع الزيادة")]
        public string? RassingType { get; set; }
        [Display(Name = "تاريخ الزيادة")]
        public DateTime? RassingDate { get; set; }
    }
}
