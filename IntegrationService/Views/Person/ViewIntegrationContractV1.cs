using System;

namespace IntegrationService.Views.Person
{
  public class ViewIntegrationContractV1
  {
    public string _id { get; set; }
    public string Document { get; set; }
    public ViewIntegrationMapOfV1 Company { get; set; }
    public long Registration { get; set; }
    public ViewIntegrationMapOfV1 Establishment { get; set; }
    public DateTime? AdmissionDate { get; set; }
    // Enabled = 0, Disabled = 1, Away = 2, Vacation = 3
    public int StatusUser { get; set; }
    public DateTime? VacationReturn { get; set; }
    public string ReasonForRemoval { get; set; }
    public DateTime? ResignationDate { get; set; }
    public ViewIntegrationMapOfV1 Occupation { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public string _IdManager { get; set; }
    public string DocumentManager { get; set; }
    public ViewIntegrationMapOfV1 CompanyManager { get; set; }
    public string RegistrationManager { get; set; }
    public string NameManager { get; set; }
    // Support = 0, Administrator = 1, Manager = 2, Employee = 3, Anonymous = 4, HR = 5, ManagerHR = 6
    public int TypeUser { get; set; }
    // OnBoarding = 0, Monitoring = 1, Checkpoint = 2
    public int TypeJourney { get; set; }
  }
}
