using MemberAPI.Data.Database;
using MemberAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberAPI.Tests.Infrastructure
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
                    MemberId = Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f"),
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
                },
                new Member
                {
                    MemberId = Guid.Parse("1c5ef6b3-bc48-4063-8247-adfa8f3de9e9"),
                    FirstName ="MAthew",
                    LastName="John",
                    DOB=new DateTime(1985,7,13),
                    Gender="Male",
                    Email="mathew@gmail.com",
                    Nationality="germany",
                    MobileNo="0552730345",
                    CountryId=1,StateId=1,
                    Username="mathew",
                    Password="leelajohn",
                    EntryBy="IT",
                    EntryDate=new DateTime(2020,10,24),
                    PaymentCategory=1,
                    MemberStatus=1,
                    IsEmailConfirmed=true,
                    EmailConfirmationToken=null,
                    ForgotPasswordConfirmationToken=null
                },
                new Member
                {
                    MemberId = Guid.Parse("a435bf9b-7852-4daf-be2e-36bf39bd5a47"),
                    FirstName ="Rehu",
                    LastName="John",
                    DOB=new DateTime(1990,3,23),
                    Gender="Male",
                    Email="rehu@gmail.com",
                    Nationality="germany",
                    MobileNo="0552730765",
                    CountryId=1,StateId=1,
                    Username="rehu",
                    Password="leelajohn",
                    EntryBy="IT",
                    EntryDate=new DateTime(2020,10,23),
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
