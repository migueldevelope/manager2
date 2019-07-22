using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class RecommendationPerson : BaseEntity
  {
    public ViewListPersonBase Person { get; set; }
    public ViewListRecommendation Recommendation { get; set; }
    public string Content { get; set; }
    public string Comments { get; set; }
    public ViewListRecommendationPerson GetViewList()
    {
      return new ViewListRecommendationPerson()
      {
        _id = _id,
        _idRecommendation = Recommendation._id,
        NameRecommendation = Recommendation.Name,
        _idPerson = Person._id,
        NamePerson = Person.Name,
        Image = Recommendation.Image
      };
    }
    public ViewCrudRecommendationPerson GetViewCrud()
    {
      return new ViewCrudRecommendationPerson()
      {
        _id = _id,
        Person = Person,
        Recommendation = Recommendation,
        Comments = Comments,
        Content = Content
      };
    }
  }
}
