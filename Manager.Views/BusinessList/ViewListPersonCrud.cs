using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonCrud : ViewListPerson
  {
    public EnumStatusUser StatusUser { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public string Occupation { get; set; }
    public string Manager { get; set; }
  }
}
