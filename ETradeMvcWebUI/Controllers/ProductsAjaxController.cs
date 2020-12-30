using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using ETradeMvcWebUI.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ETradeMvcWebUI.Controllers
{
    public class ProductsAjaxController : Controller
    {
        private readonly ETradeContext db;
        private readonly RepositoryBase<Product> productRepository;
        private readonly RepositoryBase<Category> categoryRepository;
        private readonly RepositoryBase<User> userRepository;
        private readonly RepositoryBase<Role> roleRepository;
        private readonly IService<Product, ProductModel> productService;
        private readonly IService<Category, CategoryModel> categoryService;
        private readonly IService<User, UserModel> userService;

        public ProductsAjaxController()
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

        // GET: ProductsAjax
        public ActionResult List()
        {
            var productQuery = productService.GetQuery();
            var productList = productQuery.ToList();

            // bu dönüştürme işleminin de serviste bir GetList() methodunda yapılması çok daha iyi
            foreach (var product in productList)
            {
                product.CreateDateText = product.CreateDate.ToString(new CultureInfo("en"));
                product.UpdateDateText = product.UpdateDate.HasValue
                    ? product.UpdateDate.Value.ToString(new CultureInfo("en"))
                    : "";
            }

            var categoryList = categoryService.GetQuery().ToList();
            ProductsIndexViewModel viewModel = new ProductsIndexViewModel()
            {
                Products = productList,
                Categories = new SelectList(categoryList, "Id", "Name")
            };
            return View("ProductList", viewModel);
        }

        [HttpPost]
        public ActionResult List(ProductsIndexViewModel productsIndexViewModel)
        {
            var productQuery = productService.GetQuery();
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
            var productList = productQuery.ToList();

            // bu dönüştürme işleminin de serviste bir GetList() methodunda yapılması çok daha iyi
            foreach (var product in productList)
            {
                product.CreateDateText = product.CreateDate.ToString(new CultureInfo("en"));
                product.UpdateDateText = product.UpdateDate.HasValue
                    ? product.UpdateDate.Value.ToString(new CultureInfo("en"))
                    : "";
            }
            
            ProductsIndexViewModel viewModel = new ProductsIndexViewModel()
            {
                Products = productList
            };
            return PartialView("_Products", viewModel);
        }
    }
}