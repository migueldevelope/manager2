using Manager.Core.Base;
using Manager.Views.BusinessList;
using System;

namespace Manager.Core.Business
{
  public class TermsOfService : BaseEntity
  {
    public string Text { get; set; }
    public DateTime? Date { get; set; }
    public ViewListTermsOfService GetViewList()
    {
      return new ViewListTermsOfService()
      {
        _id = _id,
        Text = Text,
        Date = Date
      };
    }
  }
}
