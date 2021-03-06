using dsExam.Models.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace dsExam.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {
        }

        public IActionResult Index()
        {
            return Redirect("/login/login");
        }

        #region 회원가입
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("login/register")]
        public IActionResult RegisterProc([FromForm] UserModel input)
        {
            try
            {
                string password2 = Request.Form["password"];

                if(input.Password != password2)
                {
                    throw new Exception("password와 password2가 다릅니다");
                }
                input.ConvertPassword();
                input.Register();

                // 성공
                return Redirect("/login/login");
            }
            catch(Exception ex)
            {
                // 실패
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }
        #endregion

        #region 로그인
        [HttpGet]
        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        [Route("login/login")]
        public async Task<IActionResult> LoginProc([FromForm]UserModel input)
        {
            try
            {
                input.ConvertPassword();
                var user = input.GetLoginUser();

                // 로그인 작업
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.User_seq.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.User_name));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));

                if (user.User_name == "test")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddHours(4),
                    AllowRefresh = true
                });
                return Redirect("/");
            }
            catch (Exception ex)
            {
                // 실패
                return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }
        #endregion

        #region 로그아웃
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion
    }
}
