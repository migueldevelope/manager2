using Manager.Views.Enumns;
using System;

namespace Manager.Views.Integration
{
  public class Colaborador
  {
    public string Empresa { get; set; }
    public string NomeEmpresa { get; set; }
    public string Estabelecimento { get; set; }
    public string NomeEstabelecimento { get; set; }
    public string Documento { get; set; }
    public long Matricula { get; set; }
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
    public Decimal SalarioNominal { get; set; }
    public DateTime? DataUltimoReajuste { get; set; }
    public string DocumentoGestor { get; set; }
    public string EmpresaGestor { get; set; }
    public string NomeEmpresaGestor { get; set; }
    public long MatriculaGestor { get; set; }
    public int TypeUser { get; set; }
    public string ChaveEmpresa { get { return (string.Format("{0};{1}", Empresa.Trim(), NomeEmpresa.Trim()).ToLower()); } }
    public string ChaveEstabelecimento { get { return (string.Format("{0};{1};{2};{3}", Empresa.Trim(), NomeEmpresa.Trim(), Estabelecimento.Trim(), NomeEstabelecimento.Trim()).ToLower()); } }
    public string ChaveGrauInstrucao { get { return (string.Format("{0};{1}", GrauInstrucao.Trim(), NomeGrauInstrucao.Trim()).ToLower()); } }
    public string ChaveCargo { get { return (string.Format("{0};{1};{2};{3}", Empresa.Trim(), NomeEmpresa.Trim(), Cargo.Trim(), NomeCargo.Trim()).ToLower()); } }
    public string ChaveColaborador { get { return (string.Format("{0};{1};{2};{3}", Documento.Trim(), Empresa.Trim(), NomeEmpresa.Trim(), Matricula).ToLower()); } }
    public string ChaveEmpresaGestor { get { return (string.Format("{0};{1}", EmpresaGestor.Trim(), NomeEmpresaGestor.Trim()).ToLower()); } }
    public string ChaveGestor { get { return (string.Format("{0};{1};{2};{3}", DocumentoGestor.Trim(), EmpresaGestor.Trim(), NomeEmpresaGestor.Trim(), MatriculaGestor).ToLower()); } }
  }
}
