namespace PROG7311_POE.Models
{
    public class ContractReadDto
    {
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public string ServiceLevel { get; set; }
        public string SignedAgreementPath { get; set; }
    }
}