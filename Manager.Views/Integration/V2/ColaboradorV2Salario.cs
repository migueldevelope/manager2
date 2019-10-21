using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Salario : IColaboradorV2
  {
    public ColaboradorV2Base Colaborador { get; set; }
    public decimal SalarioNominal { get; set; }
    public int CargaHoraria { get; set; }
    public DateTime? DataUltimoReajuste { get; set; }
    public string MotivoUltimoReajuste { get; set; }
  }
}
