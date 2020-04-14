using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudStep
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idSalaryScale { get; set; }
    public string _idGrade { get; set; }
    public EnumSteps Step { get; set; }
    public decimal Salary { get; set; }
  }
}
