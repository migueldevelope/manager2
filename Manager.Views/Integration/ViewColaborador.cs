using System;

namespace Manager.Views.Integration
{
  public class ViewColaborador
  {
    public string Empresa { get; set; }
    public string NomeEmpresa { get; set; }
    public string Estabelecimento { get; set; }
    public string NomeEstabelecimento { get; set; }
    public string Documento { get; set; }
    public string Matricula { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string Celular { get; set; }
    public string Telefone { get; set; }
    public string Identidade { get; set; }
    public string CarteiraProfissional { get; set; }
    public string Sexo { get; set; }
    public DateTime? DataAdmissao { get; set; }
    public string Situacao { get; set; }
    public DateTime? DataRetornoFerias { get; set; }
    public string MotivoAfastamento { get; set; }
    public DateTime? DataDemissao { get; set; }
    public string Cargo { get; set; }
    public string NomeCargo { get; set; }
    public DateTime? DataUltimaTrocaCargo { get; set; }
    public string GrauInstrucao { get; set; }
    public string NomeGrauInstrucao { get; set; }
    public decimal SalarioNominal { get; set; }
    public DateTime? DataUltimoReajuste { get; set; }
    public string DocumentoGestor { get; set; }
    public string EmpresaGestor { get; set; }
    public string NomeEmpresaGestor { get; set; }
    public string EstabelecimentoGestor { get; set; }
    public string NomeEstabelecimentoGestor { get; set; }
    public string MatriculaGestor { get; set; }
    public string ChaveEmpresa { get { return Empresa; } }
    public string ChaveEstabelecimento { get { return string.Format("{0};{1}", Empresa, Estabelecimento); } }
    public string ChaveGrauInstrucao { get { return GrauInstrucao; } }
    public string ChaveCargo { get { return (string.Format("{0};{1}", Empresa, Cargo)); } }
    public string ChaveColaborador { get { return string.Format("{0};{1};{2};{3}", Documento, Empresa, Estabelecimento, Matricula); } }
    public string ChaveEmpresaGestor { get { return EmpresaGestor; } }
    public string ChaveEstabelecimentoGestor { get { return string.Format("{0};{1}", EmpresaGestor, EstabelecimentoGestor); } }
    public string ChaveGestor { get { return string.Format("{0};{1};{2};{3}", DocumentoGestor, EmpresaGestor, EstabelecimentoGestor, MatriculaGestor); } }
  }
}
