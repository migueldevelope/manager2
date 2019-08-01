using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOccupationSalaryScale
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idGrade { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idSalaryScale { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idOccupation { get; set; }
    public int Workload { get; set; }
    public string NameGrade { get; set; }
    public string NameSalaryScale { get; set; }
  }
}
