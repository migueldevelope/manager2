using System.ComponentModel;

namespace IntegrationService.Enumns
{
  public enum EnumLayoutSystemCompleteV1 : byte
  {
    [Description("empresa")]
    Empresa = 0,
    [Description("nome_empresa")]
    NomeEmpresa = 1,
    [Description("estabelecimento")]
    Estabelecimento = 2,
    [Description("nome_estabelecimento")]
    NomeEstabelecimento = 3,
    [Description("cpf")]
    Cpf = 4,
    [Description("matricula")]
    Matricula = 5,
    [Description("nome")]
    Nome = 6,
    [Description("email")]
    Email = 7,
    [Description("data_nascimento")]
    DataNascimento = 8,
    [Description("celular")]
    Celular = 9,
    [Description("telefone")]
    Telefone = 10,
    [Description("identidade")]
    Identidade = 11,
    [Description("carteira_profissional")]
    CarteiraProfissional = 12,
    [Description("sexo")]
    Sexo = 13,
    [Description("data_admissao")]
    DataAdmissao = 14,
    [Description("situacao")]
    Situacao = 15,
    [Description("retorno_ferias")]
    DataRetornoFerias = 16,
    [Description("motivo_afastamento")]
    MotivoAfastamento = 17,
    [Description("data_demissao")]
    DataDemissao = 18,
    [Description("cargo")]
    Cargo = 19,
    [Description("nome_cargo")]
    DescricaoCargo = 20,
    [Description("data_ultima_troca_cargo")]
    DataUltimaTrocaCargo = 21,
    [Description("grau_instrucao")]
    GrauInstrucao = 22,
    [Description("nome_grau_instrucao")]
    DescricaoGrauInstrucao = 23,
    [Description("salario_nominal")]
    Salario = 24,
    [Description("data_ultimo_reajuste")]
    DataUltimoReajuste = 25,
    [Description("empresa_chefe")]
    EmpresaChefe = 26,
    [Description("nome_empresa_chefe")]
    NomeEmpresaChefe = 27,
    [Description("cpf_chefe")]
    CpfChefe = 28,
    [Description("matricula_chefe")]
    MatriculaChefe = 29
  }
}
