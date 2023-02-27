using API.Services;
using API.Utils;
using API.Utils.Messages;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SecurityServices _securityServices;
        private readonly UserServices _userServices;

        public UserController(IMapper mapper, SecurityServices securityServices, UserServices userServices)
        {
            _mapper = mapper;
            _securityServices = securityServices;
            _userServices = userServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody] User user)
        { 

            if (user == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", AuthErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(AuthErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _userServices.AddUser(user, RoleType.User);
            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(CommonMessages.ERROR_UPDATE);

            if (user != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(user);
                user.Token = tokenString;
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = user,
                });
            }

            return response;

        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }

            //get user id from token
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber, 0);
            User currentUser = await _userServices.GetUserById(currentUserId, 0);

            if (oldUser == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            if ((oldUser.Id != currentUserId) && (currentUser.RoleId != (int)RoleType.Admin))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!oldUser.MobileNumber.Equals(user.MobileNumber))
            {
                if (_userServices.ExistsMobileNumber(user.MobileNumber))
                {
                    ModelState.AddModelError("user", AuthErrorMessages.MOBILE_NUMBER_EXISTS);
                    return BadRequest(AuthErrorMessages.ModelStateParser(ModelState));
                }

                oldUser.MobileNumber = user.MobileNumber;
            }

            oldUser.Fullname = user.Fullname;

            bool res = await _userServices.Update(oldUser);

            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.UPDATED_SUCCESSFULLY);


        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {

            User user = await _userServices.GetUserById(id, 0);

            if (user == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User currentUser = await _userServices.GetUserById(currentUserId, 0);


            // Check if the person deleting the account is its owner
            if ((user.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)) && ((currentUser.RoleId != (int)RoleType.Admin)))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!_userServices.DeleteUser(user))
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.DELETED_SUCCESSFULLY);
        }

        // Put: api/user/ResetPassword
        [HttpPut("UpdateFirebaseToken")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFirebaseToken([FromBody] string firebaseToken)
        {
            if (firebaseToken == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (firebaseToken == "")
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(AuthErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool res = await _userServices.UpdateFirebaseToken(currentUserId, firebaseToken);
            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.UPDATED_SUCCESSFULLY);
        }

        // GET: api/user/users
        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int roleId, int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetUsers(page, pageSize, paginationInfo, roleId);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

    }
}
