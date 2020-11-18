
using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
         IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
         // save our changes to the database and
         // return the number of changes to our database
         Task<int> Complete();
    }
}