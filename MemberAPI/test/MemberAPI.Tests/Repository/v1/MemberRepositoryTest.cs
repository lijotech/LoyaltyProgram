using MemberAPI.Domain.Entities;
using MemberAPI.MockData.Repository.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MemberAPI.Tests.Repository.v1
{
    public class MemberRepositoryTest
    {
        private readonly FakeMemberRepository _repo;
        public MemberRepositoryTest()
        {
            _repo = new FakeMemberRepository();
        }

        [Fact]
        public  void GetAllMember_CheckIfMemberDataIsNotEmpty()
        {            
            var members =  _repo.GetAllMembers();            
            Assert.Equal(3, members.Count());
            Assert.IsAssignableFrom<IEnumerable< Member>>(members);
        }
        [Fact]
        public void GetAllMember_CheckWhenMemberDataIsEmpty()
        {           
            _repo._memberContext.Clear();           
            var members = _repo.GetAllMembers();           
            Assert.Empty(members);
        }
    }
}
