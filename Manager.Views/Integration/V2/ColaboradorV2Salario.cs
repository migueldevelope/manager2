using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Salario : ColaboradorV2Base, IColaboradorV2
  {
    // Informações Contrato
    public decimal SalarioNominal { get; set; }
    public int CargaHoraria { get; set; }
    public DateTime DataUltimoReajuste { get; set; }
    public string MotivoUltimoReajuste { get; set; }
  }
}
