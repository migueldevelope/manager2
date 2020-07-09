using Manager.Core.BaseV2;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.BusinessV2
{
  public class AccountV2 : BasePublic
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
        _change = _change,
        Name = Name,
        Nickname = Nickname,
        InfoClient = InfoClient
      };
    }
  }
}
