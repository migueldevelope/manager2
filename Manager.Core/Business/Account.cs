using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Account : BaseEntity
  {
    public string Name { get; set; }
    public string Nickname { get; set; }
    public string InfoClient { get; set; }
    public ViewListAccount GetViewList()
    {
      return new ViewListAccount()
      {
        _id = _id,
        Name = Name
      };
    }
    public ViewCrudAccount GetViewCrud()
    {
      return new ViewCrudAccount()
      {
        _id = _id,
        Name = Name,
        InfoClient = InfoClient
      };
    }
  }
}
