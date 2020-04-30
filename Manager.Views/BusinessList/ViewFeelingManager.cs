using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewFeelingManager
  {
    public string _idUser { get; set; }
    public string Name { get; set; }
    public EnumFeeling Felling { get; set; }
    public DateTime? Day { get; set; }
  }
}
