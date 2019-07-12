using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Data.Infrastructure.Migrations
{
  [MigrationVersion(1)]
  public class Migration_add_database_version : IMigration
  {
    public void Up(string _idNextMigration, IMongoDatabase context)
    {
      try
      {
        var script = @"db.Version.insert({ Number: 1, Date: ISODate(""" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss") + @""")});";

        var doc = new BsonDocument()
        {
            { "eval", script }
        };
        var command = new BsonDocumentCommand<BsonDocument>(doc);
        var response = context.RunCommand(command);


      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void Down(string _idPrevMigration, IMongoDatabase context)
    {
      throw new NotImplementedException();
    }

  }
}
