using MemberAPI.Data.Repository.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.MockData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberAPI.MockData.Repository.v1
{
    public class FakeMemberRepository :  IFakeMemberRepository
    {
        public List<Member> _memberContext;

        public FakeMemberRepository()
        {
            _memberContext = new List<Member>();
               var members = new[]
               {
                new Member
                {
                    MemberId=new Guid("0e240956-b588-4e5b-b51f-a67b6597dad9"),
                    FirstName ="John",
                    LastName="Smith",
                    DOB=new DateTime(1966,5,21),
                    Gender="Male",
                    Email="john@gmail.com",
                    Nationality="Italy",
                    MobileNo="0552210365",
                    CountryId=1,StateId=1,
                    Username="john",
                    Password="johnsmith",
                    EntryBy="IT",
                    EntryDate=new DateTime(2020,10,30),
                    PaymentCategory=1,
                    MemberStatus=1,
                    IsEmailConfirmed=true,
                    EmailConfirmationToken=null,
                    ForgotPasswordConfirmationToken=null
                },
                new Member
                {
                    MemberId=new Guid("f8bb6d71-8c7f-463d-b5ae-1f83625b7ef1"),
                    FirstName ="Mathew",
                    LastName="Philip",
                    DOB=new DateTime(1980,4,12),
                    Gender="Male",
                    Email="mathew@gmail.com",
                    Nationality="France",
                    MobileNo="0554555365",
                    CountryId=1,
                    StateId=2,
                    Username="mathew",
                    Password="mathewphilip",
                    EntryBy="IT",
                    EntryDate=new DateTime(2020,10,31),
                    PaymentCategory=1,
                    MemberStatus=1,
                    IsEmailConfirmed=false,
                    EmailConfirmationToken=null,
                    ForgotPasswordConfirmationToken=null
                },
                new Member
                {
                    MemberId=new Guid("c4251991-29f0-4e18-9551-a1c0b6528fdf"),
                    FirstName ="Jessy",
                    LastName="Joy",
                    DOB=new DateTime(1992,3,18),
                    Gender="Female",
                    Email="jessy@gmail.com",
                    Nationality="India",
                    MobileNo="0557855365",
                    CountryId=1,
                    StateId=3,
                    Username="jessy",
                    Password="jessyjoy",
                    EntryBy="IT",
                    EntryDate=new DateTime(2020,10,22),
                    PaymentCategory=1,
                    MemberStatus=1,
                    IsEmailConfirmed=true,
                    EmailConfirmationToken=null,
                    ForgotPasswordConfirmationToken=null
                }
            };

            _memberContext.AddRange(members);            
        }

        public  Task<Member> AddMemberAsync(Member entity)
        {
            _memberContext.Add(entity);
            return entity.AsTask();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberContext.AsQueryable();
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _memberContext.AsQueryable().AsTask();
        }

        public  Task<Member> GetMember(Guid entityId)
        {
            var entry = _memberContext.Where(s => s.MemberId == entityId).SingleOrDefault();
            return entry.AsTask();
        }

        public  Task<Member> UpdateMember(Member entity)
        {
            var entry = _memberContext.Where(s => s == entity).SingleOrDefault();
            entry = entity;
            return entity.AsTask();
        }
    }
}
