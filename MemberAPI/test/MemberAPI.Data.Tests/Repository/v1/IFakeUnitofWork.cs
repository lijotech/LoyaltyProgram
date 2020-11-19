using System;
using MemberAPI.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Data.Tests.Repository.v1{
    public interface IFakeUnitofWork:IDisposable
    {
        IFakeMemberRepository  MemberData {get;}

        int Complete();
    }
    
}