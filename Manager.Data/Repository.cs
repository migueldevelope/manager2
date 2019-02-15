using Manager.Core.Base;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Manager.Core.Enumns;
using System.Linq.Expressions;
using Manager.Core.Interfaces;

namespace Manager.Data
{
  public class Repository<T> where T : BaseEntity
  {
    public DataContext _context;
    IMongoCollection<T> _collection;
    Object _service;
    public string _idAccount;
    public BaseUser _user;

    public Repository(DataContext context)
    {
      try
      {
        _context = context;
        _collection = context._db.GetCollection<T>(typeof(T).Name);
        if (_user != null)
          _user._idAccount = _user._idAccount;
        else
        {
          _user = new BaseUser()
          {
            _idAccount = "000000000000000000000000"
          };
        }

      }
      catch
      {
        throw;
      }
    }

    public Repository(DataContext context, string idAccount)
    {
      try
      {
        _context = context;
        _collection = _context._db.GetCollection<T>(typeof(T).Name);
        if (_user != null)
          _user._idAccount = _user._idAccount;
        else
        {
          _user = new BaseUser()
          {
            _idAccount = "000000000000000000000000"
          };
        }
      }
      catch
      {
        throw;
      }
    }

    public Repository(DataContext context, Object service)
    {
      try
      {
        _context = context;
        _service = service;
        _collection = _context._db.GetCollection<T>(typeof(T).Name);
        if (_user != null)
          _user._idAccount = _user._idAccount;
        else
        {
          _user = new BaseUser()
          {
            _idAccount = "000000000000000000000000"
          };
        }
      }
      catch
      {
        throw;
      }
    }

    public Repository(DataContext context, string idAccount, Object service)
    {
      try
      {
        _context = context;
        _service = service;
        _collection = _context._db.GetCollection<T>(typeof(T).Name);
        if (_user != null)
          _user._idAccount = _user._idAccount;
        else
        {
          _user = new BaseUser()
          {
            _idAccount = idAccount
          };
        }
      }
      catch
      {
        throw;
      }
    }

    public void User(IHttpContextAccessor contextAccessor)
    {
      BaseUser baseUser = new BaseUser();
      var user = contextAccessor.HttpContext.User;
      foreach (Claim ci in user.Claims)
      {
        if (ci.Type == ClaimTypes.Name)
          baseUser.NamePerson = ci.Value;

        if (ci.Type == ClaimTypes.Hash)
          baseUser._idAccount = ci.Value;

        if (ci.Type == ClaimTypes.Email)
          baseUser.Mail = ci.Value;

        if (ci.Type == ClaimTypes.NameIdentifier)
          baseUser.NameAccount = ci.Value;

        if (ci.Type == ClaimTypes.UserData)
          baseUser._idPerson = ci.Value;

      }

      _user = baseUser;
    }

    private IQueryable<T> CreateSet()
    {
      try
      {
        return _collection.AsQueryable<T>().Where(p => p._idAccount == _idAccount);
      }
      catch
      {
        throw;
      }
    }

    public T Insert(T entity)
    {
      try
      {
        entity._idAccount = _user._idAccount;
        entity._id = ObjectId.GenerateNewId().ToString();
        _collection.InsertOne(entity);

        return entity;
      }
      catch
      {
        throw;
      }
    }

    public T InsertAccount(T entity)
    {
      try
      {
        entity._id = ObjectId.GenerateNewId().ToString();
        _collection.InsertOne(entity);

        return entity;
      }
      catch
      {
        throw;
      }
    }

    public T InsertAccountId(T entity)
    {
      try
      {
        _collection.InsertOne(entity);

        return entity;
      }
      catch
      {
        throw;
      }
    }

    public void Update(T entity, FilterDefinition<T> filter)
    {
      try
      {
        entity._idAccount = _user._idAccount;
        if (filter == null)
        {

          filter = Builders<T>.Filter.Where(p => p._id == entity._id & p._idAccount == _user._idAccount);
        }
        _collection.ReplaceOneAsync(filter, entity);
      }
      catch
      {
        throw;
      }
    }

    public void UpdateAccount(T entity, FilterDefinition<T> filter)
    {
      try
      {
        if (filter == null)
        {

          filter = Builders<T>.Filter.Where(p => p._id == entity._id);
        }
        _collection.ReplaceOneAsync(filter, entity);
      }
      catch
      {
        throw;
      }
    }

    public long Delete(string id, bool logical = false)
    {
      try
      {
        DeleteResult resultDelete;
        if (logical)
        {
          var entity = GetAll(p => p._id == id).FirstOrDefault();
          entity.Status = EnumStatus.Disabled;
          var filter = Builders<T>.Filter.Where(p => p._id == entity._id);
          var result = _collection.ReplaceOneAsync(filter, entity);
          return result.Result.ModifiedCount;
        }
        else
        {
          resultDelete = _collection.DeleteOne(s => s._id == id & s._idAccount == _user._idAccount);
          return resultDelete.DeletedCount;
        }
      }
      catch
      {
        throw;
      }
    }
 
    public T Single(Expression<Func<T, bool>> predicate = null)
    {
      try
      {
        var set = CreateSet();
        var query = (predicate == null ? set : set.Where(p => p._idAccount == _user._idAccount).Where(predicate));
        return query.SingleOrDefault();
      }
      catch
      {
        throw;
      }
    }

    public virtual IQueryable<T> GetAll()
    {
      try
      {
        var result = _collection.AsQueryable<T>().Where(p => p.Status == EnumStatus.Enabled & p._idAccount == _user._idAccount);
        return result;
      }
      catch
      {
        throw;
      }
    }

    public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
    {
      try
      {
        var result = _collection.AsQueryable<T>().Where(p => p.Status == EnumStatus.Enabled & p._idAccount == _user._idAccount).Where(filter);
        return result;
      }
      catch
      {
        throw;
      }
    }

    public virtual IQueryable<T> GetAuthentication(Expression<Func<T, bool>> filter)
    {
      try
      {
        var result = _collection.AsQueryable<T>().Where(filter);
        return result;
      }
      catch
      {
        throw;
      }
    }

    public int Count(Expression<Func<T, bool>> predicate = null)
    {
      try
      {
        var set = CreateSet();
        return (predicate == null ? set.Count() : set.Where(p => p._idAccount == _user._idAccount).Count(predicate));
      }
      catch
      {
        throw;
      }
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
      try
      {
        var set = CreateSet();
        return set.Any(predicate);
      }
      catch
      {
        throw;
      }
    }
  }
}
