using AutoMapper;
using MemberAPI.Domain.Entities;
using MemberAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
            CreateMap<CreateMemberModel, Member>()
                .ForMember(x => x.MemberId, opt => opt.Ignore())
                .ForMember(x => x.EntryBy, opt => opt.Ignore())
                .ForMember(x => x.IsEmailConfirmed, opt => opt.Ignore())
                .ForMember(x => x.EmailConfirmationToken, opt => opt.Ignore())
                .ForMember(x => x.ForgotPasswordConfirmationToken, opt => opt.Ignore())
                .ForMember(x => x.EntryDate, opt => opt.Ignore())
                .ForMember(x => x.PaymentCategory, opt => opt.Ignore())
                .ForMember(x => x.MemberStatus, opt => opt.Ignore())
                .ForMember(x => x.DOB, c => c.MapFrom(d => d.DOB))
                .ForMember(x => x.EntryDate, c => c.MapFrom(d => System.DateTime.Now))
                .ForMember(x => x.MemberStatus, c => c.MapFrom(d => 1))
                .ForMember(x => x.PaymentCategory, c => c.MapFrom(d => 1))
                .ForMember(x => x.Username, c => c.MapFrom(d => d.Email))
                .ForMember(x => x.IsEmailConfirmed, c => c.MapFrom(d =>false));


            CreateMap<UpdateMemberModel, Member>()
                .ForMember(x => x.EntryBy, opt => opt.Ignore())
                .ForMember(x => x.EntryDate, opt => opt.Ignore())               
                .ForMember(x => x.Password, opt => opt.Ignore())
                .ForMember(x => x.Email, opt => opt.Ignore())
                .ForMember(x => x.EmailConfirmationToken, opt => opt.Ignore())
                .ForMember(x => x.ForgotPasswordConfirmationToken, opt => opt.Ignore())
                .ForMember(x => x.FirstName, opt => opt.Condition(s => !string.IsNullOrEmpty(s.FirstName)))
                .ForMember(x => x.LastName, opt => opt.Condition(s => !string.IsNullOrEmpty(s.LastName)))
                .ForMember(x => x.DOB, opt => opt.Condition(s => !string.IsNullOrEmpty(s.DOB)))
                .ForMember(x => x.Gender, opt => opt.Condition(s => !string.IsNullOrEmpty(s.Gender)))
                .ForMember(x => x.Nationality, opt => opt.Condition(s => !string.IsNullOrEmpty(s.Nationality)))
                .ForMember(x => x.MobileNo, opt => opt.Condition(s => !string.IsNullOrEmpty(s.MobileNo)))
                .ForMember(x => x.CountryId, opt => opt.Condition(s => s.CountryId.HasValue))
                .ForMember(x => x.StateId, opt => opt.Condition(s => s.StateId.HasValue))
                .ForMember(x => x.PaymentCategory, opt => opt.Condition(s => s.PaymentCategory.HasValue))
                .ForMember(x => x.MemberStatus, opt => opt.Condition(s => s.MemberStatus.HasValue))
                .ForMember(x => x.DOB, c => c.MapFrom(d => d.DOB))
                //.ForMember(x => x.IsEmailConfirmed, opt => opt.Condition(s => s.IsEmailConfirmed.HasValue))
                   ;

            CreateMap<Member, ViewMemberModel>();             
              

        }
    }
}
