using Manager.Core.Base;
using Manager.Core.Business;
using System;
using System.Collections.Generic;

namespace Manager.Services.WorkModel
{
  public class ManagerNotification
  {
    public BaseFields Manager { get; set; }
    public List<Person> Defeated { get; set; }
    public List<Person> DefeatedNow { get; set; }
    public List<Person> LastSevenDays { get; set; }
    public List<Person> FifteenDays { get; set; }
    public List<Person> ThirtyDays { get; set; }
  }
}
