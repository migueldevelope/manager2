using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudUserV2 : _ViewCrudBase
  {
    public DateTime? DateBirth { get; set; }
    public string Document { get; set; }
    public string PhotoUrl { get; set; }
    public EnumSex Sex { get; set; }
    public string CellPhone { get; set; }
    public ViewListSchooling Schooling { get; set; }
  }
}
