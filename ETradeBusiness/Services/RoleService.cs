using AppCore.Business.Bases;
using ETradeBusiness.Models;
using ETradeEntities.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using AppCore.DataAccess.Repositories.Bases;

namespace ETradeBusiness.Services
{
    public class RoleService : IService<Role, RoleModel>
    {
        private readonly RepositoryBase<Role> repository;

        public RoleService(RepositoryBase<Role> repositoryParameter)
        {
            repository = repositoryParameter;
        }

        public void Add(RoleModel model, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public RoleModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<RoleModel> GetQuery(Expression<Func<RoleModel, bool>> predicate = null)
        {
            try
            {
                var query = repository.GetEntityQuery().Select(e => new RoleModel()
                {
                    Id = e.Id,
                    Name = e.Name
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
            throw new NotImplementedException();
        }

        public void Update(RoleModel model, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }
    }
}
