using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System;

namespace Manager.Data.Infrastructure
{
  public class MigrationHandle
  {
    private int currentDatabaseVersion = 0;
    public IMongoDatabase Context { get; }

    public MigrationHandle(IMongoDatabase context)
    {
      try
      {
        Context = context;
        var versionCollection = context.GetCollection<Version>("Version");
        var lastVersion = versionCollection.AsQueryable().ToList().LastOrDefault();

        if (lastVersion != null)
        {
          currentDatabaseVersion = lastVersion.Number;
        }
      }
      catch(Exception e)
      {
        throw e;
      }
      
    }

    public void Migrate()
    {
      var migrations = FilterMigrations(GetAllMigrations(), currentDatabaseVersion).Where(m => m != null);

      foreach (var migration in migrations)
      {
        migration.Up(Convert.ToString(currentDatabaseVersion), Context);
      }
    }


    public void Rollback()
    {

    }

    private IEnumerable<IMigration> GetAllMigrations()
    {
      try
      {
        var detail =
        AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
        .Where(x => typeof(IMigration).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
        .Select(t => (IMigration)Activator.CreateInstance(t));

        //var result = detail.ToList<IMigration>();
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private IEnumerable<IMigration> FilterMigrations(IEnumerable<IMigration> migrations, int currentDatabaseVersion)
    {
      foreach (var migration in migrations)
      {
        var attr = migration.GetType().GetCustomAttributes(typeof(MigrationVersionAttribute), false).First() as MigrationVersionAttribute;

        if (attr != null && attr.Number > currentDatabaseVersion)
        {
          yield return migration;
        }
        yield return null;
      }
    }
  }
}
