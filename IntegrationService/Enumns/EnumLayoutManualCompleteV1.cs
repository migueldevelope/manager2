using System.ComponentModel;

namespace IntegrationService.Enumns
{
  public enum EnumLayoutManualCompleteV1 : byte
  {
    [Description("Nome da Empresa")]
    NomeEmpresa = 0,
    [Description("Nome do Estabelecimento")]
    NomeEstabelecimento = 1,
    [Description("Cpf")]
    Cpf = 2,
    [Description("Matrícula/Matricula")]
    Matricula = 3,
    [Description("Nome")]
    Nome = 4,
    [Description("E-mail/Email")]
    Email = 5,
    [Description("Data de Nascimento")]
    DataNascimento = 6,
    [Description("Celular")]
    Celular = 7,
    [Description("Telefone")]
    Telefone = 8,
    [Description("Identidade")]
    Identidade = 9,
    [Description("Carteira Profissional")]
    CarteiraProfissional = 10,
    [Description("Sexo")]
    Sexo = 11,
    [Description("Data de Admissão/Data de Admissao")]
    DataAdmissao = 12,
    [Description("Situação/Situacao")]
    Situacao = 13,
    [Description("Retorno das Férias/Retorno das Ferias")]
    DataRetornoFerias = 14,
    [Description("Motivo Afastamento")]
    MotivoAfastamento = 15,
    [Description("Data de Demissão/Data de Demissao")]
    DataDemissao = 16,
    [Description("Descrição do Cargo/Descricao do Cargo")]
    DescricaoCargo = 17,
    [Description("Data Última Troca de Cargo/Data Ultima Troca de Cargo")]
    DataUltimaTrocaCargo = 18,
    [Description("Descrição do Grau de Instrução/Descrição do Grau de Instrucao/Descricao do Grau de Instrução/Descricao do Grau de Instrucao")]
    DescricaoGrauInstrucao = 19,
    [Description("Salário Nominal/Salario Nominal")]
    Salario = 20,
    [Description("Data Último Reajuste/Data Ultimo Reajuste")]
    DataUltimoReajuste = 21,
    [Description("Nome Empresa do Chefe/Nome Empresa do Gestor")]
    NomeEmpresaGestor = 22,
    [Description("Cpf do Chefe/Cpf do Gestor")]
    CpfGestor = 23,
    [Description("Matrícula do Chefe/Matricula do Chefe/Matrícula do Gestor/Matricula do Gestor")]
    MatriculaGestor = 24
  }
}
