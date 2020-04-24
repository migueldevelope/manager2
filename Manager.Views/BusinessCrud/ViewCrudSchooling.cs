using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSchooling : _ViewCrudBase
  {

    public long Order { get; set; }
    public EnumTypeSchooling Type { get; set; }
    public ViewCrudSchoolingOccupation GetViewCrud()
    {
      return new ViewCrudSchoolingOccupation()
      {
        Name = Name,
        Order = Order,
        Type = Type,
        _id = _id
      };
    }
  }
}
