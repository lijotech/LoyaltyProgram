using AutoMapper;
using MemberAPI.Data.Repository.v1;
using MemberAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Validators.v1
{
    /// <summary>
    /// This class is not used
    /// </summary>
    public class UniqueEmailIdValidator : ValidationAttribute
    {
        private readonly IMapper mapper;
        private readonly IRepository<Member> repository;

        public UniqueEmailIdValidator(IMapper mapper, IRepository<Member> repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public override bool IsValid(object value)
        {
           return !repository.GetAll().Any(c => c.Email == value.ToString().Trim());
        }
    }
}
