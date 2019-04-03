﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewExportStatusCheckpoint
  {
    public string NameManager { get; set; }
    public string NamePerson { get; set; }
    public string Status { get; set; }
    public string Occupation { get; set; }
    public string Result { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
  }
}
