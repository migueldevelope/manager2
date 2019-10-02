namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Gestor : ColaboradorV2Base, IColaboradorV2
  {
    // Informações do Gestor
    public string CpfGestor { get; set; }
    public string EmpresaGestor { get; set; }
    public string NomeEmpresaGestor { get; set; }
    public string EstabelecimentoGestor { get; set; }
    public string NomeEstabelecimentoGestor { get; set; }
    public string MatriculaGestor { get; set; }
    // Chaves de cálculo interno
    public string ChaveEmpresaGestor => EmpresaGestor;
    public string ChaveEstabelecimentoGestor => string.Format("{0};{1}", EmpresaGestor, EstabelecimentoGestor);
    public string ChaveGestor => string.Format("{0};{1};{2};{3}", CpfGestor, EmpresaGestor, EstabelecimentoGestor, MatriculaGestor);
  }
}
