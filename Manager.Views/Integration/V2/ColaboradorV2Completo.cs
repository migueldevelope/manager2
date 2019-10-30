using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Completo : IColaboradorV2
  {
    public ColaboradorV2Base Colaborador { get; set; }
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
    public string Situacao { get; set; }
    public DateTime DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public string Cargo { get; set; }
    public string NomeCargo { get; set; }
    public DateTime? DataTrocaCargo { get; set; }
    public ColaboradorV2Base Gestor { get; set; }
    public string CentroCusto { get; set; }
    public string NomeCentroCusto { get; set; }
    public DateTime? DataTrocaCentroCusto { get; set; }
    public decimal SalarioNominal { get; set; }
    public decimal CargaHoraria { get; set; }
    public DateTime? DataUltimoReajuste { get; set; }
    public string MotivoUltimoReajuste { get; set; }
    // Chaves de cálculo interno
    public string ChaveCargo => string.Format("{0};{1}", Colaborador.Empresa, Cargo);
    public string ChaveCentroCusto => string.Format("{0};{1}", Colaborador.Empresa, CentroCusto);
  }
}
