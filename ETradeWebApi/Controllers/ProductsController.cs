using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ETradeWebApi.Controllers
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=net-5.0
    // https://www.postman.com/downloads/

    [Authorize(Roles = "Admin")]
    public class ProductsController : ApiController
    {
        private readonly ETradeContext db;
        private readonly RepositoryBase<Product> productRepository;
        private readonly IService<Product, ProductModel> productService;

        public ProductsController()
        {
            db = new ETradeContext();
            productRepository = new Repository<Product>(db);
            productService = new ProductService(productRepository);
        }

        //[HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Get() // GetProducts (List, Index)
        {
            try
            {
                List<ProductModel> productsModel = productService.GetQuery().ToList();
                if (productsModel == null || productsModel.Count == 0)
                {
                    return NotFound();
                }
                return Ok(productsModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpGet]
        public IHttpActionResult Get(int id) // GetProduct (Item, Details)
        {
            try
            {
                ProductModel productModel = productService.GetById(id);
                if (productModel == null)
                {
                    return NotFound();
                }
                return Ok(productModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpPost]
        public IHttpActionResult Post(ProductModel product) // PostProduct (Create)
        {
            try
            {
                productService.Add(product);
                //return Ok();
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpPut]
        public IHttpActionResult Put(ProductModel product) // PutProduct (Edit)
        {
            try
            {
                productService.Update(product);
                //return Ok();
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpDelete]
        public IHttpActionResult Delete(int id) // DeleteProduct (Delete)
        {
            try
            {
                productService.Delete(id);
                //return Ok();
                return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
