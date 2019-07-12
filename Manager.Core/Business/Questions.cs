using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - perguntas
  /// </summary>
  public class Questions : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public ViewListCompany Company { get; set; }
    public EnumTypeQuestion TypeQuestion { get; set; }
    public long Order { get; set; }
  
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    public EnumTypeRotine TypeRotine { get; set; }
    public ViewListQuestions GetViewList()
    {
      return new ViewListQuestions()
      {
        _id = _id,
        Name = Name,
        Content = Content,
        TypeRotine = TypeRotine,
        Order = Order
      };
    }
  }
}
