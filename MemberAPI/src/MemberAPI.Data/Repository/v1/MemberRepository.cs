using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberAPI.Data.Database;
using MemberAPI.Domain.Entities;

namespace MemberAPI.Data.Repository.v1
{
    public class MemberRepository :Repository<Member>, IMemberRepository
    {
        private readonly MemberContext _memberContext;
        public MemberRepository(MemberContext context): base(context)
        {
             _memberContext = context;
        }

        public async Task<Member> AddMemberAsync(Member entity)
        {
             await _memberContext.Member.AddAsync(entity);
             return entity;
        }

        public IEnumerable<Member> GetAllMembers()
        {
           return _memberContext.Member.ToList();
        }

        public async Task<Member> GetMember(Guid entityId)
        {
            return await _memberContext.Member.FindAsync(entityId);
        }

        public async Task<Member> UpdateMember(Member entity)
        {
            var member = await _memberContext.Member.FindAsync(entity.MemberId);
            this._memberContext.Entry(member).CurrentValues.SetValues(entity);
            return await _memberContext.Member.FindAsync(entity.MemberId);
        }
    }
}