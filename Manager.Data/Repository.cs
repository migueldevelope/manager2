using Manager.Core.Base;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Manager.Views.Enumns;

namespace Manager.Data
{
  public class Repository<T> where T : BaseEntity
  {
    public DataContext _context;
    IMongoCollection<T> _collection;
    public BaseUser _user;

    #region Contructor
    public Repository(DataContext context)
    {
      try
      {
        _context = context;
        _collection = context._db.GetCollection<T>(typeof(T).Name);
        if (_user != null)
          _user._idAccount = _user._idAccount;
        else
          _user = new BaseUser() { _idAccount = "000000000000000000000000" };
      }
      catch
      {
        throw;
      }
    }
    public void User(IHttpContextAccessor contextAccessor)
    {
      _user = new BaseUser();
      foreach (Claim ci in contextAccessor.HttpContext.User.Claims)
      {
        if (ci.Type == ClaimTypes.Name)
          _user.NamePerson = ci.Value;
        if (ci.Type == ClaimTypes.Hash)
          _user._idAccount = ci.Value;
        if (ci.Type == ClaimTypes.Email)
          _user.Mail = ci.Value;
        if (ci.Type == ClaimTypes.NameIdentifier)
          _user.NameAccount = ci.Value;
        if (ci.Type == ClaimTypes.UserData)
          _user._idUser = ci.Value;
      }
    }
    #endregion

    #region Old
    private IQueryable<T> CreateSet()
    {
      try
      {
        return _collection.AsQueryable<T>().Where(p => p._idAccount == _user._idAccount);
      }
      catch
      {
        throw;
      }
    }

    //public T Insert(T entity)
    //{
    //  try
    //  {
    //    entity._idAccount = _user._idAccount;
    //    entity._id = ObjectId.GenerateNewId().ToString();
    //    _collection.InsertOne(entity);

    //    return entity;
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //}

    //public T InsertAccount(T entity)
    //{
    //  try
    //  {
    //    entity._id = ObjectId.GenerateNewId().ToString();
    //    _collection.InsertOne(entity);

    //    return entity;
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //}

    //public T InsertAccountId(T entity)
    //{
    //  try
    //  {
    //    _collection.InsertOne(entity);

    //    return entity;
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //}

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

    //public int Count(Expression<Func<T, bool>> predicate = null)
    //{
    //  try
    //  {
    //    var set = CreateSet();
    //    return (predicate == null ? set.Count() : set.Where(p => p._idAccount == _user._idAccount).Count(predicate));
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //}

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
    #endregion

    #region Methods Free Account
    public async Task<T> InsertAccountNewVersion(T entity)
    {
      try
      {
        entity._id = ObjectId.GenerateNewId().ToString();
        entity._idAccount = entity._id;
        entity.Status = EnumStatus.Enabled;
        await _collection.InsertOneAsync(entity);
        return entity;
      }
      catch
      {
        throw;
      }
    }
    public async Task<T> InsertFreeNewVersion(T entity)
    {
      try
      {
        await _collection.InsertOneAsync(entity);
        return entity;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<T>> GetAllFreeNewVersion(Expression<Func<T, bool>> filter, int count, int skip, string sort)
    {
      try
      {
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        string[] fieldName = sort.Split(' ');
        int order = fieldName.Count() == 1 ? 1 : fieldName[1].ToUpper().Equals("DESC") ? -1 : 1;
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation,
          Limit = count,
          Skip = skip,
          Sort = new BsonDocument(fieldName[0], order)
        };
        IAsyncCursor<T> result = await _collection.FindAsync(filter, findOptions);
        return result.ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<T>> GetAllFreeNewVersion(Expression<Func<T, bool>> filter)
    {
      try
      {
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation
        };
        IAsyncCursor<T> result = await _collection.FindAsync(filter, findOptions);
        return result.ToList();
      }
      catch
      {
        throw;
      }
    }
    public async Task<T> GetFreeNewVersion(Expression<Func<T, bool>> filter)
    {
      try
      {
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation
        };
        IAsyncCursor<T> result = await _collection.FindAsync(filter, findOptions);
        return result.FirstOrDefault();
      }
      catch
      {
        throw;
      }
    }
    public async Task<List<T>> GetAllFreeNewVersion()
    {
      try
      {
        FilterDefinition<T> filter = FilterDefinition<T>.Empty;
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation
        };
        IAsyncCursor<T> result = await _collection.FindAsync(filter, findOptions);
        return result.ToList();
      }
      catch
      {
        throw;
      }
    }

    public async Task<long> CountFreeNewVersion(Expression<Func<T, bool>> filter)
    {
      try
      {
        long result = await _collection.CountDocumentsAsync(filter);
        return result;
      }
      catch
      {
        throw;
      }
    }
    #endregion

