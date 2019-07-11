using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Establishment : BaseEntity
  {
    public ViewListCompany Company { get; set; }
    public string Name { get; set; }
    public ViewListEstablishment GetViewList()
    {
      return new ViewListEstablishment()
      {
        _id = _id,
        Name = Name
      };
    }
    public ViewCrudEstablishment GetViewCrud()
    {
      return new ViewCrudEstablishment()
      {
        _id = _id,
        Name = Name,
        Company = Company
      };
    }
  }
}
