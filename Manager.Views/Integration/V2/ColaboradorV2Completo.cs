using System;
using System.Collections.Generic;
using System.Globalization;

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

    #region Constructor
    public ColaboradorV2Completo()
    {

    }
    public ColaboradorV2Completo(List<string> list, List<string> title)
    {
      try
      {
        Colaborador = new ColaboradorV2Base
        {
          Cpf = FieldString(list, title, "cpf", null),
          Empresa = FieldString(list, title, "empresa", null),
          NomeEmpresa = FieldString(list, title, "nome_empresa", null),
          Estabelecimento = FieldString(list, title, "estabelecimento", null),
          NomeEstabelecimento = FieldString(list, title, "nome_estabelecimento", null),
          Matricula = FieldString(list, title, "matricula", null),
        };
        Acao = "CARGA";
        Nome = FieldString(list, title, "nome", null);
        Email = FieldString(list, title, "email", null);
        Sexo = FieldString(list, title, "sexo", null);
        DataNascimento = FieldDate(list, title, "data_nascimento", null);
        Celular = FieldString(list, title, "celular", null);
        GrauInstrucao = FieldString(list, title, "grau_instrucao", null);
        NomeGrauInstrucao = FieldString(list, title, "nome_grau_instrucao", null);
        Apelido = FieldString(list, title, "apelido", null);
        Situacao = FieldString(list, title, "situacao", null);
        DataAdmissao = (DateTime)FieldDate(list, title, "data_admissao", DateTime.MinValue);
        DataDemissao = FieldDate(list, title, "data_demissao", null);
        Cargo = FieldString(list, title, "cargo", null);
        NomeCargo = FieldString(list, title, "nome_cargo", null);
        DataTrocaCargo = FieldDate(list, title, "data_ultima_troca_cargo", null);
        CentroCusto = FieldString(list, title, "centro_custo", null);
        NomeCentroCusto = FieldString(list, title, "nome_centro_custo", null);
        DataTrocaCentroCusto = FieldDate(list, title, "data_troca_centro_custo", null);
        SalarioNominal = FieldDecimal(list, title, "salario_nominal", 0);
        CargaHoraria = FieldDecimal(list, title, "carga_horaria", 0);
        DataUltimoReajuste = FieldDate(list, title, "data_ultimo_reajuste", null);
        MotivoUltimoReajuste = FieldString(list, title, "motivo_ultimo_reajuste", null);
        Gestor = new ColaboradorV2Base
        {
          Cpf = FieldString(list, title, "cpf_gestor", null),
          Empresa = FieldString(list, title, "empresa_gestor", null),
          NomeEmpresa = FieldString(list, title, "nome_empresa_gestor", null),
          Estabelecimento = FieldString(list, title, "estabelecimento_gestor", null),
          NomeEstabelecimento = FieldString(list, title, "nome_estabelecimento_gestor", null),
          Matricula = FieldString(list, title, "matricula_gestor", null)
        };
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

    #region Field Formmating
    private string FieldString(List<string> list, List<string> title, string field, string defaultValue)
    {
      try
      {
        int index = title.FindIndex(p => p.Trim().ToLower().Equals(field));
        return index == -1 ? defaultValue : list[index];
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private DateTime? FieldDate(List<string> list, List<string> title, string field, DateTime? defaultValue)
    {
      try
      {
        int index = title.FindIndex(p => p.Trim().ToLower().Equals(field));
        if (index == -1)
          return null;
        DateTime.TryParse(list[index].Substring(0, 10), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.AdjustToUniversal, out DateTime resultParse);
        DateTime? result = defaultValue;
        if (!string.IsNullOrEmpty(list[index]))
          result = resultParse.ToUniversalTime();
        return result;
      }
      catch (Exception)
      {
        return null;
      }
    }
    private decimal FieldDecimal(List<string> list, List<string> title, string field, decimal defaultValue)
    {
      try
      {
        int index = title.FindIndex(p => p.Trim().ToLower().Equals(field));
        if (index == -1)
          return defaultValue;
        return string.IsNullOrEmpty(list[index]) ? defaultValue : Convert.ToDecimal(list[index].Trim());
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

  }
}
