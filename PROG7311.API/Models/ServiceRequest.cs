using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE.Models
{
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestId { get; set; }

        [Required(ErrorMessage = "Contract is required")]
        public int ContractId { get; set; }



        public Contract? Contract { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "USD cost is required")]
        [Range(0.01, 1000000, ErrorMessage = "Cost must be greater than 0")]
        public decimal CostUSD { get; set; }
        public decimal CostZAR { get; set; }

        public RequestStatus Status { get; set; }
    }
}
