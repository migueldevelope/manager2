namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Base
  {
    // Usuário
    public string Cpf { get; set; }
    // Contrato
    public string Empresa { get; set; }
    public string NomeEmpresa { get; set; }
    public string Estabelecimento { get; set; }
    public string NomeEstabelecimento { get; set; }
    public string Matricula { get; set; }
    // Chaves de cálculo interno
    public string ChaveEmpresa { get { return Empresa.Trim().ToLower(); } }
    public string ChaveEstabelecimento => string.Format("{0};{1}", Empresa.Trim().ToLower(), Estabelecimento.Trim().ToLower());
    public string Chave1 => string.Format("{0};{1};{2};{3}", Cpf.Trim().ToLower(), Empresa.Trim().ToLower(), Estabelecimento.Trim().ToLower(), Matricula.Trim().ToLower());
    public string Chave2 => string.Format("{0};{1};{2}", Cpf.Trim().ToLower(), Empresa.Trim().ToLower(), Estabelecimento.Trim().ToLower(), Matricula.Trim().ToLower());
  }
}
