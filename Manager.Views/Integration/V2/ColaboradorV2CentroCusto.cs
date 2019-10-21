using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2CentroCusto : IColaboradorV2
  {
    public ColaboradorV2Base Colaborador { get; set; }
    public string CentroCusto { get; set; }
    public string NomeCentroCusto { get; set; }
    public DateTime? DataTrocaCentroCusto { get; set; }
    // Chaves de cálculo interno
    public string ChaveCentroCusto => string.Format("{0};{1}", Colaborador.Empresa, CentroCusto);
  }
}
