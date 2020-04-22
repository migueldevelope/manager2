using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudFeelingDay : _ViewCrud
  {
    public EnumFeeling Feeling { get; set; }
    public string _idUser { get; set; }
    public DateTime? Date { get; set; }
  }
}
