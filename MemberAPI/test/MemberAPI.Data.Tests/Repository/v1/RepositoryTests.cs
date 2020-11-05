using System;
using System.Linq;
using MemberAPI.Data.Database;
using MemberAPI.Domain.Entities;
using MemberAPI.Data.Repository.v1;
using MemberAPI.Data.Tests.Infrastructure;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace MemberAPI.Data.Tests.Repository.v1
{
    public class RepositoryTests : DatabaseTestBase
    {
        private readonly MemberContext _MemberContext;
        private readonly Repository<Member> _testee;
        private readonly Repository<Member> _testeeFake;
        private readonly Member _newMember;

        public RepositoryTests()
        {
            _MemberContext = A.Fake<MemberContext>();
            _testeeFake = new Repository<Member>(_MemberContext);
            _testee = new Repository<Member>(Context);
            _newMember = new Member
                {
                    MemberId=new Guid("76460f4b-93fe-4413-824f-33ad1877ffff"), 
                    FirstName ="Leela",
                    LastName="John",
                    DOB=new DateTime(1992,8,12),
                    Gender="Female",
                    Email="leela@gmail.com",
                    Nationality="germany",
                    MobileNo="0552730365",
                    CountryId=1,StateId=1,
                    Username="leela",
                    Password="leelajohn",
                    EntryBy="IT",
                    EntryDate=new DateTime(2020,10,23),
                    PaymentCategory=1,
                    MemberStatus=1,
                    IsEmailConfirmed=true,
                    EmailConfirmationToken=null,
                    ForgotPasswordConfirmationToken=null
                };
        }

        [Theory]
        [InlineData("Changed")]
        public async void UpdateCustomerAsync_WhenCustomerIsNotNull_ShouldReturnCustomer(string firstName)
        {
            var member = Context.Member.First();
            member.FirstName = firstName;

            var result = await _testee.UpdateAsync(member);

            result.Should().BeOfType<Member>();
            result.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void AddAsync_WhenEntityIsNull_ThrowsException()
        {
            _testee.Invoking(x => x.AddAsync(null)).Should().Throw<ArgumentNullException>();
        }

      

        
    }
}