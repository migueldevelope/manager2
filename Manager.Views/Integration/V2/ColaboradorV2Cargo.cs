using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Cargo : IColaboradorV2
  {
    public ColaboradorV2Base Colaborador { get; set; }
    public string Cargo { get; set; }
    public string NomeCargo { get; set; }
    public DateTime? DataTrocaCargo { get; set; }
    // Chaves de cálculo interno
    public string ChaveCargo => string.Format("{0};{1}", Colaborador.Empresa, Cargo);
  }
}
