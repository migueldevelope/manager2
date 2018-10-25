﻿using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views.Integration
{
  public class ViewIntegrationContractV1
  {
    public string _id { get; set; }
    public string Document { get; set; }
    public ViewIntegrationMapOfV1 Company { get; set; }
    public long Registration { get; set; }
    public ViewIntegrationMapOfV1 Establishment { get; set; }
    public DateTime? DateAdm { get; set; }
    public EnumStatusUser StatusUser { get; set; }
    public DateTime? HolidayReturn { get; set; }
    public string MotiveAside { get; set; }
    public DateTime? DateResignation { get; set; }
    public ViewIntegrationMapOfV1 Occupation { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public string _IdManager { get; set; }
    public string DocumentManager { get; set; }
    public ViewIntegrationMapOfV1 CompanyManager { get; set; }
    public long RegistrationManager { get; set; }
    public string NameManager { get; set; }
    // Support = 0, Administrator = 1, Manager = 2, Employee = 3, Anonymous = 4, HR = 5, ManagerHR = 6
    public EnumTypeUser TypeUser { get; set; }
    // OnBoarding = 0, Monitoring = 1, Checkpoint = 2, OnBoardingOccupation = 3
    public EnumTypeJourney TypeJourney { get; set; }
  }
}
