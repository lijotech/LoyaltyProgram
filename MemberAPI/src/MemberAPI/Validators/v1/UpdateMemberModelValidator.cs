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
              


        


        }
    }
}
