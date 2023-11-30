namespace BankAPPAPICoreDomain.Models
{
    public class AccountModel : BaseModel
    {
        public string? AccountType { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public double AccountBalance { get; set; }
    }
}
