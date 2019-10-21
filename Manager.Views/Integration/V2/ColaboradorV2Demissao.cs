using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Demissao : IColaboradorV2
  {
    public ColaboradorV2Base Colaborador { get; set; }
    public DateTime? DataDemissao { get; set; }
  }
}
