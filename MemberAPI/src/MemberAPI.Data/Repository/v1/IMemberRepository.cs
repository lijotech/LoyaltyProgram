using System;
using System.Linq;
using System.Threading.Tasks;
using MemberAPI.Domain.Entities;
using System.Collections.Generic;

namespace MemberAPI.Data.Repository.v1
{
    public interface IMemberRepository: IRepository<Member>
    {
         IEnumerable<Member> GetAllMembers();
         Task<Member> AddMemberAsync(Member entity);
        
         Task<Member> UpdateMember(Member entity); 

         Task<Member> GetMember(Guid entityId);
    }

}