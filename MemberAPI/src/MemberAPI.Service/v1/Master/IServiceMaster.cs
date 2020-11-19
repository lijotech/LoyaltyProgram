using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;

namespace MemberAPI.Service.v1.Master
{
    public interface IServiceMaster
    {      
        IEnumerable<Member> GetAllMembers(CancellationToken ct = default);
        Task<Member> AddMemberAsync(Member entity, string confirmationLinkbase,CancellationToken ct = default);
        Task<Member> UpdateMember(Member entity,CancellationToken ct = default); 
        Task<Member> GetMember(System.Guid entityId,CancellationToken ct = default);
    }

}