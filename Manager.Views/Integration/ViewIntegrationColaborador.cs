using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.Integration
{
  public class ViewIntegrationColaborador
  {
    public ViewColaborador Colaborador { get; set; }
    public List<string> CamposAlterados { get; set; }
    public EnumColaboradorAcao Acao { get; set; }
    public EnumColaboradorSituacao Situacao { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }
    public string Message { get; set;}
  }
}
