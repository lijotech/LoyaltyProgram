
using MemberAPI.Data.Repository.v1;
using Microsoft.AspNetCore.DataProtection;
using MemberAPI.Data.Security.v1;
using System;

namespace MemberAPI.Service.v1.Master
{   
    public partial class ServiceMaster : IServiceMaster
    {        
        private IUnitofWork _unitofWork;
        private readonly IEmailSender _emailSender;
        private readonly IDataProtector protector;
        private readonly IDataProtector protectorForgotPassword;
        public IDataProtectionProvider _dataProtectionProvider { get; }
             
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
            _dataProtectionProvider = dataProtectionProvider;
            _emailSender = emailSender;
            protector = _dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberEmailConfirmationValue);
            protectorForgotPassword = _dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberForgotPasswordTokenValue);
        }

       
    }
}