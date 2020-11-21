using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;

namespace MemberAPI.Service.Master.v1
{
    public interface IServiceMaster
    {      
        IEnumerable<Member> GetAllMembers(CancellationToken ct = default);
        Task<Member> AddMemberAsync(Member entity, string confirmationLinkbase,CancellationToken ct = default);
        Task<Member> UpdateMember(Member entity,CancellationToken ct = default); 
        Task<Member> GetMember(System.Guid entityId,CancellationToken ct = default);
        Task<Member> ConfirmEmail(string username, string token, CancellationToken ct = default);
        Task<Member> ForgotPassword(string email, string forgotPasswordLinkbase, CancellationToken ct = default);
        Task<Member> ResetPassword(string email, string token, string newPassword, CancellationToken ct = default);
    }

}