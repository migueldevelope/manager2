using System;

namespace IntegrationService.Data
{
  public class Colaborador
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
    public Double SalarioNominal { get; set; }
    public DateTime? DataUltimoReajuste { get; set; }
    public string DocumentoChefe { get; set; }
    public string EmpresaChefe { get; set; }
    public string MatriculaChefe { get; set; }
    public string NomeChefe { get; set; }
  }
}
