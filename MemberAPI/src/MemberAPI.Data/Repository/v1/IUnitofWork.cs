using System;
using MemberAPI.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Data.Repository.v1{
    public interface IUnitofWork:IDisposable
    {
        IMemberRepository  MemberData {get;}

        int Complete();
    }
    
}