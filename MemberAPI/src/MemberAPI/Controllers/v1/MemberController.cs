using AutoMapper;
using MemberAPI.Domain.Entities;
using MemberAPI.Models.v1;
using MemberAPI.Service.Master.v1;
using MemberAPI.Data.Security.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IServiceMaster _serviceMaster;
        private readonly ILoggerManager _logger;

        public MemberController(IMapper mapper, IServiceMaster serviceMaster, ILoggerManager logger)
        {
            _mapper = mapper;
            _serviceMaster = serviceMaster;
            _logger = logger;
        }


        /// <summary>
        ///     Action to retrieve all members with Completed Registrations.
        /// </summary>
        /// <returns>Returns the created members</returns>
        /// <response code="200">Returned if the list of Members was retrieved</response>
        /// <response code="400">Returned if the Members could not be retrieved</response>
        /// <response code="401">Returned if Unauthorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<List<ViewMemberModel>>> MemberOperation()
        {
            try
            {
                _logger.LogInfo("Here is info message from the controller.");
                _logger.LogDebug("Here is debug message from the controller.");
                _logger.LogWarn("Here is warn message from the controller.");
                _logger.LogError("Here is error message from the controller.");
                return _mapper.Map<List<ViewMemberModel>>(await _serviceMaster.GetAllMembers());
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
                var member = await _serviceMaster.GetMember(entityId);
                if (member == null)
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

            try
            {

                var allmembers = await _serviceMaster.GetAllMembers();
                bool memberEmailCheck = allmembers.Any(c => c.Email == createMemberModel.Email);
                if (memberEmailCheck)
                    return BadRequest($"Member Already Registered with Email Id {createMemberModel.Email}");

                var addedMember = await _serviceMaster.AddMemberAsync(
                     _mapper.Map<Member>(createMemberModel),
                     Url.Action("ConfirmEmail", "Member", null, Request.Scheme)
                );
                if (addedMember == null)
                {
                    return BadRequest("Member not added.");
                }
                return _mapper.Map<ViewMemberModel>(addedMember);
            }
            catch (Exception ex)
            {
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
            try
            {
                var member = await _serviceMaster.GetMember(updateMemberModel.MemberId);
                if (member == null)
                {
                    return BadRequest($"No Member found with the id {updateMemberModel.MemberId}");
                }

                var updatedMember = await _serviceMaster.UpdateMember(_mapper.Map(updateMemberModel, member));
                if (updatedMember == null)
                {
                    return BadRequest("Invalid Request");
                }
                return _mapper.Map<ViewMemberModel>(updatedMember);
            }
            catch (Exception ex)
            {
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
            try
            {
                if (username == null || token == null)
                {
                    return BadRequest("Broken Link");
                }
                var updatedMember = await _serviceMaster.ConfirmEmail(username, token);
                if (updatedMember == null)
                {
                    return BadRequest("Invalid Request");
                }
                return Ok("Email Confirmed.");
            }
            catch (Exception ex)
            {
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
            try
            {
                if (forgotPasswordModel == null)
                {
                    return BadRequest("Email is not supplied");
                }
                var updatedMember = await _serviceMaster.ForgotPassword(
                    forgotPasswordModel.Email,
                    Url.Action("ResetPassword", "Member", null, Request.Scheme));
                if (updatedMember == null)
                {
                    return BadRequest("Invalid Request");
                }
                return Ok(" Reset Password Link Sent to Registered Email.");
            }
            catch (Exception ex)
            {
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
            try
            {
                if (resetPassword == null)
                {
                    return BadRequest("Invalid Request");
                }
                var updatedMember = await _serviceMaster.ResetPassword(
                   resetPassword.Email, resetPassword.Token, resetPassword.Password);
                if (updatedMember == null)
                {
                    return BadRequest("Password Reset Failed.");
                }
                return Ok("Password Reset Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Action for Login Check.
        /// </summary>
        /// <param name="loginMember">Model for Login</param>
        /// <returns>Returns the status Message.</returns>
        /// <response code="200">Returned if Successfull</response>
        /// <response code="400">Returned if the model couldn't be parsed or the member couldn't be found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<ViewMemberModel>> Login([FromBody] LoginMemberModel loginMember)
        {
            try
            {
                if (loginMember == null)
                {
                    return BadRequest("Invalid Request");
                }
                var validloginMember = await _serviceMaster.Login(
                   loginMember.Email, loginMember.Password);
                if (validloginMember == null)
                {
                    return BadRequest("Login Failed.");
                }
                return _mapper.Map<ViewMemberModel>(validloginMember);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
