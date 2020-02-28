using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudNewsletter: _ViewCrud
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public DateTime? Included { get; set; }
  }
}
