using System;
using System.Collections.Generic;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2 : ColaboradorV2Completo
  {
    public string _idPayrollEmployee { get; set; }
    public string _idPrevious { get; set; }
    public DateTime Registro { get; set; }
    public string SituacaoIntegracao { get; set; }
    public string _idUser { get; set; }
    public string _idContract { get; set; }
    public List<string> Mensagens { get; set; }
  }
}
