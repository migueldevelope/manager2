using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  class ViewListCheckpoint : _ViewList
  {
    public string _idPerson { get; set; }
    public string OccupationName { get; set; }
    public EnumStatusCheckpoint StatusCheckpoint { get; set; }
    public EnumCheckpoint TypeCheckpoint { get; set; }
  }
}
