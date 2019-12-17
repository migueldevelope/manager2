using System;

namespace Manager.Views.BusinessList
{
  public class ViewListMeritocracyResume: _ViewList
  {
    public decimal ResultEnd { get; set; }
    public string Name { get; set; }
    public string Manager { get; set; }
    public string Occupation { get; set; }
    public string Photo { get; set; }
    public bool ShowPerson { get; set; }
    public DateTime? DateEnd { get; set; }

  }
}
