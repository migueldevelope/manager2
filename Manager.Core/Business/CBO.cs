using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados1
  /// </summary>
  public class Cbo : BaseEntity
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public ViewListCbo GetViewList()
    {
      return new ViewListCbo()
      {
        _id = _id,
        Name = Name,
        Code = Code
      };
    }
    public ViewCrudCbo GetViewCrud()
    {
      return new ViewCrudCbo()
      {
        _id = _id,
        Name = Name,
        Code = Code
      };
    }
  }
}
