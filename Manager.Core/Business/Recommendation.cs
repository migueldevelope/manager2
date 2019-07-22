using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  public class Recommendation: BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public ViewListSkill Skill { get; set; }
    public string Image { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    public ViewListRecommendation GetViewList()
    {
      return new ViewListRecommendation()
      {
        _id = _id,
        Name = Name,
        Image = Image
      };
    }

    public ViewCrudRecommendation GetViewCrud()
    {
      return new ViewCrudRecommendation()
      {
        _id = _id,
        Name = Name,
        Content = Content,
        Image = Image,
        Skill = Skill
      };
    }

  }
}
