using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE.Models
{
    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "Client is required")]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public ContractStatus Status { get; set; }

        [Required(ErrorMessage = "Service level is required")]
        [StringLength(50)]
        public string? ServiceLevel { get; set; }

        public string? SignedAgreementPath { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
