using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ictshop.fonts.WEB_NC_QUOCHUY_V1.Ictshop.Ictshop.Models;
using Ictshop.Models;
namespace Ictshop.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        private QLdienthoaiEntitiess db = new QLdienthoaiEntitiess();
        // GET: Admin/DonHang
        public ActionResult Index()
        {
            var donhangs = db.Donhangs.ToList();
            if (donhangs == null)
            {
                return HttpNotFound();
            }
            return View(donhangs);
        }
        public ActionResult Details(int? madon, int? masp)
        {
            if (madon == null || masp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Chitietdonhangs.Any(ct => ct.Madon == madon && ct.Masp == masp))
            {
                return HttpNotFound();
            }
            Chitietdonhang chitietdonhang = db.Chitietdonhangs
                .SingleOrDefault(ct => ct.Madon == madon && ct.Masp == masp);
            return View(chitietdonhang);
        }
        }
    }
