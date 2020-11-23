
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using MemberAPI.Service.Extensions.v1;
using Microsoft.AspNetCore.DataProtection;
using MemberAPI.Service.Plugins.v1;
using System.Linq;


namespace MemberAPI.Service.Master.v1
{
    public partial class ServiceMaster
    {
        public async Task<Member> AddMemberAsync(
                                    Member member,
                                    string confirmationLinkbase,
                                    CancellationToken ct = default)
        {
            string emailToken = protectorEmailConfirm.Protect(string.Format("{0}{1}",
                                member.Username,
                                System.DateTime.Now.ToString()));

            member.EmailConfirmationToken = emailToken;
            var addedMember = await _unitofWork.MemberData.AddMemberAsync(member);

            if (addedMember != null)
            {
                var confirmationLink = string.Format("{0}?username={1}&token={2}",
                                        confirmationLinkbase,
                                        protectorEmailConfirm.Protect(member.Username),
                                        member.EmailConfirmationToken
                                        );

                var message = new Message(new string[] { "lijotech@gmail.com" },
                "Email Confirmation", "Click this link to Continue:" + confirmationLink, null);
                _emailSender.SendEmail(message);
                _unitofWork.Complete();
            }
            return addedMember;
        }

        public async Task<IEnumerable<Member>> GetAllMembers(CancellationToken ct = default)
        {
            return await _unitofWork.MemberData.GetAllMembersAsync();
        }


        public Task<Member> GetMember(System.Guid entityId, CancellationToken ct = default)
        {
            var member = _unitofWork.MemberData.GetAllMembers().Where(s => s.MemberId == entityId).SingleOrDefault();
            return Task.FromResult(member);

        }

        public async Task<Member> UpdateMember(Member entity, CancellationToken ct = default)
        {
            var updatedMember = await _unitofWork.MemberData.UpdateMember(entity);
            _unitofWork.Complete();
            return updatedMember;
        }
        public async Task<Member> Login(string email, string password, CancellationToken ct = default)
        {
            var loginMember = await _unitofWork.MemberData.GetAllMembersAsync();
            var successmember = loginMember.Where(c => c.Email == email
                        && c.IsEmailConfirmed == true
                        && c.Password == password
                        && c.MemberStatus == 1).SingleOrDefault();
            return successmember;
        }

    }
}