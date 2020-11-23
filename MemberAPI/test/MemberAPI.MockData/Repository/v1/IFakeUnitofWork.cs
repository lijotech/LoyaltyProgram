using System;
using System.Collections.Generic;
using System.Text;

namespace MemberAPI.MockData.Repository.v1
{
    
    public interface IFakeUnitofWork : IDisposable
    {
        IFakeMemberRepository MemberData { get; }

        int Complete();
    }
}
