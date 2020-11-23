using MemberAPI.Data.Repository.v1;
using MemberAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemberAPI.MockData.Repository.v1
{
    public class FakeUnitofWork : IFakeUnitofWork
    {
        //private  List<Member> _context;
        public IFakeMemberRepository MemberData { get; }

        public FakeUnitofWork()
        {
           // _context = memberContext;
            MemberData = new FakeMemberRepository();
        }
        public int Complete()
        {
            return 1;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                return;
            }
        }
    }
}
