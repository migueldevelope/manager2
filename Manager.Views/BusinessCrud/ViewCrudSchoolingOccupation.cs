namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSchoolingOccupation : ViewCrudSchooling
  {

    public string Complement { get; set; }

    public ViewCrudSchooling GetViewCrud()
    {
      return new ViewCrudSchooling()
      {
        _id = _id,
        Name = Name,
        Order = Order,
        Type = Type
      };
    }

    public ViewCrudSchoolingOccupation GetViewCrudOccupation()
    {
      return new ViewCrudSchoolingOccupation()
      {
        _id = _id,
        Name = Name,
        Order = Order,
        Complement = Complement,
        Type = Type
      };
    }
  }
}
