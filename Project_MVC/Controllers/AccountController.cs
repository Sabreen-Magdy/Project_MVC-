using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerView)
        {
            if (ModelState.IsValid) {
				var user = new ApplicationUser()
				{
					UserName = registerView.Email.Split('@')[0],
					Email = registerView.Email,
					IsAgree = registerView.IsAgree
				};
               var result= await _userManager.CreateAsync(user,registerView.Password);
                if(result.Succeeded)
                   return RedirectToAction(nameof(Login));
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerView);
            }
            else
            {
				return View(registerView);
			}
		}
      
		public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(loginViewModel.Email);
                if(user != null)
                {
                    bool flag =await _userManager.CheckPasswordAsync(user,loginViewModel.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                        return View(loginViewModel);
                    }
                    ModelState.AddModelError(string.Empty, "Password is Invalid!");
                }
				ModelState.AddModelError(string.Empty, "Email is Invalid!");
			}
			return View(loginViewModel);
		}
		public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(SendEmailViewModel sendEmailViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(sendEmailViewModel.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPassLink = Url.Action("ResetPassword", "Account", new { email = sendEmailViewModel.Email, token = token }, Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        To = sendEmailViewModel.Email,
                        Body = resetPassLink
                    };
                    AccountSetting.SendEmailToResetPassword(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not Existed!");
                }
            }
            return View(sendEmailViewModel);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
		public IActionResult ResetPassword(string email,string token)
		{
            TempData["email"]=email;
            TempData["token"]=token;
			return View();
		}
        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
		{
            if (ModelState.IsValid)
            {
                var email= TempData["email"] as string;
                var token= TempData["token"] as string;
				if (email == null || token == null)
				{
					ModelState.AddModelError(string.Empty, "Invalid password reset token.");
					return View(resetPasswordViewModel);
				}
				var user =await _userManager.FindByEmailAsync(email);
                if(user != null) {
					var resetPassResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.Password);
					if (resetPassResult.Succeeded)
					{
						await _signInManager.SignOutAsync();

						return RedirectToAction(nameof(Login));
					}
					else
					{
						foreach (var error in resetPassResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
                else
                {
                    ModelState.AddModelError(string.Empty, "Email isn't existed!");
                }
            } 
                return View(resetPasswordViewModel);
            
		}
	}
   
    }

