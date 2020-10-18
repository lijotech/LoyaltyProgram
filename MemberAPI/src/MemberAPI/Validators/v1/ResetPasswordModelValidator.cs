using FluentValidation;
using MemberAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Validators.v1
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(x => x.Email)
             .NotNull()
             .WithMessage("The Email must not be Empty");

            RuleFor(x => x.Token)
            .NotNull()
            .WithMessage("The Token must not be Empty");

            RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("The Password must not be Empty");

            RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .WithMessage("The ConfirmPassword must not be Empty");


            RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("The Password and ConfirmPassword should match");

            RuleFor(x => x.Password)
             .MinimumLength(8)
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
