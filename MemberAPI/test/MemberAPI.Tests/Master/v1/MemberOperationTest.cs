using FakeItEasy;
using FluentAssertions;
using MemberAPI.Data.Database;
using MemberAPI.Data.Repository.v1;
using MemberAPI.Data.Security.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.Service.Master.v1;
using MemberAPI.Service.Plugins.v1;
using MemberAPI.Tests.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MemberAPI.Tests.Master.v1
{
    public class MemberOperationTest: DatabaseTestBase
    {
        private readonly ServiceMaster _testee;
        private UnitofWork _unitofWork;     
        private readonly IEmailSender _emailSender;      
        private readonly IDataProtector protectorEmailConfirm;
        private readonly IDataProtector protectorForgotPassword;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly DataProtectionPurposeStrings _dataProtectionPurposeStrings;
        private readonly Member _newMember;

        public MemberOperationTest()
        {           
            _unitofWork = new UnitofWork(Context);           
            _dataProtectionProvider = A.Fake<IDataProtectionProvider>();
            _emailSender= A.Fake<IEmailSender>();
            _dataProtectionPurposeStrings = A.Fake<DataProtectionPurposeStrings>();
            //protectorEmailConfirm= _dataProtectionProvider.CreateProtector(_dataProtectionPurposeStrings.MemberEmailConfirmationValue);
            _testee = new ServiceMaster(_unitofWork,
                _dataProtectionProvider,
                _emailSender,
                _dataProtectionPurposeStrings);
          
            _newMember = new Member
            {
                MemberId = new Guid("521d19c9-b94e-4483-a394-adcdf6da4c20"),
                FirstName = "Hanna",
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
            };
        }

        [Fact]
        public async void Handle_ShouldCallAddAsync()
        {
           // _testee.protectorEmailConfirm = "";
            var member=await _testee.AddMemberAsync(_newMember, "http://www.testdomain.com");

            member.Should().BeOfType<Member>();

            member.FirstName.Should().Be("Hanna");
        }
    }
}
