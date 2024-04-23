using IdentityAuth.DTOs;
using IdentityAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                FullName = registerDTO.FullName,
                UserName=registerDTO.FullName,
                Email = registerDTO.Email,
                Age = registerDTO.Age,
                Status = registerDTO.Status,
            };

            var checkedUser= await _userManager.FindByEmailAsync(registerDTO.Email);
            if(checkedUser != null)
            {
                return BadRequest("You already have account");
            }

            var result = await _userManager.CreateAsync(user,registerDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.First());
            }

            foreach(var role in registerDTO.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return Ok("User successfully created!");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something went wrong!");
            }
            var user= await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null)
                return NotFound("Email not found");

            //var test= await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            var result = await _signInManager.PasswordSignInAsync(user: user, password: loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized("Something went wrong in Authorization");


            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result =  _userManager.Users.ToList();
            return Ok(result);
        }
    }
}
