
using MemberAPI.Data.Repository.v1;
using MemberAPI.Data.Security.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.Service.Plugins.v1;
using Microsoft.AspNetCore.DataProtection;

namespace MemberAPI.Service.Master.v1
{   
    public partial class ServiceMaster : IServiceMaster
    {        
        private IUnitofWork _unitofWork;
        private readonly IEmailSender _emailSender;
        private readonly IDataProtector protectorEmailConfirm;
        private readonly IDataProtector protectorForgotPassword;

        public ServiceMaster()
        {

        }
        public ServiceMaster(
            IUnitofWork unitofWork,
            IDataProtectionProvider dataProtectionProvider,
            IEmailSender emailSender,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _unitofWork=unitofWork;            
            _emailSender = emailSender;           
             protectorEmailConfirm = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberEmailConfirmationValue);
             protectorForgotPassword = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberForgotPasswordTokenValue);
        }

       
    }
}