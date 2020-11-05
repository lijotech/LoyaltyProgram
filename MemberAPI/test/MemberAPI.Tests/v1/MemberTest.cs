using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;
using MemberAPI.Security.v1;
using MemberAPI.Service.v1;
using Microsoft.AspNetCore.DataProtection;
using MemberAPI.Data.Repository.v1;
using MemberAPI.Controllers.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.Models.v1;
using FakeItEasy;
using FluentAssertions;
using System.Threading.Tasks;

namespace MemberAPI.Tests
{
    public class MemberTest
    {
        private readonly MemberController _testee;
        private readonly CreateMemberModel _createMemberModel;
        private readonly IMapper _mapper;
        public IRepository<Member> _repository { get; }
        
        private IUnitofWork _unitofWork;
        private readonly IEmailSender _emailSender;
       // private  IDataProtectionProvider _dataProtectionProvider;      
        private readonly IDataProtector protector;
  //private  DataProtectionPurposeStrings _dataProtectionPurposeStrings; 
        private readonly IDataProtector protectorForgotPassword;
        private readonly Guid _memberid = Guid.Parse("5224ed94-6d9c-42ec-ba93-dfb11fe68931");
        public MemberTest()
        {
            var dataProtectionProvider = A.Fake<IDataProtectionProvider>();
             var dataProtectionPurposeStrings = A.Fake<DataProtectionPurposeStrings>();           
             protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberEmailConfirmationValue);
             protectorForgotPassword = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberForgotPasswordTokenValue);   
            _testee = new MemberController(_mapper, _unitofWork,
            dataProtectionProvider,_emailSender,dataProtectionPurposeStrings);
            _createMemberModel = new CreateMemberModel
            {
                    FirstName ="Leela",
                    LastName="John",
                    DOB="12-Dec-1992",
                    Gender="Female",
                    Email="leela@gmail.com",
                    Nationality="germany",
                    MobileNo="0552730365",
                    CountryId=1,StateId=1,                    
                    Password="Leela2020"                                   
            };
            var member = new Member
            {
                    MemberId = _memberid,
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

            // A.CallTo(() => _mapper.Map<Member>(A<Member>._)).Returns(member);
        }

          [Fact]
        public async void Post_ShouldReturnCustomer()
        {
            var result = await _testee.MemberOperation(_createMemberModel);

             (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
             result.Value.Should().BeOfType<ViewMemberModel>();
             result.Value.MemberId.Should().Be(_memberid);
        }
        /*[Fact]
        public void GetReturnsProduct()
        {
            // Arrange
            var controller = new ProductsController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(10);

            // Assert
            Product product;
            Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            Assert.AreEqual(10, product.Id);
        }
        */

       
    }
}
