using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThiChuyenTrang.Models;
using ThiChuyenTrang.Models.EF;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System;

namespace ThiChuyenTrang.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ThiChuyenTrangContext context;
        public CategoryController(ThiChuyenTrangContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllCategory(string txtTitle, int start, int length)
        {
            RequestRespone Respone = new RequestRespone();
            var query = (from ct in context.Category
                         join us in context.Users on ct.CreatedBy equals us.Id
                         join usUpdate in context.Users on ct.UpdatedBy equals usUpdate.Id into usUpdates
                         from us_Update in usUpdates.DefaultIfEmpty()
                         where (string.IsNullOrEmpty(txtTitle) || ct.Title.Contains(txtTitle))
                         && (ct.IsDelete==false)
                         select new
                         {
                             Id = ct.Id,
                             Titles = ct.Title,
                             CreateByName = us.FullName,
                             CreatedDate = ct.CreatedDate,
                             UpdateByName = us_Update.FullName,
                             UpdateByDate = ct.UpdatedDate,
                         }).ToList();
            Respone.data = query.Skip(start).Take(length).ToList();
            Respone.recordsTotal = query.Count;
            Respone.recordsTotal = query.Count;
            return Json(Respone);
        }

        public IActionResult DanhMuc_Sua(int Id)
        {
            ViewBag.DetailCategory = context.Category.Where(e => e.Id == Id).FirstOrDefault();
            return View();
        }


        public JsonResult DetailCategorySave(string Title, string Slug, int Id)
        {

            UserInfoModel userInfoModel = JsonSerializer.Deserialize<UserInfoModel>(HttpContext.Session.GetString("UserAdminToken"));

            RequestRespone Respone = new RequestRespone();
            try
            {
                if (Id > 0)
                {
                    Category ct = context.Category.Where(e => e.Id == Id).FirstOrDefault();
                    ct.Title = Title;
                    ct.Slug = Slug;
                    ct.UpdatedBy = userInfoModel.Id;
                    ct.UpdatedDate = DateTime.Now;
                    context.SaveChanges();
                    Respone.message = "Cập nhật thành công";
                    Respone.icon = Containts.ERROR;
                }
                else
                {
                    Category ct = new Category();
                    ct.Title = Title;
                    ct.Slug = Slug;
                    ct.UpdatedBy = userInfoModel.Id;
                    ct.UpdatedDate = DateTime.Now;
                    ct.CreatedBy = userInfoModel.Id;
                    ct.CreatedDate = DateTime.Now;
                    context.Category.Add(ct);
                    context.SaveChanges();
                    Respone.message = "Thêm mới thành công";
                    Respone.icon = Containts.ERROR;
                }
                return Json(Respone);
            }
            catch(Exception e)
            {
                Respone.message = "Có lỗi:"+e.Message;
                Respone.icon = Containts.ERROR;
                return Json(Respone);
            }
        }

        public IActionResult DanhMuc_ThemMoi()
        {
            return View();
        }
    }
}
