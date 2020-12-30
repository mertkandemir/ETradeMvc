using AppCore.Business.Bases;
using ETradeBusiness.Models;
using ETradeEntities.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using AppCore.DataAccess.Repositories.Bases;

namespace ETradeBusiness.Services
{
    public class UserService : IService<User, UserModel>
    {
        private readonly RepositoryBase<User> userRepository;
        private readonly RepositoryBase<Role> roleRepository;

        public UserService(RepositoryBase<User> userRepositoryParameter, RepositoryBase<Role> roleRepositoryParameter)
        {
            userRepository = userRepositoryParameter;
            roleRepository = roleRepositoryParameter;
        }

        public void Add(UserModel model, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserModel> GetQuery(Expression<Func<UserModel, bool>> predicate = null)
        {
            try
            {
                var userQuery = userRepository.GetEntityQuery();
                var roleQuery = roleRepository.GetEntityQuery();
                var innerJoinQuery = from users in userQuery
                                     join roles in roleQuery
                                         on users.RoleId equals roles.Id
                                     where users.Active
                                     select new UserModel()
                                     {
                                         Id = users.Id,
                                         UserName = users.UserName,
                                         Password = users.Password,
                                         EMail = users.EMail,
                                         BirthDay = users.BirthDay,
                                         RoleId = users.RoleId,
                                         Role = new RoleModel()
                                         {
                                             Id = roles.Id,
                                             Name = roles.Name
                                         }
                                     };

                //var leftJoinQuery = from users in userQuery
                //                    join roles in roleQuery
                //                        on users.RoleId equals roles.Id into users_roles
                //                    from in_users_roles in users_roles.DefaultIfEmpty()
                //                    where users.Active
                //                    select new UserModel()
                //                    {
                //                        Id = users.Id,
                //                        UserName = users.UserName,
                //                        Password = users.Password,
                //                        EMail = users.EMail,
                //                        BirthDay = users.BirthDay,
                //                        RoleId = users.RoleId,
                //                        Role = new RoleModel()
                //                        {
                //                            Id = in_users_roles.Id,
                //                            Name = in_users_roles.Name
                //                        }
                //                    };


                if (predicate != null)
                {
                    innerJoinQuery = innerJoinQuery.Where(predicate);
                }

                return innerJoinQuery;
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

        public void Update(UserModel model, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }
    }
}
