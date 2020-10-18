using AutoMapper;
using MemberAPI.Data.Repository.v1;
using MemberAPI.Domain.Entities;
using MemberAPI.Models.v1;
using MemberAPI.Security.v1;
using MemberAPI.Service.v1;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Controllers.v1
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMapper _mapper;

        public IRepository<Member> _repository { get; }
        private readonly IEmailSender _emailSender;
        private readonly IDataProtector protector;
        private readonly IDataProtector protectorForgotPassword;

        public MemberController(IMapper mapper,
            IRepository<Member> repository,
            IDataProtectionProvider dataProtectionProvider,
            IEmailSender emailSender,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _mapper = mapper;
            _repository = repository;
            _emailSender = emailSender;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberEmailConfirmationValue);
            protectorForgotPassword = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.MemberForgotPasswordTokenValue);
        }


        /// <summary>
        ///     Action to retrieve all members with Completed Registrations.
        /// </summary>
        /// <returns>Returns the created members</returns>
        /// <response code="200">Returned if the list of Members was retrieved</response>
        /// <response code="400">Returned if the Members could not be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public ActionResult<List<ViewMemberModel>> MemberOperation()
        {
            try
            {
                var members = _repository.GetAll().ToList();
                return _mapper.Map<List<ViewMemberModel>>(members);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        ///     Action to retrieve one member.
        /// </summary>
        /// <returns>Returns a single member</returns>
        /// <response code="200">Returned if the member information is retrieved</response>
        /// <response code="400">Returned if the member information cannot be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{entityId}")]
        public async Task<ActionResult<ViewMemberModel>> MemberOperation(Guid entityId)
        {
            try
            {

                var member = await _repository.GetItem(entityId);
                return _mapper.Map<ViewMemberModel>(member);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Action to create a new member in the database.
        /// </summary>
        /// <param name="createMemberModel">Model to create a new member</param>
        /// <returns>Returns the created customer</returns>
        /// <response code="200">Returned if the member was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or the member couldn't be saved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost]
        public async Task<ActionResult<ViewMemberModel>> MemberOperation([FromBody] CreateMemberModel createMemberModel)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                bool memberEmailCheck = _repository.GetAll().Any(c => c.Email == createMemberModel.Email);
                if (memberEmailCheck)
                    return BadRequest($"Member Already Registered with Email Id {createMemberModel.Email}");


                var member = _mapper.Map<Member>(createMemberModel);
                member.EmailConfirmationToken = protector.Protect(string.Format("{0}{1}", member.Username, System.DateTime.Now.ToString()));
                var addedMember = await _repository.AddAsync(member);

                if (addedMember != null)
                {
                    // throw new Exception("deliberate");
                    var confirmationLink = Url.Action("ConfirmEmail", "Member",
                                        new
                                        {
                                            u = protector.Protect(member.Username),
                                            t = member.EmailConfirmationToken
                                        }, Request.Scheme);
                    var message = new Message(new string[] { "lijotech@gmail.com" },
                        "Email Confirmation", "Click this link to Continue:" + confirmationLink, null);
                    _emailSender.SendEmail(message);
                }
                else
                {
                    return BadRequest("Member not added.");
                }

                int f = await _repository.SaveChangesAsync();
                return _mapper.Map<ViewMemberModel>(addedMember);
            }
            catch (Exception ex)
            {
                _repository.Rollback();
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Action to update an existing Member
        /// </summary>
        /// <param name="updateMemberModel">Model to update an existing Member</param>
        /// <returns>Returns the updated Member</returns>
        /// /// <response code="200">Returned if the Member was updated</response>
        /// /// <response code="400">Returned if the model couldn't be parsed or the Member couldn't be found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPut]
        public async Task<ActionResult<ViewMemberModel>> MemberOperation([FromBody] UpdateMemberModel updateMemberModel)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                var member = await _repository.GetItem(updateMemberModel.MemberId);

                if (member == null)
                {
                    return BadRequest($"No Member found with the id {updateMemberModel.MemberId}");
                }               

                var updatedMember = _repository.Update(_mapper.Map(updateMemberModel, member));
                int f = await _repository.SaveChangesAsync();
                return _mapper.Map<ViewMemberModel>(updatedMember);

            }
            catch (Exception ex)
            {
                _repository.Rollback();
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Action to Confirm Email After Registration.
        /// </summary>
        /// <param name="ConfirmEmailModel">Model to Confirm Email</param>
        /// <returns>Returns the confirmed Member</returns>
        /// /// <response code="200">Returned if the member was created</response>
        /// /// <response code="400">Returned if the model couldn't be parsed or the member couldn't be saved</response>
        /// /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string u, [FromQuery] string t)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                if (u == null || t == null)
                {
                    return BadRequest("Broken Link");
                }
                string g = protector.Unprotect(u);
                var memberEmailConfirmCheck = _repository.GetAll()
                    .Where(c => c.Email == protector.Unprotect(u)
                    && c.EmailConfirmationToken == t).SingleOrDefault();
                if (memberEmailConfirmCheck != null)
                {
                    UpdateMemberModel updateMember = new UpdateMemberModel
                    {
                        MemberId = memberEmailConfirmCheck.MemberId,
                        IsEmailConfirmed = true
                    };
                    var updatedMember = _repository.Update(_mapper.Map(updateMember, memberEmailConfirmCheck));
                    int f = await _repository.SaveChangesAsync();
                    return "Email Confirmed.";
                }

                return BadRequest("Invalid Request");
            }
            catch (Exception ex)
            {
                _repository.Rollback();
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Action to Confirm Email After Registration.
        /// </summary>
        /// <param name="ConfirmEmailModel">Model to Confirm Email</param>
        /// <returns>Returns the confirmed Member</returns>
        /// /// <response code="200">Returned if the member was created</response>
        /// /// <response code="400">Returned if the model couldn't be parsed or the member couldn't be saved</response>
        /// /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] ForgotPasswordModel e)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                if (e == null)
                {
                    return BadRequest("Email is not supplied");
                }
                //string g = protector.Unprotect(e);
                var forgotPasswordUser = _repository.GetAll()
                    .Where(c => c.Email == e.Email && c.IsEmailConfirmed == true).SingleOrDefault();
                if (forgotPasswordUser != null)
                {
                    UpdateMemberModel updateMember = new UpdateMemberModel
                    {
                        MemberId = forgotPasswordUser.MemberId
                    };

                    var memberToUpdate = _mapper.Map(updateMember, forgotPasswordUser);
                    memberToUpdate.ForgotPasswordConfirmationToken = protectorForgotPassword
                        .Protect(string.Format("{0}|{1}", forgotPasswordUser.MemberId, System.DateTime.Now.ToString()));
                    var updatedMember = _repository.Update(memberToUpdate);

                    if (updatedMember != null)
                    {
                        var PasswordResetLink = Url.Action("ResetPassword", "Member",
                                        new
                                        {
                                            u = protectorForgotPassword.Protect(updatedMember.Email),
                                            t = memberToUpdate.ForgotPasswordConfirmationToken
                                        }, Request.Scheme);


                        var message = new Message(new string[] { "lijotech@gmail.com" },
                        "Reset Password", "Click this link to Continue:" + PasswordResetLink, null);
                        _emailSender.SendEmail(message);
                    }
                    int f = await _repository.SaveChangesAsync();
                    return "Forgot Password Reset Link Sent to Registered Email.";
                }
                return BadRequest("Invalid Request");
            }
            catch (Exception ex)
            {
                _repository.Rollback();
                return BadRequest(ex.Message);
            }
        }
        [Route("ResetPassword")]
        [HttpGet]
        public ActionResult<string> ResetPassword([FromQuery] string u, [FromQuery] string t)
        {
            return BadRequest("Invalid Request");
        }

        [Route("ResetPassword")]
        [HttpPost]
        public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                string g = protectorForgotPassword.Unprotect(resetPassword.Email);
                var forgotPasswordUser = _repository.GetAll()
                    .Where(c => c.Email == g && c.IsEmailConfirmed == true
                    && c.ForgotPasswordConfirmationToken == resetPassword.Token).SingleOrDefault();
                if (forgotPasswordUser != null)
                {
                    UpdateMemberModel updateMember = new UpdateMemberModel
                    {
                        MemberId = forgotPasswordUser.MemberId
                    };
                    var memberToUpdate = _mapper.Map(updateMember, forgotPasswordUser);
                    memberToUpdate.Password = resetPassword.Password;
                    memberToUpdate.ForgotPasswordConfirmationToken = null;
                    var updatedMember = _repository.Update(memberToUpdate);

                    int f = await _repository.SaveChangesAsync();
                    if (updatedMember != null)
                        return "Password Reset Successfully.";
                    else
                        return "Password Reset Failed.";
                }
                return BadRequest("Invalid Request");
            }
            catch (Exception ex)
            {
                _repository.Rollback();
                return BadRequest(ex.Message);
            }
        }

    }
}
