namespace BankAPPAPICoreDomain.Models
{
    public class UserModel : BaseModel
    {
        public string? AccountName { get; set; }
        public string? Email { get; set; }
        public string? PassWord { get; set; }
    }
}
