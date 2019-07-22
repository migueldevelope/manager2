using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudRecommendationPerson: _ViewCrud
  {
    public ViewListPersonBase Person { get; set; }
    public ViewListRecommendation Recommendation { get; set; }
    public string Content { get; set; }
    public string Comments { get; set; }
  }
}
