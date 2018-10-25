using System.ComponentModel;

namespace IntegrationService.Enumns
{
  public enum EnumLayoutSystemBasicV1 : byte
  {
    [Description("empresa")]
    Empresa = 0,
    [Description("nome_empresa")]
    NomeEmpresa = 1,
    [Description("cpf")]
    Cpf = 2,
    [Description("matricula")]
    Matricula = 3,
    [Description("nome")]
    Nome = 4,
    [Description("email")]
    Email = 5,
    [Description("data_nascimento")]
    DataNascimento = 6,
    [Description("celular")]
    Celular = 7,
    [Description("telefone")]
    Telefone = 8,
    [Description("sexo")]
    Sexo = 9,
    [Description("data_admissao")]
    DataAdmissao = 10,
    [Description("situacao")]
    Situacao = 11,
    [Description("data_demissao")]
    DataDemissao = 12,
    [Description("cargo")]
    Cargo = 13,
    [Description("nome_cargo")]
    DescricaoCargo = 14,
    [Description("grau_instrucao")]
    GrauInstrucao = 15,
    [Description("nome_grau_instrucao")]
    DescricaoGrauInstrucao = 16,
    [Description("empresa_chefe")]
    EmpresaChefe = 17,
    [Description("nome_empresa_chefe")]
    NomeEmpresaChefe = 18,
    [Description("cpf_chefe")]
    CpfChefe = 19,
    [Description("matricula_chefe")]
    MatriculaChefe = 20
  }
}
