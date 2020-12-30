using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeBusiness.Services;

namespace ETradeMvcWebUI.Controllers
{
    //[Authorize(Users = "cagil")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ETradeContext db;
        private readonly RepositoryBase<Category> categoryRepository;
        private readonly IService<Category, CategoryModel> categoryService;

        public CategoriesController()
        {
            db = new ETradeContext();
            categoryRepository = new Repository<Category>(db);
            categoryService = new CategoryService(categoryRepository);
        }

        // GET: Categories
        [AllowAnonymous]
        public ActionResult Index()
        {
            var categories = categoryService.GetQuery().ToList();
            return View(categories);
        }

        // GET: Categories/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryModel category = categoryService.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        
        public ActionResult Create()
        {
            var model = new CategoryModel();
            return View(model);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                categoryService.Add(category);
                TempData["Message"] = "Operation successful.";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryModel category = categoryService.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                categoryService.Update(category);
                TempData["Message"] = "Operation successful.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryModel category = categoryService.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(int id)
        {
            var category = categoryService.GetById(id);
            if (category.ProductCount > 0)
            {
                TempData["Message"] = "Operation failed: Category has products.";
                return RedirectToAction("Index");
            }
            categoryService.Delete(id);
            TempData["Message"] = "Operation successful.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
