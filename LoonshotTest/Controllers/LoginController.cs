using LoonshotTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using LoonshotTest.Models.Login;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace LoonshotTest.Controllers
{
    public class LoginController : Controller
    {
        
        public LoginController()
        {

        }


        public IActionResult Index()
        {
            return View("/login/login");
        }

        [HttpGet]
        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        /*
        [HttpPost]
        [Route("/login/login")]
        public async Task<IActionResult> LoginProc([FromForm]LoginModel input)
        {
            var Login = input.GetLoginUser();

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Login.staff_login_Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, Login.staff_id.ToString()));
            identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMDDHHmmss")));


            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddHours(4),
                AllowRefresh = true
            });
            
                
            return null;
            
        }
        */

        public IActionResult Register(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        [Route("/login/register")]
        public IActionResult RegisterProc([FromForm] LoginModel input)
        {
            try
            {
                string staff_login_pw2 = Request.Form["staff_login_pw2"];
                
                if (input.staff_login_pw != staff_login_pw2)
                {
                    throw new Exception("패스워드가 불일치 합니다.");
                }

                input.ConvertPassword();

                input.Register();
                return Redirect("/login/login");
            }
            catch (Exception ex)
            {
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
