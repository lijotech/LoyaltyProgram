using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberAPI.Data.Database;
using MemberAPI.Domain.Entities;

namespace MemberAPI.Data.Tests.Repository.v1
{
    public class FakeMemberRepository :Repository<Member>, IFakeMemberRepository
    {
        private readonly DatabaseTestBase _memberContext;
        public MemberRepository(DatabaseTestBase context): base(context)
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