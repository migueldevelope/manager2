using System.Collections.Generic;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Ativo
  {
    public string _id { get; set; }
    // Usuário
    public string Cpf { get; set; }
    // Contrato
    public List<string> Chaves { get; set; }
    public string Matricula { get; set; }
  }
}
