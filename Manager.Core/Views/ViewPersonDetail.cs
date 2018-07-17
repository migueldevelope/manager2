using Manager.Core.Business;
using System;
namespace Manager.Core.Views
{
  public class ViewPersonDetail
  {
    public string IdPerson { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public Occupation Occuaption { get; set; }
    public DateTime? Birth { get; set; }
  }
}
