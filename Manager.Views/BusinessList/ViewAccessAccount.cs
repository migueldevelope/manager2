using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewAccessAccount
  {
    public long Last3Month { get; set; }
    public List<ViewAccessAccountDay> Days { get; set; }
  }
}
