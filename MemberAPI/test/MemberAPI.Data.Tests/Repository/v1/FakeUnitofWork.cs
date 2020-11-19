using System;
using System.Linq;
using System.Threading.Tasks;
using MemberAPI.Data.Database;

namespace MemberAPI.Data.Tests.Repository.v1
{

    public class FakeUnitofWork :IFakeUnitofWork
    {
        private readonly MemberContext _context;
        public IFakeMemberRepository MemberData { get; }

        public UnitofWork(MemberContext memberContext)
        {
            _context = memberContext;            
            MemberData = new FakeMemberRepository(_context);;           
        }
        public int Complete()
        {
            return _context.SaveChanges();
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
                _context.Dispose();
            }
        }
    }
}