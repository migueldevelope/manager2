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
    public string ChaveEmpresa { get { return Empresa; } }
    public string ChaveEstabelecimento => string.Format("{0};{1}", Empresa, Estabelecimento);
    public string Chave => string.Format("{0};{1};{2};{3}", Cpf, Empresa, Estabelecimento, Matricula);
  }
}
