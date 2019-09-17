using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - entidade da turma
  /// </summary>
  public class Entity : BaseEntity
  {
    public string Name { get; set; }
    public ViewCrudEntity GetCrudEntity()
    {
      return new ViewCrudEntity()
      {
        _id = _id,
        Name = Name
      };
    }
  }
}
