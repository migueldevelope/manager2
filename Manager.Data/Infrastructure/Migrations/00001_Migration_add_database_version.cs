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
      
    }

    public void Down(string _idPrevMigration, IMongoDatabase context)
    {
      throw new NotImplementedException();
    }

  }
}
