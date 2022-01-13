using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dsExam.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public AdminController()
        {
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult GetCheck()
        {
            return Json(new { a = 9 });
        }

        [AllowAnonymous] // 로그인 되지 않은 익명 사용자를 받겠다.
        public IActionResult GetUserCheck()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(new { a = 9 });
            }
            return Json(new { a = 1 });
        }
    }
}