    #region Methods
    public async Task<T> InsertNewVersion(T entity)
    {
      try
      {
        entity._idAccount = _user._idAccount;
        entity.Status = EnumStatus.Enabled;
        await _collection.InsertOneAsync(entity);
        return entity;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //public async Task<T> UpdateNewVersion(T entity)
    //{
    //  try
    //  {
    //    T entityOld = GetNewVersion(p => p._id == entity._id).Result;
    //    Type type = entity.GetType();
    //    IEnumerable<string> unequalProperties =
    //        from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
    //        let selfValue = type.GetProperty(pi.Name).GetValue(entityOld, null)
    //        let toValue = type.GetProperty(pi.Name).GetValue(entity, null)
    //        where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
    //        select pi.Name;
    //    if (unequalProperties.Count() != 0)
    //    {
    //      FilterDefinition<T> filter = Builders<T>.Filter.Where(p => p._idAccount == _user._idAccount && p._id == entity._id);
    //      List<UpdateDefinition<T>> entityChange = new List<UpdateDefinition<T>>();
    //      foreach (string item in unequalProperties)
    //        entityChange.Add(Builders<T>.Update.Set(item, entity.GetType().GetProperty(item).GetValue(entity, null)));
    //      entity = await _collection.FindOneAndUpdateAsync(filter, Builders<T>.Update.Combine(entityChange));
    //    }
    //    return entity;
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //}
    public async Task<List<T>> GetAllNewVersion(Expression<Func<T, bool>> filter, int count, int skip, string sort)
    {
      try
      {
        var @params = filter.Parameters;
        // Account, Status
        var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(@params[0], "Status"), Expression.Constant(EnumStatus.Enabled));
        var checkAccount = Expression.Equal(Expression.PropertyOrField(@params[0], "_idAccount"), Expression.Constant(_user._idAccount));
        var originalBody = filter.Body;
        var midleExpr = Expression.And(originalBody, checkAccount);
        var fullExpr = Expression.And(midleExpr, checkNotDeleted);
        var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, @params);
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        string[] fieldName = sort.Split(' ');
        int order = fieldName.Count() == 1 ? 1 : fieldName[1].ToUpper().Equals("DESC") ? -1 : 1;
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation,
          Limit = count,
          Skip = skip,
          Sort = new BsonDocument(fieldName[0], order)
        };
        IAsyncCursor<T> result = await _collection.FindAsync(lambda, findOptions);
        return result.ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<T>> GetAllNewVersion(Expression<Func<T, bool>> filter)
    {
      try
      {
        var @params = filter.Parameters;
        // Account, Status
        var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(@params[0], "Status"), Expression.Constant(EnumStatus.Enabled));
        var checkAccount = Expression.Equal(Expression.PropertyOrField(@params[0], "_idAccount"), Expression.Constant(_user._idAccount));
        var originalBody = filter.Body;
        var midleExpr = Expression.And(originalBody, checkAccount);
        var fullExpr = Expression.And(midleExpr, checkNotDeleted);
        var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, @params);
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation
        };
        IAsyncCursor<T> result = await _collection.FindAsync(lambda, findOptions);
        return result.ToList();
      }
      catch
      {
        throw;
      }
    }
    public List<T> GetAllNewVersion()
    {
      try
      {
        return _collection.AsQueryable<T>().Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled).ToList();
      }
      catch
      {
        throw;
      }
    }
    public async Task<long> CountNewVersion(Expression<Func<T, bool>> filter)
    {
      try
      {
        var @params = filter.Parameters;
        // Account, Status
        var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(@params[0], "Status"), Expression.Constant(EnumStatus.Enabled));
        var checkAccount = Expression.Equal(Expression.PropertyOrField(@params[0], "_idAccount"), Expression.Constant(_user._idAccount));
        var originalBody = filter.Body;
        var midleExpr = Expression.And(originalBody, checkAccount);
        var fullExpr = Expression.And(midleExpr, checkNotDeleted);
        var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, @params);
        long result = await _collection.CountDocumentsAsync(lambda);
        return result;
      }
      catch
      {
        throw;
      }
    }
    public async Task<T> GetNewVersion(Expression<Func<T, bool>> filter)
    {
      try
      {
        var @params = filter.Parameters;
        // Account, Status
        var checkNotDeleted = Expression.Equal(Expression.PropertyOrField(@params[0], "Status"), Expression.Constant(EnumStatus.Enabled));
        var checkAccount = Expression.Equal(Expression.PropertyOrField(@params[0], "_idAccount"), Expression.Constant(_user._idAccount));
        var originalBody = filter.Body;
        var midleExpr = Expression.And(originalBody, checkAccount);
        var fullExpr = Expression.And(midleExpr, checkNotDeleted);
        var lambda = Expression.Lambda<Func<T, bool>>(fullExpr, @params);
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation
        };
        IAsyncCursor<T> result = await _collection.FindAsync(lambda, findOptions);
        return result.FirstOrDefault();
      }
      catch
      {
        throw;
      }
    }
    public async Task<T> GetNewVersion(string id)
    {
      try
      {
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        var findOptions = new FindOptions<T>
        {
          Collation = _caseInsensitiveCollation
        };
        IAsyncCursor<T> result = await _collection.FindAsync(p => p._idAccount == _user._idAccount && p._id == id && p.Status == EnumStatus.Enabled, findOptions);
        return result.FirstOrDefault();
      }
      catch
      {
        throw;
      }
    }
    public bool Exists(string name)
    {
      try
      {
        var filter = new BsonDocument("name", name);
        return _context._db.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter }).Result.Any();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}
