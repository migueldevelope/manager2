using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudParticipant: _ViewCrud
  {
    public ViewListPerson Person { get; set; }
    public List<ViewCrudFrequencyEvent> FrequencyEvent { get; set; }
    public bool Approved { get; set; }
    public bool ApprovedGrade { get; set; }
    public decimal Grade { get; set; }
    public string Name { get; set; }
    public EnumTypeParticipant TypeParticipant { get; set; }
  }
}
