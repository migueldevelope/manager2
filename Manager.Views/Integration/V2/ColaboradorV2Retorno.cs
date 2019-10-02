using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Retorno
  {
    public string IdPayrollEmployee { get; set; }
    public string IdUser { get; set; }
    public string IdContract { get; set; }
    public EnumSituacaoRetornoIntegracao Situacao { get; set; }
    public List<string> Mensagem { get; set; }

  }
}
