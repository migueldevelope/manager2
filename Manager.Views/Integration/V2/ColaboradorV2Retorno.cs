using Manager.Views.Enumns;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Retorno
  {
    public string IdUser { get; set; }
    public string IdContract { get; set; }
    public EnumSituacaoRetornoIntegracao Situacao { get; set; }
    public string Mensagem { get; set; }

  }
}
