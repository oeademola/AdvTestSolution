using System.Security.Claims;
using System.Threading.Tasks;
using Advansio.API.Dtos;
using Advansio.API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advansio.API.Controllers 
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase 
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController (IUserService userService, IMapper mapper) 
        {
            _mapper = mapper;
            _userService = userService;

        }

        [HttpPost ("fundAccount/{amount}")]
        public async Task<IActionResult> FundAccount (decimal amount) 
        {
            var currentUserId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);

            var user = await _userService.FundAccount (currentUserId, amount);

            var userToReturn = _mapper.Map<UserForListDto>(user);

            return Ok (userToReturn);

        }

        [HttpPost ("transferFund/{amount}/{accountNumber}")]
        public async Task<IActionResult> TransferFund (decimal amount, string accountNumber) 
        {
            var currentUserId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);

            var currentUser = await _userService.GetUser(currentUserId);

            var beneficiary = await _userService.GetUserByAccountNo(accountNumber);

            if (amount > currentUser.AccountBalance)
            {
                return BadRequest("Sorry you do not have sufficient fund to perform this transaction");

            } else if(beneficiary == null) {
                return BadRequest("Invalid account number, please make sure you enter a valid account number");
            }
            

            var updatedUser = await _userService.TransferFund (currentUserId, amount, accountNumber);

            var userToReturn = _mapper.Map<UserForListDto>(updatedUser);

            return Ok (userToReturn);

        }
    }
}