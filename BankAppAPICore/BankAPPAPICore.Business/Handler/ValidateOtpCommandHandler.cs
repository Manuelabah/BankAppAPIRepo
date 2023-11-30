using BankAPPAPICore.Business.Command;
using BankAPPAPICore.Business.Utilities;
using BankAPPAPICoreDomain.BindingModels;
using BankAPPAPICoreDomain.Const;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAPPAPICore.Business.Handler
{
    public class ValidateOtpCommandHandler : IRequestHandler<ValidateOtpCommand, ServiceResponse>
    {
        private readonly ILogger<ValidateOtpCommandHandler> _logger;
        private readonly ICacheService _cacheService;
        public ValidateOtpCommandHandler(ILogger<ValidateOtpCommandHandler> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<ServiceResponse> Handle(ValidateOtpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string cachedOTP = await _cacheService.RetrieveFromCacheAsync<string>(request.RequestId);

                if (cachedOTP != null)
                {
                    if (cachedOTP != request.Otp)
                    {
                        return new ServiceResponse
                        {
                            StatusMessage = "Invalid otp.",
                            StatusCode = ResponseCode.BadRequest
                        };
                    }
                    return new ServiceResponse
                    {
                        StatusMessage = "Otp validated successfully.",
                        StatusCode = ResponseCode.SUCCESSFUL
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        StatusMessage = "OTP not found.",
                        StatusCode = ResponseCode.NOTFOUND
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse
                {
                    StatusMessage = "An error occured.",
                    StatusCode = ResponseCode.Error
                };
            }
        }
    }
}
