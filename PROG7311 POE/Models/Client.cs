using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace PROG7311_POE.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Client name is required")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Contact details are required")]
        [StringLength(150)]
        public string? ContactDetails { get; set; }

        [Required(ErrorMessage = "Region is required")]
        [StringLength(50)]
        public string? Region { get; set; }

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
