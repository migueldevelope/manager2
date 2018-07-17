using Manager.Core.Base;
using MongoDB.Driver;

namespace Manager.Data
{
  public class DataContext : BaseEntity
  {
    private IMongoClient _client;
    public IMongoDatabase _db;

    public DataContext(string dburl, string dbname)
    {
      _client = new MongoClient(dburl);
      _db = _client.GetDatabase(dbname);
    }

  }
}
