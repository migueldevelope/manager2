using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Axis : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
    public Axis Template { get; set; }
    public Company Company { get; set; }
    public ViewListAxis GetViewList()
    {
      return new ViewListAxis()
      {
        _id = _id,
        Name = Name,
        TypeAxis = TypeAxis
      };
    }
    public ViewCrudAxis GetViewCrud()
    {
      return new ViewCrudAxis()
      {
        _id = _id,
        Name = Name,
        TypeAxis = TypeAxis,
        Company = Company.GetViewList()
      };
    }
  }
}
