using System;
using System.Linq.Expressions;

namespace Bank.Core.Interface
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
