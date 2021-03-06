﻿using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudPerson : _ViewCrud
  {
    public EnumStatusUser StatusUser { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListOccupationResume Occupation { get; set; }
    public ViewBaseFields Manager { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public DateTime? DateResignation { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public DateTime? HolidayReturn { get; set; }
    public string MotiveAside { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public string Registration { get; set; }
    public int Workload { get; set; }
    public ViewCrudUser User { get; set; }
    public ViewSalaryScalePerson SalaryScales { get; set; }
  }
}
