using Manager.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Data.Infrastructure
{
  public interface IMigration
  {
    void Up(string _idNextMigration, IMongoDatabase context);
    void Down(string _idPrevMigration, IMongoDatabase context);
  }

  [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  sealed class MigrationVersionAttribute : Attribute
  {
    private readonly int number;

    public MigrationVersionAttribute(int number)
    {
      this.number = number;
    }

    public int Number => number;
  }
}
