
using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudRecommendation: _ViewCrudBase
  {
    public string Content { get; set; }
    public ViewListSkill Skill { get; set; }
    public string Image { get; set; }
  }
}
