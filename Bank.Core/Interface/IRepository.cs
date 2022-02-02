using Bank.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Interface
{
    public  interface IRepository
    {
        T GetById<T>(int id) where T : BaseEntity;
        T GetById<T>(int id, string include) where T : BaseEntity;
        List<T> List<T>(ISpecification<T> spec) where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
    }
}
