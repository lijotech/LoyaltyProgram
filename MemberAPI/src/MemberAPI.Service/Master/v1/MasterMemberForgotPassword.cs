using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using System.Linq;
using MemberAPI.Service.Plugins.v1;

namespace MemberAPI.Service.Master.v1
{
    public partial class ServiceMaster
    {
        public async Task<Member> ForgotPassword(string email,
            string forgotPasswordLinkbase, CancellationToken ct = default)
        {
            var forgotPasswordUser = _unitofWork.MemberData.GetAllMembers()
                     .Where(c => c.Email == email && c.IsEmailConfirmed == true).SingleOrDefault();
            if (forgotPasswordUser != null)
            {

                forgotPasswordUser.ForgotPasswordConfirmationToken = protectorForgotPassword
                    .Protect(string.Format("{0}|{1}", forgotPasswordUser.MemberId, System.DateTime.Now.ToString()));
                var updatedMember = await _unitofWork.MemberData.UpdateMember(forgotPasswordUser);

                if (updatedMember != null)
                {
                    var PasswordResetLink = string.Format("{0}?email={1}&token={2}",
                                        forgotPasswordLinkbase,
                                        protectorForgotPassword.Protect(updatedMember.Email),
                                        forgotPasswordUser.ForgotPasswordConfirmationToken
                                        );
                    var message = new Message(new string[] { "lijotech@gmail.com" },
                    "Reset Password", "Click this link to Continue:" + PasswordResetLink, null);
                    _emailSender.SendEmail(message);
                    _unitofWork.Complete();
                }
                else
                    return null;
            }
            return forgotPasswordUser;
        }

        public async Task<Member> ResetPassword(string email, string token, string newPassword,
                                 CancellationToken ct = default)
        {
            string g = protectorForgotPassword.Unprotect(email);
            var forgotPasswordUser = _unitofWork.MemberData.GetAllMembers()
                .Where(c => c.Email == g 
                && c.IsEmailConfirmed == true
                && c.ForgotPasswordConfirmationToken == token).SingleOrDefault();
            if (forgotPasswordUser != null)
            {

                forgotPasswordUser.Password = newPassword;
                forgotPasswordUser.ForgotPasswordConfirmationToken = null;
                var updatedMember = await _unitofWork.MemberData.UpdateMember(forgotPasswordUser);
                _unitofWork.Complete();
            }
            else
                return null;
            return forgotPasswordUser;
        }
    }
}