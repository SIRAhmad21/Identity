using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TestIdentity.Models.ViewModel;

namespace TestIdentity.Controllers
{
   [Authorize]
    public class AccountController : Controller
    {
        #region Configration
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        #endregion
        #region Users
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
       [HttpPost] 
       [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.Mobile
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)

        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                }
                ModelState.AddModelError("", "Invailed uaernamwe or password");
                return View(model);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        [Authorize(Roles = "User")]
        public IActionResult ChangePasswprd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswprd(ChangePasswprdViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name); // Get the currently logged-in user
                if (user != null)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (changePasswordResult.Succeeded)
                    {
                        // Password changed successfully
                        return RedirectToAction("Index", "Home"); // Redirect to the home page or another appropriate page
                    }

                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        #endregion
        #region Roles
        [Authorize(Roles ="Admin")]
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                ModelState.AddModelError("", "Not Create Role ");

                return View(model); 
            }
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            EditRoleViewModel model = new EditRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name

            };
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    model.Users.Add(user.Email!);
                }

            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId!);
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RoleList));
                }
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id ,  IdentityRole r)
        {
         if (id == null) { return NotFound(); }
         var role = await _roleManager.FindByIdAsync(id);
         var result = await _roleManager.DeleteAsync(role!);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(RoleList));
            }
            return View(role);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserRole(string id)
        {
            List<UserRoleViewModel> userRoleViewModels = new List<UserRoleViewModel>();
            if (id == null)
            {
                return NotFound();

            }
         var role = await _roleManager.FindByIdAsync(id);   
            foreach (var user in _userManager.Users) {
            UserRoleViewModel model = new UserRoleViewModel { UserName=user.UserName,UserId=user.Id };
                if (await _userManager.IsInRoleAsync(user,role.Name!)  ) {
                model.IsSelected=true;
                }
            else
                {
                    model.IsSelected=false;
                }
                userRoleViewModels.Add(model);  

            }
            return View(userRoleViewModels);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserRole(string id, List<UserRoleViewModel> models)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            IdentityResult result = null;

            for (int i = 0; i < models.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(models[i].UserId);

                if (models[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);

                }

                else if (!(models[i].IsSelected) && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                }
            }
            if (result!.Succeeded)
            {
                return RedirectToAction("EditRole", new { id = id });
            }
            return View(models);
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

     
  #endregion

}
