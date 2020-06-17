using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Retorno
  {
    public string IdPayrollEmployee { get; set; }
    public string IdUser { get; set; }
    public string IdContract { get; set; }
    public string Situacao { get; set; }
    public string IdGestor { get; set; }
    public EnumTypeUser TypeUserGestor { get; set; }
    public List<string> Mensagem { get; set; }
  }
}
