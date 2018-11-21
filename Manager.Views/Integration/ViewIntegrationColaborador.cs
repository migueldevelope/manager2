using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.Integration
{
  public class ViewIntegrationColaborador
  {
    public Colaborador Colaborador { get; set; }
    public List<string> CamposAlterados { get; set; }
    public int Acao { get; set; }
    public int Situacao { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }
    public string Message { get; set;}
  }
}
