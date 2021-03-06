﻿using Manager.Views.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewPersonImport
  {
    public string Name { get; set; }
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public EnumStatusUser StatusUser { get; set; }
    public string NameCompany { get; set; }
    public long Registration { get; set; }
    public string NameManager { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public string DocumentManager { get; set; }
    public string NameSchooling { get; set; }
  }
}
