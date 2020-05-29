using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudImpedimentsIniciatives
  {
    public string Description { get; set; }
    public List<ViewCrudLike> Like { get; set; }
    public List<ViewCrudLike> Deslike { get; set; }
  }
}
