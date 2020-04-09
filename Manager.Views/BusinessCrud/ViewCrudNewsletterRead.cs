using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudNewsletterRead:_ViewCrud
  {
    public string _idNewsletter { get; set; }
    public string _idUser { get; set; }
    public DateTime? ReadDate { get; set; }
    public bool DontShow { get; set; }
  }
}
