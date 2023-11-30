using BankAPPAPICoreDomain.BindingModels;
using MediatR;

namespace BankAPPAPICore.Business.Command
{
    public class ValidateOtpCommand : IRequest<ServiceResponse>
    {
        public string? RequestId { get; set; }
        public string? Otp { get; set; }
    }
}
