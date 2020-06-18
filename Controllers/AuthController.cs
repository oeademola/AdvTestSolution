using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Advansio.API.Dtos;
using Advansio.API.Helpers;
using Advansio.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Advansio.API.Controllers 
{
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthController (IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var user = await _userManager.FindByEmailAsync(userForRegisterDto.Email);
            if (user != null)
            {
                return BadRequest(new { error = "User already exist" });
            }
            
            try{

            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            userToCreate.AccountNumber = StaticDetails.GenerateAccountNo();
            userToCreate.AccountBalance = StaticDetails.DefaultAmount;
            userToCreate.IsActive = true;
            userToCreate.DateCreated = DateTime.Now;
            userToCreate.UserName = userForRegisterDto.Email;

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            // var userToReturn = _mapper.Map<UserForListDto>(userToCreate);
           
            if (result.Succeeded)
            {
                return Ok("Congratulations!!! Your account creation was successful. Kindly remember to fund your account");
            }

            return BadRequest(result.Errors);
            }catch (Exception)
            {
                return StatusCode(500,"Error Occured please try again later,please try again later...");
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userForLoginDto.Email);

            if(user == null)
            {
                return Unauthorized(new {desciption="Your login details are not correct"});
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = _mapper.Map<UserForListDto>(user);

                return Ok(new
                {
                    token = GenerateJwtToken(user).Result,
                    user = appUser
                });
            }

            return Unauthorized(new {desciption="Your login details are not correct"});

        }

          private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Email, user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}