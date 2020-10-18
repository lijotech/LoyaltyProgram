using FluentValidation;
using MemberAPI.Domain.Entities;
using MemberAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Validators.v1
{
    public class CreateMemberModelValidator : AbstractValidator<CreateMemberModel>
    {
        public CreateMemberModelValidator()
        {
            RuleFor(x => x.FirstName)
              .NotNull()
              .WithMessage("The first name must not be Empty")            
                .MinimumLength(2).
                WithMessage("The first name must be at least 2 character long");

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("The last name must not be Empty")            
                .MinimumLength(2)
                .WithMessage("The last name must be at least 2 character long");


            //RuleFor(x => x.DOB)
            //    .InclusiveBetween(DateTime.Now.AddYears(-150).Date, DateTime.Now)
            //    .WithMessage("The Date of Birth must not be longer ago than 150 years and can not be in the future");

            RuleFor(x => x.Gender)
                           .NotNull()
                           .WithMessage("The Gender must not be Empty");
            RuleFor(x => x.Gender)
                .IsEnumName(typeof(Gender))
                .WithMessage("Gender Should be one among this :"+ string.Join(",", Enum.GetNames(typeof(Gender))));

            RuleFor(x => x.Email)
                           .NotNull()
                           .WithMessage("The Email must not be Empty");
            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.MobileNo)
                           .NotNull()
                           .WithMessage("The MobileNo must not be Empty");

       
            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("The Password must not be Empty");
            RuleFor(x => x.Password)
              .MinimumLength (8)
              .WithMessage("The Password must be at least 8 character long");
            RuleFor(x => x.Password)
             .MaximumLength(20)
             .WithMessage("The Password must be less than 20 character long");

            RuleFor(x => x.Password)
             .Must((v) =>  
                (v.Any(char.IsUpper) && v.Any(char.IsUpper) && v.Any(char.IsDigit))
             )
             .WithMessage("The Password must contain atleast one Uppercase, one Lowercase and one Digit.");

        }
    }
}
