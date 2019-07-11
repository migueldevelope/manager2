using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Sphere : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSphere TypeSphere { get; set; }
    public ViewListCompany Company { get; set; }
    public string Template { get; set; }
    public ViewListSphere GetViewList()
    {
      return new ViewListSphere()
      {
        _id = _id,
        Name = Name,
        TypeSphere = TypeSphere
      };
    }
  }
}
