using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Situacao : ColaboradorV2Base, IColaboradorV2
  {
    // Informações Contrato
    public string Situacao { get; set; }
    public DateTime? DataRetornoFerias { get; set; }
    public string MotivoAfastamento { get; set; }
  }
}
