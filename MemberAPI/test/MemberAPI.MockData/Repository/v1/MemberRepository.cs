using MemberAPI.Data.Repository.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.MockData.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberAPI.MockData.Repository.v1
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public Task<Member> AddMemberAsync(Member entity)
        {
            entity.MemberId = new Guid();
            return entity.AsTask();
        }

        public List<Member> GetAllMembers()
        => new Member
        {
            MemberId = new Guid("76460f4b-93fe-4413-824f-33ad1877ffff"),
            FirstName = "Leela",
            LastName = "John",
            DOB = new DateTime(1992, 8, 12),
            Gender = "Female",
            Email = "leela@gmail.com",
            Nationality = "germany",
            MobileNo = "0552730365",
            CountryId = 1,
            StateId = 1,
            Username = "leela",
            Password = "leelajohn",
            EntryBy = "IT",
            EntryDate = new DateTime(2020, 10, 23),
            PaymentCategory = 1,
            MemberStatus = 1,
            IsEmailConfirmed = true,
            EmailConfirmationToken = null,
            ForgotPasswordConfirmationToken = null
        }.AsList();



        public  Task<Member> GetMember(Guid entityId)
        => new Member
        {
            MemberId =  entityId,
            FirstName = "Leela",
            LastName = "John",
            DOB = new DateTime(1992, 8, 12),
            Gender = "Female",
            Email = "leela@gmail.com",
            Nationality = "germany",
            MobileNo = "0552730365",
            CountryId = 1,
            StateId = 1,
            Username = "leela",
            Password = "leelajohn",
            EntryBy = "IT",
            EntryDate = new DateTime(2020, 10, 23),
            PaymentCategory = 1,
            MemberStatus = 1,
            IsEmailConfirmed = true,
            EmailConfirmationToken = null,
            ForgotPasswordConfirmationToken = null
        }.AsTask();

        public  Task<Member> UpdateMember(Member entity) => Task.FromResult(entity);
    }
}
