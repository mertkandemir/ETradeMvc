using System.Collections.Generic;
using System.Linq;
using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ETrade.Tests
{
    // MSTest, XUnit, NUnit
    // Shouldly
    [TestClass]
    public class ProductServiceTests
    {
        private readonly ETradeContext db;
        private readonly RepositoryBase<Product> productRepository;
        private readonly IService<Product, ProductModel> productService;
        
        public ProductServiceTests()
        {
            db = new ETradeContext();
            productRepository = new Repository<Product>(db);
            productService = new ProductService(productRepository);
        }

        [TestMethod]
        public void ShouldGetProducts()
        {
            List<ProductModel> productsModel = productService.GetQuery().ToList();
            Assert.IsTrue(productsModel.Count > 0);
        }

        [TestMethod]
        public void ShouldGetProductWithNameLaptop()
        {
            ProductModel productModel = productService.GetQuery().SingleOrDefault(product => product.Name == "Laptop");
            //productModel.ShouldNotBeNull(); // Shouldly
            Assert.IsNotNull(productModel);
        }
    }
}
