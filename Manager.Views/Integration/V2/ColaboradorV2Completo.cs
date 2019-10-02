using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Completo : ColaboradorV2Base, IColaboradorV2
  {
    public string Acao { get; set; }
    // Usuário
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Sexo { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string Celular { get; set; }
    public string GrauInstrucao { get; set; }
    public string NomeGrauInstrucao { get; set; }
    public string Apelido { get; set; }
    // Informações Contrato
    public DateTime DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public string Cargo { get; set; }
    public string NomeCargo { get; set; }
    public DateTime? DataTrocaCargo { get; set; }
    public string CentroCusto { get; set; }
    public string NomeCentroCusto { get; set; }
    public DateTime? DataTrocaCentroCusto { get; set; }
    public decimal SalarioNominal { get; set; }
    public int CargaHoraria { get; set; }
    public DateTime? DataUltimoReajuste { get; set; }
    public string MotivoUltimoReajuste { get; set; }
    public string Situacao { get; set; }
    public DateTime? DataRetornoFerias { get; set; }
    public string MotivoAfastamento { get; set; }
    // Informações do Gestor
    public string CpfGestor { get; set; }
    public string EmpresaGestor { get; set; }
    public string NomeEmpresaGestor { get; set; }
    public string EstabelecimentoGestor { get; set; }
    public string NomeEstabelecimentoGestor { get; set; }
    public string MatriculaGestor { get; set; }
    // Chaves de cálculo interno
    public string ChaveCargo => string.Format("{0};{1}", Empresa, Cargo);
    public string ChaveCentroCusto => string.Format("{0};{1}", Empresa, CentroCusto);
    public string ChaveEmpresaGestor => EmpresaGestor;
    public string ChaveEstabelecimentoGestor => string.Format("{0};{1}", EmpresaGestor, EstabelecimentoGestor);
    public string ChaveGestor => string.Format("{0};{1};{2};{3}", CpfGestor, EmpresaGestor, EstabelecimentoGestor, MatriculaGestor);
  }
}
