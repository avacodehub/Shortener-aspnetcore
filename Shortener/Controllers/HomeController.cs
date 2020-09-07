using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shortener.Models;

namespace Shortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        [BindProperty]
        public Link Link { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult S(string id)
        {
            Link Link = _db.Links.FirstOrDefault(u => u.ShortUrl.Contains(id));
            if (Link == null)
            {
                return View();
            }
            else
            {
                Link.UseCount += 1;
                _db.Links.Update(Link);
                _db.SaveChanges();
                return Redirect(Link.LongUrl);
            }
        }

        public IActionResult Upsert(int? id)
        {
            Link = new Link();
            if (id == null)
            {
                //create
                return View(Link);
            }
            //update 
            Link = _db.Links.FirstOrDefault(u => u.Id == id);
            if (Link == null)
            {
                return NotFound();
            }

            return View(Link);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {

                if (Link.Id == 0)
                {
                    //create
                    //Link.ShortUrl = UniqueLink();
                    _db.Links.Add(Link);
                }
                else
                {
                    _db.Links.Update(Link);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Link);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API calls
        [HttpGet]
        public async Task<IActionResult> GetNewLink()
        {
            string res = UniqueLink();
            return Json(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Links.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var itemFromDb = await _db.Links.FirstOrDefaultAsync(u => u.Id == id);
            if (itemFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _db.Links.Remove(itemFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion

        private string UniqueLink()
        {
            string link = "";
            int tries = 0;
            do
            {
                link = string.Format("{0}://{1}/{2}/{3}", Request.Scheme, Request.Host, "home/s", Generator.GetString());
            } while (_db.Links.Any(u => u.ShortUrl == link) && tries++ < 5);

            if (tries >= 5)
            {
                link = string.Format("{0}://{1}/{2}/{3}", Request.Scheme, Request.Host, "home/s", Generator.GetStringLong());
            }

            return link;
        }
    }
}
