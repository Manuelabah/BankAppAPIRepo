namespace BankAPPAPICoreDomain.Models
{
    public class TransactionModel : BaseModel
    {
        public string? Amount { get; set; }
        public string? TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public string? TransactionDescription { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public double AccountBalance { get; set; }
    }
}
