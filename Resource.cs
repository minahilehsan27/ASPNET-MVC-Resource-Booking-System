using System.ComponentModel.DataAnnotations;

namespace homework_8.Models
{
    public class Resource
    {
        public int ResourceID { get; set; }

        [Required(ErrorMessage = "Resource name is required")]
        [StringLength(100)]
        public string ResourceName { get; set; }

     
        public string? ResourceDescription { get; set; }

        [Required(ErrorMessage = "Resource Type Required")]
        public string Type { get; set; }
        public string? CreatedByUserId { get; set; } 

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public bool IsAvailable { get; set; } = true;
    }
}