using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Manager.Core.Interfaces
{
  public interface IServiceGeneric<T>
  {
    void User(IHttpContextAccessor contextAccessor);

    IQueryable<T> CreateSet();

    T Insert(T entity);

    T InsertAccount(T entity);

    void Update(T entity, FilterDefinition<T> filter);

    void UpdateAccount(T entity, FilterDefinition<T> filter);

    long Delete(string id, bool logical = false);

    T Single(Expression<Func<T, bool>> predicate = null);

    IEnumerable<T> GetAllNewVersion();

    IQueryable<T> GetAllNewVersion(Expression<Func<T, bool>> filter);

    IQueryable<T> GetAuthentication(Expression<Func<T, bool>> filter);

    int Count(Expression<Func<T, bool>> predicate = null);

    bool Exists(Expression<Func<T, bool>> predicate);
  }
}
