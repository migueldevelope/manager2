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
    public string ChaveCargo() { return string.Format("{0};{1}", Colaborador.ChaveEmpresa(), Cargo.Trim().ToLower()); }
    public string ChaveCentroCusto() { return string.IsNullOrEmpty(CentroCusto) ? null : string.Format("{0};{1}", Colaborador.ChaveEmpresa(), CentroCusto.Trim().ToLower()); }

    #region Constructor
    public ColaboradorV2Completo()
    {

    }
    public ColaboradorV2Completo(List<string> list, List<string> title, string cultureDate)
    {
      try
      {
        Colaborador = new ColaboradorV2Base
        {
          Cpf = FieldStringCpf(list, title, "cpf", null),
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
        DataNascimento = FieldDate(list, title, "data_nascimento", null, cultureDate);
        Celular = FieldString(list, title, "celular", null);
        GrauInstrucao = FieldString(list, title, "grau_instrucao", null);
        NomeGrauInstrucao = FieldString(list, title, "nome_grau_instrucao", null);
        Apelido = FieldString(list, title, "apelido", null);
        Situacao = FieldString(list, title, "situacao", null);
        DataAdmissao = (DateTime)FieldDate(list, title, "data_admissao", DateTime.MinValue, cultureDate);
        DataDemissao = FieldDate(list, title, "data_demissao", null, cultureDate);
        Cargo = FieldString(list, title, "cargo", null);
        NomeCargo = FieldString(list, title, "nome_cargo", null);
        DataTrocaCargo = FieldDate(list, title, "data_ultima_troca_cargo", null, cultureDate);
        CentroCusto = FieldString(list, title, "centro_custo", null);
        NomeCentroCusto = FieldString(list, title, "nome_centro_custo", null);
        DataTrocaCentroCusto = FieldDate(list, title, "data_troca_centro_custo", null, cultureDate);
        SalarioNominal = FieldDecimal(list, title, "salario_nominal", 0);
        CargaHoraria = FieldDecimal(list, title, "carga_horaria", 0);
        DataUltimoReajuste = FieldDate(list, title, "data_ultimo_reajuste", null, cultureDate);
        MotivoUltimoReajuste = FieldString(list, title, "motivo_ultimo_reajuste", null);
        Gestor = new ColaboradorV2Base
        {
          Cpf = FieldStringCpf(list, title, "cpf_gestor", null),
          Empresa = FieldString(list, title, "empresa_gestor", null),
          NomeEmpresa = FieldString(list, title, "nome_empresa_gestor", null),
          Estabelecimento = FieldString(list, title, "estabelecimento_gestor", null),
          NomeEstabelecimento = FieldString(list, title, "nome_estabelecimento_gestor", null),
          Matricula = FieldString(list, title, "matricula_gestor", null)
        };
        if (Gestor.Cpf == null || Gestor.Matricula == null)
        {
          Gestor = new ColaboradorV2Base
          {
            Cpf = FieldStringCpf(list, title, "cpf_chefe", null),
            Empresa = FieldString(list, title, "empresa_chefe", null),
            NomeEmpresa = FieldString(list, title, "nome_empresa_chefe", null),
            Estabelecimento = FieldString(list, title, "estabelecimento_chefe", null),
            NomeEstabelecimento = FieldString(list, title, "nome_estabelecimento_chefe", null),
            Matricula = FieldString(list, title, "matricula_chefe", null)
          };
        }
        if (Gestor.Cpf == null || Gestor.Matricula == null)
        {
          Gestor = null;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public ColaboradorV2Completo(ViewIntegrationMetadadosV1 view, string cultureDate, string idAccount)
    {
      try
      {
        Colaborador = new ColaboradorV2Base
        {
          Cpf = FieldStringCpf(view.Cpf, null),
          Empresa = view.Empresa.Trim(),
          NomeEmpresa = view.NomeEmpresa.Trim(),
          Estabelecimento = view.Estabelecimento.Trim(),
          NomeEstabelecimento = view.NomeEstabelecimento.Trim(),
          Matricula = view.Matricula?.Trim()
        };
        Acao = "CARGA";
        Nome = FieldString(view.Nome, null);
        Email = FieldString(view.Email, null);
        // Ajuste de e-mail da UNIMEDNERS
        if (Email != null && Email.Contains("unimed-ners.net"))
        {
          Email = null;
        }
        Sexo = FieldString(view.Sexo, null);
        DataNascimento = FieldDate(view.DataNascimento, null, cultureDate);
        Celular = FieldString(view.Celular, null);
        GrauInstrucao = FieldString(view.GrauInstrucao, "7");
        NomeGrauInstrucao = FieldString(view.NomeGrauInstrucao, "Ensino médio completo");
        Apelido = FieldString(view.UsuarioAd, null);
        Situacao = FieldString(view.Situacao, "ATIVO");
        DataAdmissao = (DateTime)FieldDate(view.DataAdmissao, DateTime.MinValue, cultureDate);
        DataDemissao = FieldDate(view.DataDemissao, null, cultureDate);
        Cargo = FieldString(view.Cargo, null);
        NomeCargo = FieldString(view.NomeCargo, null);
        DataTrocaCargo = FieldDate(view.DataTrocaCargo, null, cultureDate);
        CentroCusto = FieldString(view.ClassContabil, null);
        NomeCentroCusto = FieldString(view.ClassContabil, null);
        DataTrocaCentroCusto = FieldDate(view.DataTrocaClassContabil, null, cultureDate);
        SalarioNominal = view.SalarioNominal == null ? 0 : (decimal)view.SalarioNominal;
        CargaHoraria = view.CargaHoraria == null ? 0 : (decimal)view.CargaHoraria;
        DataUltimoReajuste = FieldDate(view.DataUltimoReajuste, null, cultureDate);
        MotivoUltimoReajuste = FieldString(view.MotivoUltimoReajuste, null);
        Gestor = new ColaboradorV2Base
        {
          Cpf = FieldStringCpf(view.GestorCpf, null),
          Empresa = view.GestorEmpresa?.Trim(),
          NomeEmpresa = view.GestorNomeEmpresa?.Trim(),
          Estabelecimento = view.GestorEstabelecimento?.Trim(),
          NomeEstabelecimento = view.GestorNomeEstabelecimento?.Trim(),
          Matricula = view.GestorMatricula?.Trim()
        };
        if (Gestor.Cpf == null || Gestor.Matricula == null)
        {
          Gestor = null;
        }
        if (idAccount.Equals("5b91299a17858f95ffdb79f6")) // Unimed Nordeste Rs
        {
          Gestor = null;
        }
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
        return index == -1 ? defaultValue : list[index].Trim();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private string FieldString(string field, string defaultValue)
    {
      try
      {
        return string.IsNullOrEmpty(field) ? defaultValue : field.Trim();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private string FieldStringCpf(List<string> list, List<string> title, string field, string defaultValue)
    {
      try
      {
        int index = title.FindIndex(p => p.Trim().ToLower().Equals(field));
        return index == -1
          ? defaultValue
          : FieldStringCpf(list[index], defaultValue);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private string FieldStringCpf(string cpf, string defaultValue)
    {
      try
      {
        return string.IsNullOrEmpty(cpf) ? defaultValue : cpf.Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0');
      }
      catch (Exception)
      {
        throw;
      }
    }
    private DateTime? FieldDate(List<string> list, List<string> title, string field, DateTime? defaultValue, string culture)
    {
      try
      {
        int index = title.FindIndex(p => p.Trim().ToLower().Equals(field));
        if (index == -1)
          return null;
        return FieldDate(list[index], defaultValue, culture);
      }
      catch (Exception)
      {
        return null;
      }
    }
    private DateTime? FieldDate(string field, DateTime? defaultValue, string culture)
    {
      try
      {
        if (field == null)
        {
          return null;
        }
        DateTime.TryParse(field.Substring(0, 10), CultureInfo.CreateSpecificCulture(culture), DateTimeStyles.AdjustToUniversal, out DateTime resultParse);
        DateTime? result = defaultValue;
        if (!string.IsNullOrEmpty(field))
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
        return FieldDecimal(list[index], defaultValue);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private decimal FieldDecimal(string field, decimal defaultValue)
    {
      try
      {
        return string.IsNullOrEmpty(field) ? defaultValue : Convert.ToDecimal(field.Trim());
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

  }
}
