using DemoJwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using ThiChuyenTrang.Models;
using ThiChuyenTrang.Models.EF;

namespace ThiChuyenTrang.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class LoginController : Controller
    {
        private IJwtAuthenicationManager IJwtAuthenicationManager;
        private ThiChuyenTrangContext context;
        public LoginController(IJwtAuthenicationManager IJwtAuthenicationManager, ThiChuyenTrangContext _context)
        {
            this.IJwtAuthenicationManager = IJwtAuthenicationManager;
            context = _context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            
            return View();
        }

        [AllowAnonymous]
        public IActionResult Authenicate(UserModel user)
        {
            RequestRespone respone = new RequestRespone();
            var checkModel = context.Users.Where(e => e.Username == user.TaiKhoan && e.Password == user.MatKhau).FirstOrDefault();
            if(checkModel == null)
            {
                respone.icon = Containts.ERROR;
                respone.message = "Thông tin tài khoản không đúng";
                return Json(respone);
            }

            var token = IJwtAuthenicationManager.Autheniaction(user.TaiKhoan, user.MatKhau);
            if (token == null)
            {
                respone.icon = Containts.ERROR;
                respone.message = "Thông tin tài khoản, mật khẩu không đúng";
                return Json(respone);
            }

            UserInfoModel userInfoModel = new UserInfoModel();
            userInfoModel.FullName = checkModel.FullName;
            userInfoModel.Id = checkModel.Id;
            userInfoModel.Username = checkModel.Username;
            userInfoModel.Token = token.JwtToken.ToString();

            HttpContext.Session.SetString("UserAdminToken", JsonSerializer.Serialize(userInfoModel) );

            respone.icon = Containts.SUCCESS;
            respone.message = "Thành công";
            return Json(respone);
        }
    }
}
