using Microsoft.AspNetCore.Mvc;
using TestIdentity.Data;
using TestIdentity.Models;

namespace TestIdentity.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext db;
        public ProductController(AppDbContext _db)
        {
            db = _db;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Prodect P)
        {
            db.Prodectes.Add(P);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
    }
}
