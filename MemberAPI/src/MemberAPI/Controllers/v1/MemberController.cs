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
        /// <response code="401">Returned if Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK) ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <response code="401">Returned if Unauthorized</response>
        /// <response code="404">Not Found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{entityId}")]
        public async Task<ActionResult<ViewMemberModel>> MemberOperation(Guid entityId)
        {
            try
            {

                var member = await _repository.GetItem(entityId);
                if (member==null)
                {
                    return NotFound("Member Information Not Found");
                }
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
        /// <response code="422">Model couldn't be parsed</response>
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
                    var confirmationLink = Url.Action("ConfirmEmail", "Member",
                                        new
                                        {
                                            username = protector.Protect(member.Username),
                                            token = member.EmailConfirmationToken
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
        /// <response code="200">Returned if the Member was updated</response>
        /// <response code="400">Returned if cannot be Processed or the Member couldn't be found</response>
        /// <response code="422">Model couldn't be parsed</response>
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
        /// <param>Model to Confirm Email</param>
        /// <returns>Returns the ststus Message</returns>
        /// <response code="200">Returned if the member Email Confirmation is successfull</response>
        /// <response code="400">Returned if the model couldn't be parsed or validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string username, [FromQuery] string token)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                if (username == null || token == null)
                {
                    return BadRequest("Broken Link");
                }
                //string g = protector.Unprotect(username);
                var memberEmailConfirmCheck = _repository.GetAll()
                    .Where(c => c.Email == protector.Unprotect(username)
                    && c.EmailConfirmationToken == token).SingleOrDefault();
                if (memberEmailConfirmCheck != null)
                {
                    UpdateMemberModel updateMember = new UpdateMemberModel
                    {
                        MemberId = memberEmailConfirmCheck.MemberId                       
                    };

                    var updatedMember = _mapper.Map(updateMember, memberEmailConfirmCheck);
                    updatedMember.IsEmailConfirmed = true;
                    updatedMember = _repository.Update(updatedMember);
                    int f = await _repository.SaveChangesAsync();
                    return Ok("Email Confirmed.");
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
        /// Action for ForgotPassword.
        /// </summary>
        /// <param name="forgotPasswordModel">Model to Confirm Email</param>
        /// <returns>Returns the status Message.Reset Password link will be sent to registered EmailId.</returns>
        /// <response code="200">Returned if Successfull</response>
        /// <response code="400">Returned if the model couldn't be parsed or the member couldn't be found</response>
        /// <response code="422">Model couldn't be parsed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                if (forgotPasswordModel == null)
                {
                    return BadRequest("Email is not supplied");
                }
                //string g = protector.Unprotect(e);
                var forgotPasswordUser = _repository.GetAll()
                    .Where(c => c.Email == forgotPasswordModel.Email && c.IsEmailConfirmed == true).SingleOrDefault();
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
                                            email = protectorForgotPassword.Protect(updatedMember.Email),
                                            token = memberToUpdate.ForgotPasswordConfirmationToken
                                        }, Request.Scheme);


                        var message = new Message(new string[] { "lijotech@gmail.com" },
                        "Reset Password", "Click this link to Continue:" + PasswordResetLink, null);
                        _emailSender.SendEmail(message);
                    }
                    int f = await _repository.SaveChangesAsync();
                    return Ok(" Reset Password Link Sent to Registered Email.");
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
        /// Reset password link with Get Request which should not be used.
        /// </summary>        
        /// <returns>Returns the status Message.Cannot Use Get Request for reset password.</returns>
        /// <response code="400">Returs Invalid Request</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("ResetPassword")]
        [HttpGet]
        public ActionResult<string> ResetPassword([FromQuery] string email, [FromQuery] string token)
        {
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// Action for Resetpassword.
        /// </summary>
        /// <param name="resetPassword">Model for Reset password</param>
        /// <returns>Returns the status Message.</returns>
        /// <response code="200">Returned if Successfull</response>
        /// <response code="400">Returned if the model couldn't be parsed or the member couldn't be found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                        return Ok("Password Reset Successfully.");
                    else
                        return BadRequest("Password Reset Failed.");
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
