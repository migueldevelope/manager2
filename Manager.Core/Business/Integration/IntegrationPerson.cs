using Manager.Core.Base;
using Manager.Views.Integration;

namespace Manager.Core.Business.Integration
{
  public class IntegrationPerson : BaseEntity
  {
    public string Key { get; set; }
    public ViewColaborador Employee { get; set; }
  }
}
