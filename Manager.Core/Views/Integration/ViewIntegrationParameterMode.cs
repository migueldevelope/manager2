﻿using Manager.Core.Enumns;

namespace Manager.Core.Views.Integration
{
  public class ViewIntegrationParameterMode
  {
    public EnumIntegrationType Type { get; set; }
    public EnumIntegrationMode Mode { get; set; }
    public string ConnectionString { get; set; }
    public string SqlCommand { get; set; }
    public string FilePathLocal { get; set; }
  }
}
