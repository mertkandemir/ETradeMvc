using System;
using AppCore.Business.Bases;
using AppCore.Business.Enums;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ETradeMvcWebUI.Models;
using System.IO;

namespace ETradeMvcWebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ETradeContext db;
        private readonly RepositoryBase<Product> productRepository;
        private readonly RepositoryBase<Category> categoryRepository;
        private readonly RepositoryBase<User> userRepository;
        private readonly RepositoryBase<Role> roleRepository;
        private readonly IService<Product, ProductModel> productService;
        private readonly IService<Category, CategoryModel> categoryService;
        private readonly IService<User, UserModel> userService;

        public ProductsController()
        {
            db = new ETradeContext();
            productRepository = new Repository<Product>(db);
            categoryRepository = new Repository<Category>(db);
            userRepository = new Repository<User>(db);
            roleRepository = new Repository<Role>(db);
            productService = new ProductService(productRepository);
            categoryService = new CategoryService(categoryRepository);
            userService = new UserService(userRepository, roleRepository);
        }

        // GET: Products
        public ActionResult Index(ProductsIndexViewModel productsIndexViewModel, string message = "")
        {
            //var productList = productService.GetQuery().ToList().Select(m => new ProductModel()
            //{
            //    Id = m.Id,
            //    Name = m.Name,
            //    UnitPrice = m.UnitPrice,
            //    StockAmount = m.StockAmount,
            //    CategoryId = m.CategoryId,
            //    CreateDate = m.CreateDate,
            //    UpdateDate = m.UpdateDate,
            //    IsDeleted = m.IsDeleted,
            //    CategoryName = m.CategoryName,
            //    CreateDateText = m.CreateDate.ToString(new CultureInfo("en")),
            //    UpdateDateText = m.UpdateDate.HasValue ? m.UpdateDate.Value.ToString(new CultureInfo("en")) : "",
            //    StockAmountText = m.StockAmountText
            //}).ToList();
            //var productList = productService.GetQuery().ToList(); 

            // Microsoft DynamicLinq kütüphanesi
            // Sayfalama için PagedList kullanımı: https://www.gencayyildiz.com/blog/asp-net-mvcde-pagedlist-kullanarak-verileri-sayfalama/
            // 1: Tüm sorguyu çek
            var productQuery = productService.GetQuery();

            // 2: Sorguya where filtrelerini uygula
            if (!string.IsNullOrWhiteSpace(productsIndexViewModel.Name))
            {
                productQuery = productQuery.Where(product => product.Name.ToUpper().Contains(productsIndexViewModel.Name.ToUpper().Trim()));
            }
            if (productsIndexViewModel.CategoryId.HasValue)
            {
                productQuery = productQuery.Where(product => product.CategoryId == productsIndexViewModel.CategoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(productsIndexViewModel.UnitPriceMin))
            {
                double unitPriceMin = Convert.ToDouble(productsIndexViewModel.UnitPriceMin, new CultureInfo("en"));
                productQuery = productQuery.Where(product => product.UnitPrice >= unitPriceMin);
            }
            if (!string.IsNullOrWhiteSpace(productsIndexViewModel.UnitPriceMax))
            {
                double unitPriceMax = Convert.ToDouble(productsIndexViewModel.UnitPriceMax, new CultureInfo("en"));
                productQuery = productQuery.Where(product => product.UnitPrice <= unitPriceMax);
            }
            if (!string.IsNullOrWhiteSpace(productsIndexViewModel.CreateDateMin))
            {
                DateTime createDateMin = DateTime.Parse(productsIndexViewModel.CreateDateMin, new CultureInfo("en"));
                productQuery = productQuery.Where(product => product.CreateDate >= createDateMin);
            }
            if (!string.IsNullOrWhiteSpace(productsIndexViewModel.CreateDateMax))
            {
                DateTime createDateMax = DateTime.Parse(productsIndexViewModel.CreateDateMax, new CultureInfo("en"));
                productQuery = productQuery.Where(product => product.CreateDate <= createDateMax);
            }

            // 3: Filtrelenmiş sorgu üzerinden kayıt sayısını çek
            int totalProductCount = productQuery.Count();

            // 4: Sayfada gösterilecek kayıt sayısını belirle
            int productCountPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["ProductCountPerPage"]);

            // 5: Sorguyu herhangi bir özelliğe (veya özelliklere) göre sırala
            switch (productsIndexViewModel.OrderBy)
            {
                case "name asc":
                    productQuery = productQuery.OrderBy(product => product.Name);
                    break;
                case "name desc":
                    productQuery = productQuery.OrderByDescending(product => product.Name);
                    break;
                case "categoryname asc":
                    productQuery = productQuery.OrderBy(product => product.CategoryName);
                    break;
                case "categoryname desc":
                    productQuery = productQuery.OrderByDescending(product => product.CategoryName);
                    break;
                case "unitprice asc":
                    productQuery = productQuery.OrderBy(product => product.UnitPrice);
                    break;
                case "unitprice desc":
                    productQuery = productQuery.OrderByDescending(product => product.UnitPrice);
                    break;
                case "stockamount asc":
                    productQuery = productQuery.OrderBy(product => product.StockAmount);
                    break;
                case "stockamount desc":
                    productQuery = productQuery.OrderByDescending(product => product.StockAmount);
                    break;
                case "createdate asc":
                    productQuery = productQuery.OrderBy(product => product.CreateDate);
                    break;
                default:
                    productQuery = productQuery.OrderByDescending(product => product.CreateDate);
                    break;
            }

            // 6: Sayfa numarasına göre belirli sayıda kayıtları atla (Skip) ve sayfada gösterilecek kayıt sayısı kadar kayıtları al (Take)
            productQuery = productQuery.Skip((productsIndexViewModel.PageNo - 1) * productCountPerPage).Take(productCountPerPage);
            
            // 7: Sorgu sonucunu listeye çek
            var productList = productQuery.ToList();
            
            // bu dönüştürme işleminin de serviste bir GetList() methodunda yapılması çok daha iyi
            foreach (var product in productList)
            {
                product.CreateDateText = product.CreateDate.ToString(new CultureInfo("en"));
                product.UpdateDateText = product.UpdateDate.HasValue
                    ? product.UpdateDate.Value.ToString(new CultureInfo("en"))
                    : "";
            }

            // 8: Toplam sayfa sayısını sorgu sonucunda where ile filtrelenen toplam kayıt sayısı ve sayfada gösterilecek kayıt sayısı üzerinden hesapla
            int numberOfPages = Convert.ToInt32(Math.Ceiling((decimal)totalProductCount / productCountPerPage));

            // 9: Toplam sayfa sayısı üzerinden sayfa listesini doldur
            List<SelectListItem> pageSelectListItems = new List<SelectListItem>();
            SelectListItem pageSelectListItem;
            for (int i = 1; i <= numberOfPages; i++)
            {
                pageSelectListItem = new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                };
                pageSelectListItems.Add(pageSelectListItem);
            }
            productsIndexViewModel.Pages = new SelectList(pageSelectListItems, "Value", "Text", productsIndexViewModel.PageNo);

            List<SelectListItem> orderSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "categoryname asc",
                    Text = "Category Name Ascending"
                },
                new SelectListItem()
                {
                    Value = "categoryname desc",
                    Text = "Category Name Descending"
                },
                new SelectListItem()
                {
                    Value = "name asc",
                    Text = "Name Ascending"
                },
                new SelectListItem()
                {
                    Value = "name desc",
                    Text = "Name Descending"
                },
                new SelectListItem()
                {
                    Value = "unitprice asc",
                    Text = "Unit Price Ascending"
                },
                new SelectListItem()
                {
                    Value = "unitprice desc",
                    Text = "Unit Price Descending"
                },
                new SelectListItem()
                {
                    Value = "stockamount asc",
                    Text = "Stock Amount Ascending"
                },
                new SelectListItem()
                {
                    Value = "stockamount desc",
                    Text = "Stock Amount Descending"
                },
                new SelectListItem()
                {
                    Value = "createdate asc",
                    Text = "Create Date Ascending"
                },
                new SelectListItem()
                {
                    Value = "createdate desc",
                    Text = "Create Date Descending"
                }
            };
            productsIndexViewModel.Order = new SelectList(orderSelectListItems, "Value", "Text", productsIndexViewModel.OrderBy);

            var categoryList = categoryService.GetQuery().ToList();
            productsIndexViewModel.Products = productList;
            productsIndexViewModel.Categories = new SelectList(categoryList, "Id", "Name");

            ViewBag.Message = message;
            return View(productsIndexViewModel);
        }

        // GET: Products/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel product = productService.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        // Yeni bir ürün oluşturma işlemini sadece Admin rolündekiler yapabilsin
        //[Authorize(Users = "cagil,leo")]
        public ActionResult Create()
        {
            if (!AdminControl())
            {
                return RedirectToAction("Login", "Account");
            }
            var categoryList = categoryService.GetQuery().ToList();
            ViewBag.Categories = new SelectList(categoryList, "Id", "Name");
            var model = new ProductModel();
            return View(model);
        }

        // Account/Login aksiyonunda Forms Authentication ile giriş yapılan kullanıcı adı bilgisi saklanır.
        // Genellikle veritabanı kullanan yönetim sistemlerinde giriş yapan kullanıcının sistemde yapabileceği işlemler
        // kullanıcı adı üzerinden değil, kullanıcı rolü üzerinden kontrol edilir.
        // Eğer Forms Authentication kullanılıyorsa bu methodda olduğu gibi veritabanından tekil kullanıcı adına göre önce
        // kullanıcı kaydı getirilir, daha sonra bu kullanıcı kaydı üzerinden rolüne bakılır ve kullanıcının rol adı üzerinden
        // işleme yetkisi olup olmadığı kontrol edilir.
        public bool AdminControl()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return false;
            }

            //UserModel userModel = userService.GetQuery(user => user.UserName == User.Identity.Name).FirstOrDefault();
            UserModel userModel = userService.GetQuery().SingleOrDefault(user => user.UserName == User.Identity.Name);
            if (userModel.Role.Name != RoleEnum.Admin.ToString())
            {
                return false;
            }

            return true;
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // Yeni bir ürün oluşturma işlemini sadece Admin rolündekiler yapabilsin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel product, HttpPostedFileBase Image)
        {
            if (!AdminControl())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                string guidFileName = null;
                string extension = null;

                if (Image != null && Image.ContentLength > 0)
                {
                    string fileName = Image.FileName; // asus.jpg
                    extension = Path.GetExtension(fileName); // .jpg
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" ||
                        extension.ToLower() == ".png" || extension.ToLower() == ".bmp")
                    {
                        string fileFolder = Server.MapPath("~/Images"); // ETradeMvcWebUI\Images
                        guidFileName = Guid.NewGuid().ToString();
                        Image.SaveAs(fileFolder + "/" + guidFileName + extension);
                    }
                }

                if (guidFileName != null && extension != null)
                {
                    product.ImagePath = "/Images/" + guidFileName + extension;
                }

                productService.Add(product);

                return RedirectToAction("Index", new { message = "Operation successful." });
            }

            var categoryList = categoryService.GetQuery().ToList();
            ViewBag.Categories = new SelectList(categoryList, "Id", "Name");
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!AdminControl())
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductModel product = productService.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }

            List<CategoryModel> categoryList = categoryService.GetQuery().ToList();
            ViewBag.Categories = new SelectList(categoryList, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel product, HttpPostedFileBase Image)
        {
            if (!AdminControl())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                string guidFileName = null;
                string extension = null;
                if (Image != null && Image.ContentLength > 0)
                {
                    string fileName = Image.FileName; // asus.jpg
                    extension = Path.GetExtension(fileName); // .jpg
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" ||
                        extension.ToLower() == ".png" || extension.ToLower() == ".bmp")
                    {
                        if (product.ImagePath != null)
                        {
                            string filePath = Server.MapPath("~/" + product.ImagePath);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        string folderPath = Server.MapPath("~/Images"); // ETradeMvcWebUI\Images
                        guidFileName = Guid.NewGuid().ToString();
                        Image.SaveAs(folderPath + "/" + guidFileName + extension);
                    }
                }
                if (guidFileName != null && extension != null)
                {
                    product.ImagePath = "/Images/" + guidFileName + extension;
                }

                productService.Update(product);
                return RedirectToAction("Index", new { message = "Operation successful." });
            }
            
            List<CategoryModel> categoryList = categoryService.GetQuery().ToList();
            ViewBag.Categories = new SelectList(categoryList, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!AdminControl())
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductModel product = productService.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string ImagePath)
        {
            if (!AdminControl())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ImagePath != null)
            {
                string filePath = Server.MapPath(ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            productService.Delete(id);
            return RedirectToAction("Index", new { message = "Operation successful." });
        }

        public ActionResult Json()
        {
            var productsModel = productService.GetQuery().ToList();
            return Json(productsModel, JsonRequestBehavior.AllowGet);
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
