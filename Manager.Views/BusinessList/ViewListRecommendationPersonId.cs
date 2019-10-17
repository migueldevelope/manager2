using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListRecommendationPersonId: _ViewList
  {
    public string NameRecommendation { get; set; }
    public string Content { get; set; }
    public string Image { get; set; }
    public string Comments { get; set; }
    public bool Read { get; set; }
  }
}
