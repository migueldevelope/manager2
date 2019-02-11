using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.Integration
{
  public class ViewIntegrationColaborador
  {
    public ViewColaborador Colaborador { get; set; }
    public List<string> CamposAlterados { get; set; }
    public EnumColaboradorSituacao Situacao { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }
    public string Message { get; set;}
  }
}
