using MemberAPI.MockData.Repository.v1;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MemberAPI.Tests.Master.v1
{
    public class MasterMemberOperationTest
    {
        private readonly FakeUnitofWork _fakeUnitofWork;
        public MasterMemberOperationTest()
        {
            _fakeUnitofWork = new FakeUnitofWork();           
        }
        [Fact]
        public void GetAllMembers()
        {
            var memberinfo=_fakeUnitofWork.MemberData.GetAllMembers();
            Assert.Equal(3, memberinfo.Count());
        }
    }
}
