using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using MemberAPI.Service.Extensions.v1;
using Microsoft.AspNetCore.DataProtection;
using System.Linq;

namespace MemberAPI.Service.Master.v1
{
    public partial class ServiceMaster
    {
        public async Task<Member> ConfirmEmail(string username, string token, CancellationToken ct = default)
        {
            var allmembers = await _unitofWork.MemberData.GetAllMembersAsync();
            var memberEmailConfirmCheck = allmembers.Where(c => c.Email == protectorEmailConfirm.Unprotect(username)
             && c.EmailConfirmationToken == token).SingleOrDefault();

            if (memberEmailConfirmCheck != null)
            {
                if (memberEmailConfirmCheck.IsEmailConfirmed)
                    return memberEmailConfirmCheck;
                memberEmailConfirmCheck.IsEmailConfirmed = true;
                var updatedmember = await _unitofWork.MemberData.UpdateMember(memberEmailConfirmCheck);
                _unitofWork.Complete();
            }
            return memberEmailConfirmCheck;
        }
    }
}