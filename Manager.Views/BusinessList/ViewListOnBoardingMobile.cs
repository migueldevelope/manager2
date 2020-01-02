using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListOnBoardingMobile: _ViewList
  {
    public ViewListPersonInfo Person { get; set; }
    public ViewListOccupationResume Occupation { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsEnd { get; set; }
    public List<ViewListItensMobile> Itens { get; set; }
    public EnumStatusOnBoarding StatusOnBoarding { get; set; }
  }
}
