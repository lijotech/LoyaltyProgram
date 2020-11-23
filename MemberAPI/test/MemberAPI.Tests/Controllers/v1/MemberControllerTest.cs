using System;
using Xunit;

using System.Net;

using MemberAPI.Data.Repository.v1;

using MemberAPI.Domain.Entities;

using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;

namespace MemberAPI.Tests.Controllers.v1
{
    public class MemberControllerTest
    {
        /*private readonly MemberController _testee;
        private readonly CreateMemberModel _createMemberModel;
        private readonly IRepository<Member> _MemberRepository;
        private IUnitofWork _unitofWork;
        private IMapper _mapper;      
        private readonly IDataProtector protector;
        private readonly IDataProtector protectorForgotPassword;
        private readonly Guid _memberid = Guid.Parse("5224ed94-6d9c-42ec-ba93-dfb11fe68931");
        public MemberControllerTest()
        {
            var dataProtectionProvider = A.Fake<IDataProtectionProvider>();
            var dataProtectionPurposeStrings = A.Fake<DataProtectionPurposeStrings>();           
            protector = dataProtectionProvider
                    .CreateProtector(dataProtectionPurposeStrings.MemberEmailConfirmationValue);
            protectorForgotPassword = dataProtectionProvider
                    .CreateProtector(dataProtectionPurposeStrings.MemberForgotPasswordTokenValue);   
            _unitofWork=A.Fake<IUnitofWork>();
            _mapper=A.Fake<IMapper>();
            _MemberRepository=A.Fake<IRepository<Member>>();
            // var mockRepo = new Mock<IMemberRepository>();

            _testee = new MemberController(
                _mapper,
                _unitofWork,
                dataProtectionProvider,
                A.Fake<IEmailSender>(),
                dataProtectionPurposeStrings);

            _testee.ControllerContext = new ControllerContext();

         

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
            A.CallTo(() => _mapper.Map<Member>(A<Member>._)).Returns(member);
        }

        /*[Fact]
        public async void Post_ShouldReturnCustomer()
        {
            var result = await _testee.MemberOperation(_createMemberModel);

             (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
             result.Value.Should().BeOfType<ViewMemberModel>();
             result.Value.MemberId.Should().Be(_memberid);
        }
        */

        /*[Fact]
        public  void Members_ShouldReturnListOfMembers()
        {
            // var mockSet = new Mock<DbSet<Member>>();

            // var mockContext = new Mock<Member>();
            //mockContext.Setup(m => m.Blogs).Returns(mockSet.Object);

            var result =  _testee.MemberOperation();

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            //result.Value.Should().BeOfType<List<ViewMemberModel>>();
            //result.Value.Count.Should().Be(1);
            Assert.Equal(4,4);
        }
        */
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
