using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        //new Dictionary<Type, object>();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //Get Type Name
            var typeName = typeof(TEntity).Name;
            //Check if the repository already exists dictionary <string, object> string key[type name] object value from the generic repository
            // if (_repositories.ContainsKey(typeName))
            //    return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            //  {
            //Return the existing repository
            //    return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            // }
            if (_repositories.TryGetValue(typeName, out object value))
                return (IGenericRepository<TEntity, TKey>)value;

            else
            {
                //Create a new instance of the repository
                var Repo = new GenericRepository<TEntity, TKey>(_dbContext);
                //Store obj in dic 
                _repositories["typeName"] = Repo;
                return Repo;
            }
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    }
}
