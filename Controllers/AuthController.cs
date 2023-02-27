using API.Dto;
using API.Services;
using API.Utils;
using API.Utils.Messages;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SecurityServices _securityServices;
        private readonly UserServices _userServices;

        public AuthController(IMapper mapper, SecurityServices securityServices, UserServices userServices)
        {
            _mapper = mapper;
            _securityServices = securityServices;
            _userServices = userServices;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginBodyDto loginBody)
        {
            bool isEmpty = true;

            if (loginBody.mobileNumber != null)
            {
                if (loginBody.mobileNumber.Trim() != "")
                {
                    if (loginBody.password != null)
                    {
                        if (loginBody.password.Trim() != "")
                        {
                            isEmpty = false;
                        }
                    }
                }
            }

            if (isEmpty)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(loginBody.mobileNumber, 0);

            if (user == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            user.Language = loginBody.language;

            await _userServices.UpdateUserLanguage(user.Id, loginBody.language);

            if (!_securityServices.VerifyPassword(loginBody.password, user.StoredSalt, user.Password))
            {
                ModelState.AddModelError("Password", AuthErrorMessages.INCORRECT_PASSWORD);
                return BadRequest(AuthErrorMessages.ModelStateParser(ModelState));
            }

            var jwtToken = _securityServices.GenerateJWTToken(user);

            user.Token = jwtToken;
            IActionResult response = Ok(new
            {
                token = jwtToken,
                userDetails = _mapper.Map<User>(user),
            });

            return Ok(response);
        }

        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] User user)
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
    }
}
