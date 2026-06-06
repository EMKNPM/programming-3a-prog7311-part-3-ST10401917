using Microsoft.AspNetCore.Http;
using PROG7311_POE.Models;

namespace PROG7311_POE.API.DTOs
{
    public class ContractFormRequest
    {
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public string ServiceLevel { get; set; }

        public IFormFile? File { get; set; }
    }
}