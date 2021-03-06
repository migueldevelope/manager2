﻿using System;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonMeritocracy : _ViewListBase
  {
    public DateTime? CompanyDate { get; set; }
    public DateTime? OccupationDate { get; set; }
    public string CurrentSchooling { get; set; }
    public string OccupationSchooling { get; set; }
    public string OccupationName { get; set; }
    public decimal Salary { get; set; }
    public string Photo { get; set; }
  }
}
