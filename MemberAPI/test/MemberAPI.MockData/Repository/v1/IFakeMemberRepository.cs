using System;
using System.Linq;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;
using System.Collections.Generic;
using MemberAPI.Data.Repository.v1;

namespace MemberAPI.MockData.Repository.v1
{
    public interface IFakeMemberRepository 
    {
        IEnumerable<Member> GetAllMembers();
        Task<IEnumerable<Member>> GetAllMembersAsync();

        Task<Member> AddMemberAsync(Member entity);

        Task<Member> UpdateMember(Member entity);

        Task<Member> GetMember(Guid entityId);
    }
}
