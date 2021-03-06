﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewCertificate
  {
    public string NameEvent { get; set; }
    public string NameCourse { get; set; }
    public string NameEntity { get; set; }
    public string Content { get; set; }
    public string DateBegin { get; set; }
    public string DateEnd { get; set; }
    public decimal Workload { get; set; }
    public string Instructor { get; set; }
    public string NameParticipant { get; set; }
    public string Code { get; set; }
  }
}
