using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeEntities.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace ETradeBusiness.Services
{
    public class ProductService : IService<Product, ProductModel>
    {
        private readonly RepositoryBase<Product> repository;

        public ProductService(RepositoryBase<Product> repositoryParameter)
        {
            repository = repositoryParameter;
        }

        public void Add(ProductModel model, bool saveChanges = true)
        {
            try
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    UnitPrice = model.UnitPrice,
                    StockAmount = model.StockAmount,
                    CategoryId = model.CategoryId,
                    CreateDate = DateTime.Now,
                    ImagePath = model.ImagePath
                };
                repository.AddEntity(entity);
                if (saveChanges)
                    SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Delete(int id, bool saveChanges = true)
        {
            try
            {
                repository.DeleteEntity(id);
                if (saveChanges)
                    SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public ProductModel GetById(int id)
        {
            try
            {
                var query = GetQuery(e => e.Id == id);
                var model = query.FirstOrDefault();
                
                // bu tip dönüştürme işlemleri serviste olmalı!
                model.CreateDateText = model.CreateDate.ToString(new CultureInfo("en"));
                model.UpdateDateText = model.UpdateDate.HasValue
                    ? model.UpdateDate.Value.ToString(new CultureInfo("en"))
                    : "";
                
                
                return model;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IQueryable<ProductModel> GetQuery(Expression<Func<ProductModel, bool>> predicate = null)
        {
            try
            {
                var query = repository.GetEntityQuery("Category").Where(e => e.IsDeleted == false).Select(e => new ProductModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    UnitPrice = e.UnitPrice,
                    StockAmount = e.StockAmount,
                    CategoryId = e.CategoryId,
                    CreateDate = e.CreateDate,
                    UpdateDate = e.UpdateDate,
                    CategoryName = e.Category.Name,
                    StockAmountText = (e.StockAmount ?? 0) <= 10 ? "<label class=\"text-danger\">!</label>" : "",
                    ImagePath = e.ImagePath
                });
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }
                return query;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public int SaveChanges()
        {
            try
            {
                return repository.SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Update(ProductModel model, bool saveChages = true)
        {
            try
            {
                var entity = repository.GetEntityById(model.Id);
                entity.Name = model.Name;
                entity.UnitPrice = model.UnitPrice;
                entity.StockAmount = model.StockAmount;
                entity.CategoryId = model.CategoryId;
                entity.UpdateDate = DateTime.Now;
                entity.ImagePath = model.ImagePath;
                repository.UpdateEntity(entity);
                if (saveChages)
                    SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
