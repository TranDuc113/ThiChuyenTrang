using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThiChuyenTrang.Models;
using ThiChuyenTrang.Models.EF;
using System.Linq;

namespace ThiChuyenTrang.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class NewsController : Controller
    {

        private ThiChuyenTrangContext context;
        public NewsController(ThiChuyenTrangContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllNews(string txtTitle,int slDanhMucBaiViet, int start, int length)
        {
            RequestRespone Respone = new RequestRespone();
            var query = (from ns in context.News
                         join ct in context.Category on ns.CategoryId equals  ct.Id
                         join us in context.Users on ns.CreatedBy equals us.Id
                         join usUpdate in context.Users on ns.UpdatedBy equals usUpdate.Id into usUpdates
                         from us_Update in usUpdates.DefaultIfEmpty()
                         where (string.IsNullOrEmpty(txtTitle) || ct.Title.Contains(txtTitle))
                         && (ct.IsDelete == false)
                         select new
                         {
                             Id = ns.Id,
                             Titles = ns.Title,
                             CreateByName = us.FullName,
                             CreatedDate = ns.CreatedDate,
                             UpdateByName = us_Update.FullName,
                             UpdateByDate = ns.UpdatedDate,
                             CategoryTitle = ct.Title
                         }).ToList();
            Respone.data = query.Skip(start).Take(length).ToList();
            Respone.recordsTotal = query.Count;
            Respone.recordsTotal = query.Count;
            return Json(Respone);
        }


        public IActionResult News_Add()
        {
            ViewBag.lstCategory=context.Category.Where(e=>e.IsDelete==false).ToList();
            return View();
        }
    }
}
