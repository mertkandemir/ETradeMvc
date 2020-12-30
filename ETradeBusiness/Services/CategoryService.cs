using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Models;
using ETradeEntities.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ETradeBusiness.Services
{
    public class CategoryService : IService<Category, CategoryModel>
    {
        private readonly RepositoryBase<Category> repository;

        public CategoryService(RepositoryBase<Category> repositoryParameter)
        {
            repository = repositoryParameter;
        }

        public void Add(CategoryModel model, bool saveChanges = true)
        {
            try
            {
                var entity = new Category()
                {
                    Id = model.Id,
                    Name = model.Name
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

        public CategoryModel GetById(int id)
        {
            try
            {
                var query = GetQuery(e => e.Id == id);
                var model = query.FirstOrDefault();
                return model;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IQueryable<CategoryModel> GetQuery(Expression<Func<CategoryModel, bool>> predicate = null)
        {
            try
            {
                var query = repository.GetEntityQuery("Products").Select(e => new CategoryModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    ProductCount = e.Products.Count
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

        public void Update(CategoryModel model, bool saveChanges = true)
        {
            try
            {
                var entity = repository.GetEntityById(model.Id);
                entity.Name = model.Name;
                repository.UpdateEntity(entity);
                if (saveChanges)
                    SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
