using System;

namespace Manager.Core.Views.Integration
{
  public class ViewIntegrationPersonV1
  {
    public string _id { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string Document { get; set; }
    public DateTime? DateBirth { get; set; }
    public string CellPhone { get; set; }
    public string Phone { get; set; }
    public string DocumentId { get; set; }
    public string DocumentProfessional { get; set; }
    public string Sex { get; set; }
    public ViewIntegrationMapOfV1 Schooling { get; set; }
    public ViewIntegrationContractV1 Contract { get; set; }
  }
}
