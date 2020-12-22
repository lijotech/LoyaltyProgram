
using MemberAPI.Data.Repository.v1;
using MemberAPI.Data.Security.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.Service.Plugins.v1;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace MemberAPI.Service.Master.v1
{   
    public partial class ServiceMaster : IServiceMaster
    {        
        private IUnitofWork _unitofWork;
        private readonly IEmailSender _emailSender;
        private readonly IDataProtector protectorEmailConfirm;
        private readonly IDataProtector protectorForgotPassword;
        private readonly ILogger<ServiceMaster> _logger;

        public ServiceMaster()
        {

        }
        public ServiceMaster(
            IUnitofWork unitofWork,
            IDataProtectionProvider dataProtectionProvider,
            IEmailSender emailSender,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            ILogger<ServiceMaster> logger)
        {
            _unitofWork=unitofWork;            
            _emailSender = emailSender;
            _logger = logger;
            protectorEmailConfirm = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberEmailConfirmationValue);
             protectorForgotPassword = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberForgotPasswordTokenValue);
        }

       
    }
}