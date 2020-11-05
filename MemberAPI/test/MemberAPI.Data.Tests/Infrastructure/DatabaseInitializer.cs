using System;
using System.Linq;
using MemberAPI.Data.Database;
using MemberAPI.Domain.Entities;

namespace MemberAPI.Data.Tests.Infrastructure
{
    public class DatabaseInitializer
    {
        public static void Initialize(MemberContext context)
        {
            if (context.Member.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(MemberContext context)
        {
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

            context.Member.AddRange(members);
            context.SaveChanges();
        }
    }
}