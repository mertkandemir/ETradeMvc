using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ETradeBusiness.Models;
using System.Web.Mvc;
using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;

namespace ETradeMvcWebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly DbContext db;
        private readonly RepositoryBase<Product> productRepository;
        private readonly RepositoryBase<User> userRepository;
        private readonly RepositoryBase<Role> roleRepository;
        private readonly IService<Product, ProductModel> productService;
        private readonly IService<User, UserModel> userService;

        public CartController()
        {
            db = new ETradeContext();
            productRepository = new Repository<Product>(db);
            userRepository = new Repository<User>(db);
            roleRepository = new Repository<Role>(db);
            productService = new ProductService(productRepository);
            userService = new UserService(userRepository, roleRepository);
        }

        public ActionResult AddToCart(int? id)
        {
            if (id.HasValue)
            {
                List<CartModel> cart;
                int cartId;
                if (Session["Cart"] == null)
                {
                    cart = new List<CartModel>();
                    cartId = 1;
                }
                else
                {
                    cart = Session["Cart"] as List<CartModel>;
                    if (cart.Count == 0)
                    {
                        cartId = 1;
                    }
                    else
                    {
                        cartId = cart.Max(c => c.Id) + 1;
                    }
                }
                UserModel user = userService.GetQuery().SingleOrDefault(u => u.UserName == User.Identity.Name);
                ProductModel product = productService.GetById(id.Value);
                CartModel cartItem = new CartModel()
                {
                    Id = cartId,
                    ProductId = product.Id,
                    UserId = user.Id,
                    UnitPrice = product.UnitPrice,
                    ProductName = product.Name,
                    UserName = user.UserName
                };
                cart.Add(cartItem);
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFromCart(int? id)
        {
            List<CartModel> cart;
            CartModel cartItem;
            if (id.HasValue)
            {
                if (Session["Cart"] != null)
                {
                    cart = Session["Cart"] as List<CartModel>;
                    if (cart.Count > 0)
                    {
                        cartItem = cart.SingleOrDefault(c => c.Id == id.Value);
                        cart.Remove(cartItem);
                        Session["Cart"] = cart;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            List<CartModel> cart = new List<CartModel>();
            if (Session["Cart"] != null)
            {
                cart = Session["Cart"] as List<CartModel>;
            }
            return View(cart);
        }
    }
}