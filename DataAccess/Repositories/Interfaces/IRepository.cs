using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> List { get; }
        T Add(T entity);
        void Delete(T entity);
        IQueryable<T> FilterList(Expression<Func<T, bool>> filterFunc);
        IQueryable<T> FilterListWithInclude<TInclude>(Expression<Func<T, bool>> filterFunc, Expression<Func<T, TInclude>> includeFunc) where TInclude : Entity;
        IQueryable<T> FilterListWithIncludeArray<TInclude>(Expression<Func<T, bool>> filterFunc, Expression<Func<T, ICollection<TInclude>>> includeFunc) where TInclude : Entity;
        T FindById(int id);
        T FindByIdWithInclude<TInclude>(int id, Expression<Func<T, TInclude>> includeFunc) where TInclude : Entity;
        T FindByIdWithIncludeArray<TInclude>(int id, Expression<Func<T, ICollection<TInclude>>> includeFunc) where TInclude : Entity;
        IQueryable<T> ListWithInclude<TInclude>(Expression<Func<T, TInclude>> includeFunc) where TInclude : Entity;
        IQueryable<T> ListWithInclude<TInclude1, TInclude2>(Expression<Func<T, TInclude1>> includeFunc1, Expression<Func<T, TInclude2>> includeFunc2)
            where TInclude1 : Entity
            where TInclude2 : Entity;
        IQueryable<T> ListWithIncludeArray<TInclude>(Expression<Func<T, ICollection<TInclude>>> includeFunc) where TInclude : Entity;
        void Update(T entity);
    }
}
