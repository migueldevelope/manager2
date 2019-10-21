using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Admissao : IColaboradorV2
  {
    public ColaboradorV2Base Colaborador { get; set; }
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
    public string Cargo { get; set; }
    public string NomeCargo { get; set; }
    public ColaboradorV2Base Gestor { get; set; }
    public string CentroCusto { get; set; }
    public string NomeCentroCusto { get; set; }
    public decimal SalarioNominal { get; set; }
    public int CargaHoraria { get; set; }
    // Chaves de cálculo interno
    public string ChaveCargo => string.Format("{0};{1}", Colaborador.Empresa, Cargo);
    public string ChaveCentroCusto => string.Format("{0};{1}", Colaborador.Empresa, CentroCusto);
  }
}
