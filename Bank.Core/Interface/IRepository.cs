using Bank.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Interface
{
    public  interface IRepository
    {
        T GetById<T>(int id) where T : BaseModel;
        T GetById<T>(int id, string include) where T : BaseModel;
        List<T> List<T>(ISpecification<T> spec = null) where T : BaseModel;
        T Add<T>(T entity) where T : BaseModel;
        void Update<T>(T entity) where T : BaseModel;
        void Delete<T>(T entity) where T : BaseModel;
    }
}
