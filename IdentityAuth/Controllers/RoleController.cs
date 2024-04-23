using IdentityAuth.DTOs;
using IdentityAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async  Task<IActionResult> CreateRole(RoleDTO role)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var roleName=await _roleManager.FindByNameAsync(role.RoleName);
            if (roleName != null)
            {
                return BadRequest("Role already occured");
            }
            await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            return Ok("Role created");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles=await _roleManager.Roles.ToListAsync();
            return Ok(roles);   
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteRoleById(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest("Error occured");
            }
                return Ok($"Role '{role.Name}' deleted successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoleById(string Id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            role.Name = roleName;
            
            var result =await  _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest("Error occured");
            }
                return Ok($"Role updated to '{roleName}'.");
        }
    }
}
