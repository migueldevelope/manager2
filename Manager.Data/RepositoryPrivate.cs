using Manager.Core.BaseV2;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Manager.Data
{
  public class RepositoryPrivate<T> where T : BasePrivate
  {
    private readonly IMongoCollection<T> mongoCollection;
    private BaseUserV2 baseUserV2;

    #region Contructor
    public RepositoryPrivate(DataContext context)
    {
      try
      {
        mongoCollection = context._db.GetCollection<T>(typeof(T).Name);
        baseUserV2 = new BaseUserV2() { _idPerson = null, _idAccount = null, _idUser = null };
      }
      catch
      {
        throw;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      baseUserV2 = new BaseUserV2() { _idAccount = null, _idPerson = null, _idUser = null };
      foreach (Claim ci in contextAccessor.HttpContext.User.Claims)
      {
        if (ci.Type == ClaimTypes.Hash)
        {
          baseUserV2._idAccount = ci.Value;
        }
        if (ci.Type == ClaimTypes.NameIdentifier)
        {
          baseUserV2.NameAccount = ci.Value;
        }
        if (ci.Type == ClaimTypes.UserData)
        {
          baseUserV2._idUser = ci.Value;
        }
        if (ci.Type == ClaimTypes.Actor)
        {
          baseUserV2._idPerson = ci.Value;
        }
        if (ci.Type == ClaimTypes.Email)
        {
          baseUserV2.Mail = ci.Value;
        }
        if (ci.Type == ClaimTypes.Name)
        {
          baseUserV2.NamePerson = ci.Value;
        }
      }
    }
    public BaseUserV2 GetUser()
    {
      return baseUserV2;
    }
    #endregion

    #region Methods
    public virtual async Task<T> InsertAsync(T entity)
    {
      try
      {
        // Initialize control document
        entity._id = ObjectId.GenerateNewId().ToString();
        entity._disabled = false;
        entity._included = DateTime.UtcNow;
        entity._altered = entity._included;
        entity._includedUser = baseUserV2._idUser;
        entity._alteredUser = baseUserV2._idUser;
        entity._includedPerson = baseUserV2._idPerson;
        entity._alteredPerson = baseUserV2._idPerson;
        entity._idAccount = baseUserV2._idAccount;
        entity._change = 0;
        await mongoCollection.InsertOneAsync(entity);
        return entity;
      }
      catch
      {
        throw;
      }
    }
    public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
      try
      {
        T entity = await mongoCollection.FindAsync(Builders<T>.Filter.And(filter,
              Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
              Builders<T>.Filter.Eq(f => f._disabled, false)
            )
          ).Result.FirstOrDefaultAsync();
        if (entity == null)
        {
          throw new Exception(string.Format("{0} not found!", typeof(T).Name));
        }
        return entity;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
      try
      {
        return await mongoCollection.FindAsync(Builders<T>.Filter.And(filter,
            Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
            Builders<T>.Filter.Eq(f => f._disabled, false)
          ),
          new FindOptions<T>
          {
            Collation = new Collation("en", strength: CollationStrength.Primary)
          }).Result.ToListAsync();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string sort)
    {
      try
      {
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        // TODO: ver split de campos para ordem
        string[] fieldName = sort.Split(' ');
        int order = fieldName.Count() == 1 ? 1 : fieldName[1].ToUpper().Equals("DESC") ? -1 : 1;
        return await mongoCollection.FindAsync(Builders<T>.Filter.And(filter,
                                               Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
                                               Builders<T>.Filter.Eq(f => f._disabled, false)),
         new FindOptions<T>
         {
           Collation = _caseInsensitiveCollation,
           Sort = new BsonDocument(fieldName[0], order)
         })
          .Result.ToListAsync();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, int count, int skip, string sort)
    {
      try
      {
        Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
        string[] fieldName = sort.Split(' ');
        int order = fieldName.Count() == 1 ? 1 : fieldName[1].ToUpper().Equals("DESC") ? -1 : 1;
        return await mongoCollection.FindAsync(Builders<T>.Filter.And(filter,
                                               Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
                                               Builders<T>.Filter.Eq(f => f._disabled, false)),
          new FindOptions<T>
          {
            Collation = _caseInsensitiveCollation,
            Limit = count,
            Skip = skip,
            Sort = new BsonDocument(fieldName[0], order)
          })
          .Result.ToListAsync();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //public virtual async Task<T> UpdateAsync(string _id, long _change, UpdateDefinition<T> update)
    //{
    //  try
    //  {
    //    // Change control document
    //    update = update.Inc(p => p._change, 1)
    //                   .Set(p => p._altered, DateTime.UtcNow)
    //                   .Set(p => p._alteredUser, baseUserV2._idUser);

    //    // Update document
    //    T entity = await mongoCollection.FindOneAndUpdateAsync(
    //      Builders<T>.Filter.And(Builders<T>.Filter.Eq(f => f._id, _id),
    //                             Builders<T>.Filter.Eq(f => f._disabled, false),
    //                             Builders<T>.Filter.Eq(f => f._change, _change)),
    //      update,
    //      new FindOneAndUpdateOptions<T> { ReturnDocument = ReturnDocument.After });
    //    if (entity == null)
    //    {
    //      throw new Exception("Data not found! Restart delete!");
    //    }
    //    return entity;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    public virtual async Task<T> UpdateAsync(T entity, long _change)
    {
      try
      {
        entity._change++;
        entity._altered = DateTime.UtcNow;
        entity._alteredUser = baseUserV2._idUser;
        entity._alteredPerson = baseUserV2._idPerson;
        entity = await mongoCollection.FindOneAndReplaceAsync(
          p => p._idAccount == baseUserV2._idAccount && p._id == entity._id && p._disabled == false && p._change == _change,
          entity);
        if (entity == null)
        {
          throw new Exception("Data not found! Restart delete!");
        }
        return entity;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public virtual async Task<T> DeleteAsync(string _id, long _change)
    {
      try
      {
        // Change control document
        UpdateDefinition<T> update = Builders<T>.Update.Inc("_change", 1)
                                        .Set("_disabled", true)
                                        .Set("_altered", DateTime.UtcNow)
                                        .Set("_alteredUser", baseUserV2._idUser)
                                        .Set("_alteredPerson", baseUserV2._idPerson);
        T result = await mongoCollection.FindOneAndUpdateAsync(
          Builders<T>.Filter.And(Builders<T>.Filter.Eq(f => f._id, _id),
                                 Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
                                 Builders<T>.Filter.Eq(f => f._disabled, false),
                                 Builders<T>.Filter.Eq(f => f._change, _change)),
          update,
          new FindOneAndUpdateOptions<T> { ReturnDocument = ReturnDocument.After });
        // Teste para o controle de alteração
        if (result == null)
        {
          throw new Exception("Data not found! Restart delete!");
        }
        return result;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public virtual async Task<long> DeletePhiscalAsync(string _id, long _change)
    {
      try
      {
        DeleteResult result = await mongoCollection.DeleteOneAsync(
          Builders<T>.Filter.And(Builders<T>.Filter.Eq(f => f._id, _id),
                                 Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
                                 Builders<T>.Filter.Eq(f => f._disabled, false),
                                 Builders<T>.Filter.Eq(f => f._change, _change)
          )
        );
        // Teste para o controle de alteração
        if (result.DeletedCount == 0)
        {
          throw new Exception("Data not found! Restart delete!");
        }
        return result.DeletedCount;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public virtual async Task<long> CountAsync(Expression<Func<T, bool>> filter)
    {
      try
      {
        return await mongoCollection.
          CountDocumentsAsync(Builders<T>.Filter.And(filter,
                              Builders<T>.Filter.Eq(f => f._idAccount, baseUserV2._idAccount),
                              Builders<T>.Filter.Eq(f => f._disabled, false)));
      }
      catch
      {
        throw;
      }
    }
    #endregion

  }
}

