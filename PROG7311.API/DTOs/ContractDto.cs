using PROG7311_POE.Models;

namespace PROG7311.API.DTOs
{
    public class ContractDto
    {
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public string? ServiceLevel { get; set; }
    }
}
