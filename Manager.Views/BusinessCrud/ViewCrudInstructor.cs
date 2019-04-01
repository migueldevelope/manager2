using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudInstructor: _ViewCrudBase
  {
    public string _idPerson { get; set; }
    public string Document { get; set; }
    public string Schooling { get; set; }
    public ViewCrudCbo Cbo { get; set; }
    public string Content { get; set; }
    public EnumTypeInstructor TypeInstructor { get; set; }
  }
}
