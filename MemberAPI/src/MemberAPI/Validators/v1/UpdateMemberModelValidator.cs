using FluentValidation;
using MemberAPI.Domain.Entities;
using MemberAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Validators.v1
{
    public class UpdateMemberModelValidator : AbstractValidator<UpdateMemberModel>
    {
        public UpdateMemberModelValidator()
        {
            RuleFor(x => x.MemberId)
            .NotNull()
            .WithMessage("MemberId Cannot be Empty");

            RuleFor(x => x.FirstName)
            .MinimumLength(2).
            WithMessage("The first name must be at least 2 character long");

            RuleFor(x => x.LastName)
            .MinimumLength(2)
            .WithMessage("The last name must be at least 2 character long");

            RuleFor(x => x.Gender)
            .IsEnumName(typeof(Gender))
            .WithMessage("Gender Should be one among this :" + string.Join(",", Enum.GetNames(typeof(Gender))));




        }
    }
}
