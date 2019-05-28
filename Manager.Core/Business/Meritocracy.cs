using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  public class Meritocracy : BaseEntity
  {

    public ViewListPersonMeritocracy Person { get; set; }
    public decimal Maturity { get; set; }
    public decimal ActivitiesExcellence { get; set; }
    public EnumStatusMeritocracy StatusMeritocracy { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
  }
}
