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
    public string Phone { get; set; }
    public string PhoneFixed { get; set; }
    public string DocumentID { get; set; }
    public string DocumentCTPF { get; set; }
    public string Sex { get; set; }
    public ViewIntegrationMapOfV1 Schooling { get; set; }
    public ViewIntegrationContractV1 Contract { get; set; }
  }
}
