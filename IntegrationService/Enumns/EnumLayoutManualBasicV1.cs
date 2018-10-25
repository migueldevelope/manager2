using System.ComponentModel;

namespace IntegrationService.Enumns
{
  public enum EnumLayoutManualBasicV1 : byte
  {
    [Description("Nome da Empresa")]
    NomeEmpresa = 0,
    [Description("Cpf")]
    Cpf = 1,
    [Description("Matrícula/Matricula")]
    Matricula = 2,
    [Description("Nome")]
    Nome = 3,
    [Description("E-mail/Email")]
    Email = 4,
    [Description("Data de Nascimento")]
    DataNascimento = 5,
    [Description("Celular")]
    Celular = 6,
    [Description("Telefone")]
    Telefone = 7,
    [Description("Sexo")]
    Sexo = 8,
    [Description("Data de Admissao/Data de Admissão")]
    DataAdmissao = 9,
    [Description("Situação/Situacao")]
    Situacao = 10,
    [Description("Data de Demissao/Data de Demissão")]
    DataDemissao = 11,
    [Description("Descrição do Cargo/Descricao do Cargo")]
    DescricaoCargo = 12,
    [Description("Descrição do Grau de Instrução/Descricao do Grau de Instrução/Descrição do Grau de Instrucao/Descricao do Grau de Instrucao")]
    DescricaoGrauInstrucao = 13,
    [Description("Nome Empresa do Chefe")]
    NomeEmpresaChefe = 14,
    [Description("Cpf do Chefe")]
    CpfChefe = 15,
    [Description("Matrícula do Chefe/Matricula do Chefe")]
    MatriculaChefe = 16
  }
}
