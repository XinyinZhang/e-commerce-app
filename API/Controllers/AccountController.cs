using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API.Controllers
{
    // the controller to manage user account's CRUD actions 
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
    [Authorize]
    [HttpGet]
    // the method to get curent login user
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Displayname = user.DisplayName
        };
    }
    // check if an email address is already in used
    // Note: also our API already reject a client to register with the same email address
    // we still can have this method, which is used by the client to check if a email address
    // is already in used(before they register)
    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }
    // get current user address
    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {

        var user = await _userManager
        .FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);
        return _mapper.Map<Address, AddressDto>(user.Address);
    }
    // update user Address
    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);
        user.Address = _mapper.Map<AddressDto, Address>(address);
        var result = await _userManager.UpdateAsync(user);
        if(result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        return BadRequest("Problem updating the user");
    }

    // the method we used to login a user
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // get the user with the specific email
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        // if we can't find a user corresponding to the login user, 
        // then login fails
        if (user == null) return Unauthorized(new ApiResponse(401));
        // if we find the user, try to login this user with the password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,
        false);
        // if sign in fails, means the password isn't correct
        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
        // if login successful
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Displayname = user.DisplayName
        };
    }
    [HttpPost("register")]
    // register a user
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // convert registerDto type to AppUser type and
        // store new user's data into identity database
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email

        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400));
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Displayname = user.DisplayName
        };
    }
}   
}


