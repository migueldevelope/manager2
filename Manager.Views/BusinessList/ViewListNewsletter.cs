﻿using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListNewsletter:_ViewList
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public EnumTypeNewsletter TypeNewsletter { get; set; }
  }
}
