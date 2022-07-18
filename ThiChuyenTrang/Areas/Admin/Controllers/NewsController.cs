using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThiChuyenTrang.Models;
using ThiChuyenTrang.Models.EF;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

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

        public JsonResult GetAllNews(string txtTitle, int slDanhMucBaiViet, int start, int length)
        {
            RequestRespone Respone = new RequestRespone();
            var query = (from ns in context.News
                         join ct in context.Category on ns.CategoryId equals ct.Id
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

        [HttpPost]
        public FileResult Upload(List<IFormFile> file)
        {
            byte[] array = new byte[file.Count];
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition, Content-Length, X-Content-Transfer-Id");
            Response.Headers.Add("Content-Type", file.GetType().ToString());
            Response.Headers.Add("Content-Length", "3965123");
            Response.Headers.Add("Content-Disposition", "inline; filename=\"my - file.jpg\"");
            Response.Headers.Add("X-Content-Transfer-Id", "12345");

            return File(array, "application/pdf", "ExportThongKeSinhVienKyLuat.pdf");
            //return Json(Guid.NewGuid().ToString());
        }

        [HttpPost]
        public JsonResult revert(string Id)
        {
            return Json(Guid.NewGuid().ToString());
        }

        public IActionResult News_Add()
        {
            ViewBag.lstCategory = context.Category.Where(e => e.IsDelete == false).ToList();
            return View();
        }

        public IActionResult News_Detail()
        {
            ViewBag.lstCategory = context.Category.Where(e => e.IsDelete == false).ToList();
            return View();
        }
    }
}
