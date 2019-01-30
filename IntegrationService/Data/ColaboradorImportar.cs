using IntegrationService.Enumns;
using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IntegrationService.Data
{
  public class ColaboradorImportar : ViewColaborador
  {
    public string Message { get; set; }

    #region Construtores
    public ColaboradorImportar() {}
    public ColaboradorImportar(List<string> list, EnumLayoutManualBasicV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        Empresa = FormatedFieldKey(list[(int)EnumLayoutManualBasicV1.NomeEmpresa]);
        NomeEmpresa = FormatedField(list[(int)EnumLayoutManualBasicV1.NomeEmpresa]);
        Estabelecimento = "1";
        NomeEstabelecimento = "Estabelecimento Padrão";
        Documento = list[(int)EnumLayoutManualBasicV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutManualBasicV1.Matricula].Trim());
        Nome = FormatedField(list[(int)EnumLayoutManualBasicV1.Nome]);
        Email = FormatedFieldKey(list[(int)EnumLayoutManualBasicV1.Email]);
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutManualBasicV1.Celular].Trim();
        Telefone = list[(int)EnumLayoutManualBasicV1.Telefone].Trim();
        Identidade = string.Empty;
        CarteiraProfissional = string.Empty;
        Sexo = list[(int)EnumLayoutManualBasicV1.Sexo].Trim();
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutManualBasicV1.Situacao].Trim();
        DataRetornoFerias = null;
        MotivoAfastamento = string.Empty;
        DataDemissao = dataDemissao;
        Cargo = FormatedFieldKey(list[(int)EnumLayoutManualBasicV1.DescricaoCargo]);
        NomeCargo = FormatedField(list[(int)EnumLayoutManualBasicV1.DescricaoCargo]);
        DataUltimaTrocaCargo = null;
        GrauInstrucao = FormatedFieldKey(list[(int)EnumLayoutManualBasicV1.DescricaoGrauInstrucao]);
        NomeGrauInstrucao = FormatedField(list[(int)EnumLayoutManualBasicV1.DescricaoGrauInstrucao]);
        SalarioNominal = 0;
        DataUltimoReajuste = null;
        EmpresaGestor = FormatedFieldKey(list[(int)EnumLayoutManualBasicV1.NomeEmpresaGestor]);
        NomeEmpresaGestor = FormatedField(list[(int)EnumLayoutManualBasicV1.NomeEmpresaGestor]);
        DocumentoGestor = list[(int)EnumLayoutManualBasicV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualBasicV1.MatriculaGestor]))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutManualBasicV1.MatriculaGestor].Trim());
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorImportar(List<string> list, EnumLayoutManualCompleteV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataRetornoFerias], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataRetornoFerias);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataUltimaTrocaCargo], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimaTrocaCargo);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataUltimoReajuste], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimoReajuste);
        Empresa = list[(int)EnumLayoutManualCompleteV1.NomeEmpresa].Trim().ToLower();
        NomeEmpresa = list[(int)EnumLayoutManualCompleteV1.NomeEmpresa].Trim();
        Estabelecimento = list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento].Trim().ToLower();
        NomeEstabelecimento = list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento].Trim();
        Documento = list[(int)EnumLayoutManualCompleteV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutManualCompleteV1.Matricula].Trim());
        Nome = list[(int)EnumLayoutManualCompleteV1.Nome].Trim();
        Email = list[(int)EnumLayoutManualCompleteV1.Email].Trim().ToLower();
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutManualCompleteV1.Celular].Trim();
        Telefone = list[(int)EnumLayoutManualCompleteV1.Telefone].Trim();
        Identidade = list[(int)EnumLayoutManualCompleteV1.Identidade].Trim();
        CarteiraProfissional = list[(int)EnumLayoutManualCompleteV1.CarteiraProfissional].Trim();
        Sexo = list[(int)EnumLayoutManualCompleteV1.Sexo].Trim();
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutManualCompleteV1.Situacao].Trim();
        DataRetornoFerias = dataRetornoFerias;
        MotivoAfastamento = list[(int)EnumLayoutManualCompleteV1.MotivoAfastamento].Trim();
        DataDemissao = dataDemissao;
        Cargo = list[(int)EnumLayoutManualCompleteV1.DescricaoCargo].Trim().ToLower();
        NomeCargo = list[(int)EnumLayoutManualCompleteV1.DescricaoCargo].Trim();
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao].Trim().ToLower();
        NomeGrauInstrucao = list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao].Trim();
        SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutManualCompleteV1.Salario].Trim());
        DataUltimoReajuste = dataUltimoReajuste;
        DocumentoGestor = list[(int)EnumLayoutManualCompleteV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        EmpresaGestor = list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor].Trim().ToLower();
        NomeEmpresaGestor = list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor].Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor]))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor].Trim());
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorImportar(List<string> list, EnumLayoutSystemBasicV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        Empresa = list[(int)EnumLayoutSystemBasicV1.Empresa].Trim().ToLower();
        NomeEmpresa = list[(int)EnumLayoutSystemBasicV1.NomeEmpresa].Trim();
        Estabelecimento = "1";
        NomeEstabelecimento = "Estabelecimento Padrão";
        Documento = list[(int)EnumLayoutSystemBasicV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutSystemBasicV1.Matricula].Trim());
        Nome = list[(int)EnumLayoutSystemBasicV1.Nome].Trim();
        Email = list[(int)EnumLayoutSystemBasicV1.Email].Trim().ToLower();
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutSystemBasicV1.Celular].Trim();
        Telefone = list[(int)EnumLayoutSystemBasicV1.Telefone].Trim();
        Identidade = string.Empty;
        CarteiraProfissional = string.Empty;
        Sexo = list[(int)EnumLayoutSystemBasicV1.Sexo].Trim();
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutSystemBasicV1.Situacao].Trim();
        DataRetornoFerias = null;
        MotivoAfastamento = string.Empty;
        DataDemissao = dataDemissao;
        Cargo = list[(int)EnumLayoutSystemBasicV1.Cargo].Trim().ToLower();
        NomeCargo = list[(int)EnumLayoutSystemBasicV1.DescricaoCargo].Trim();
        DataUltimaTrocaCargo = null;
        GrauInstrucao = list[(int)EnumLayoutSystemBasicV1.GrauInstrucao].Trim().ToLower();
        NomeGrauInstrucao = list[(int)EnumLayoutSystemBasicV1.DescricaoGrauInstrucao].Trim();
        SalarioNominal = 0;
        DataUltimoReajuste = null;
        DocumentoGestor = list[(int)EnumLayoutSystemBasicV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        EmpresaGestor = list[(int)EnumLayoutSystemBasicV1.EmpresaGestor].Trim().ToLower();
        NomeEmpresaGestor = list[(int)EnumLayoutSystemBasicV1.NomeEmpresaGestor].Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor]))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor].Trim());
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorImportar(List<string> list, EnumLayoutSystemCompleteV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataRetornoFerias], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataRetornoFerias);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataUltimaTrocaCargo], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimaTrocaCargo);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataUltimoReajuste], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimoReajuste);
        Empresa = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.Empresa]);
        NomeEmpresa = FormatedField(list[(int)EnumLayoutSystemCompleteV1.NomeEmpresa]);
        Estabelecimento = list[(int)EnumLayoutSystemCompleteV1.Estabelecimento].Trim().ToLower();
        NomeEstabelecimento = list[(int)EnumLayoutSystemCompleteV1.NomeEstabelecimento].Trim();
        Documento = list[(int)EnumLayoutSystemCompleteV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutSystemCompleteV1.Matricula].Trim());
        Nome = list[(int)EnumLayoutSystemCompleteV1.Nome].Trim();
        Email = list[(int)EnumLayoutSystemCompleteV1.Email].Trim().ToLower();
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutSystemCompleteV1.Celular].Trim();
        Telefone = list[(int)EnumLayoutSystemCompleteV1.Telefone].Trim();
        Identidade = list[(int)EnumLayoutSystemCompleteV1.Identidade].Trim();
        CarteiraProfissional = list[(int)EnumLayoutSystemCompleteV1.CarteiraProfissional].Trim();
        Sexo = list[(int)EnumLayoutSystemCompleteV1.Sexo].Trim();
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutSystemCompleteV1.Situacao].Trim();
        DataRetornoFerias = dataRetornoFerias;
        MotivoAfastamento = list[(int)EnumLayoutSystemCompleteV1.MotivoAfastamento].Trim();
        DataDemissao = dataDemissao;
        Cargo = list[(int)EnumLayoutSystemCompleteV1.Cargo].Trim().ToLower();
        NomeCargo = list[(int)EnumLayoutSystemCompleteV1.DescricaoCargo].Trim();
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = list[(int)EnumLayoutSystemCompleteV1.GrauInstrucao].Trim();
        NomeGrauInstrucao = list[(int)EnumLayoutSystemCompleteV1.DescricaoGrauInstrucao].Trim();
        SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutSystemCompleteV1.Salario].Trim());
        DataUltimoReajuste = dataUltimoReajuste;
        DocumentoGestor = list[(int)EnumLayoutSystemCompleteV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        EmpresaGestor = list[(int)EnumLayoutSystemCompleteV1.EmpresaGestor].Trim().ToLower();
        NomeEmpresaGestor = list[(int)EnumLayoutSystemCompleteV1.NomeEmpresaGestor].Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor]))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor].Trim());
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Formatação de String
    private string FormatedField(string param)
    {
      try
      {
        string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(param.Trim().ToLower());
        result = result.Replace(" Da "," da ").Replace(" De ", " de ").Replace(" Do ", " do ").Replace(" Dos ", " dos ").Replace(" Iii", " III").Replace(" Ii", " II").Replace(" Em "," em ");
        return result;
      }
      catch (Exception)
      {

        throw;
      }
    }
    private string FormatedFieldKey(string param)
    {
      try
      {
        return(param.Trim().ToLower());
      }
      catch (Exception)
      {

        throw;
      }
    }
    #endregion

    #region Validação de dados do Colaborador
    private void ValidDataColaborator()
    {
      try
      {
        Message = string.Empty;
        // Nome da Empresa
        if (string.IsNullOrEmpty(NomeEmpresa))
          Message = string.Concat(Message, "Nome da empresa vazia!;");
        // Valid Cpf
        if (string.IsNullOrEmpty(Documento))
          Message = string.Concat(Message, "CPF vazio!;");
        else
          if (!IsValidCPF(Documento))
          Message = string.Concat(Message, "CPF inválido!;");
        // Matricula obrigatória
        if (Matricula == 0)
          Message = string.Concat(Message, "Matricula não informada!;");
        // Nome
        if (string.IsNullOrEmpty(Nome))
          Message = string.Concat(Message, "Nome não informado!;");
        // E-mail
        if (string.IsNullOrEmpty(Email))
          Message = string.Concat(Message, "E-mail não informado!;");
        // Data admissão
        if (DataAdmissao == null)
          Message = string.Concat(Message, "Data de admissão inválida!;");
        // Situação
        if (!Situacao.Equals("Ativo", StringComparison.InvariantCultureIgnoreCase) &&
            !Situacao.Equals("Férias", StringComparison.InvariantCultureIgnoreCase) &&
            !Situacao.Equals("Ferias", StringComparison.InvariantCultureIgnoreCase) &&
            !Situacao.Equals("Afastado", StringComparison.InvariantCultureIgnoreCase) &&
            !Situacao.Equals("Demitido", StringComparison.InvariantCultureIgnoreCase))
          Message = string.Concat(Message, "Situação diferente de Ativo/Férias/Afastado/Demitido!;");
        if (Situacao.Equals("Demitido") && DataDemissao == null)
          Message = string.Concat(Message, "Data de demissão deve ser informada!;");
        // Nome do Cargo
        if (string.IsNullOrEmpty(NomeCargo))
          Message = string.Concat(Message, "Nome do cargo vazio!;");
        // Grau de Instrução, Nome do Grau de Instrução
        if (string.IsNullOrEmpty(NomeGrauInstrucao))
          Message = string.Concat(Message, "Nome do grau de instrução vazio!;");
        // Valid Cpf chefe
        if (!string.IsNullOrEmpty(DocumentoGestor))
          if (!IsValidCPF(DocumentoGestor))
            Message = string.Concat(Message, "Documento do gestor é inválido!;");
      }
      catch (Exception)
      {
        throw;
      }
    }
    private bool VerficarSeTodosOsDigitosSaoIdenticos(string cpf)
    {
      var previous = -1;
      for (var i = 0; i < cpf.Length; i++)
      {
        if (char.IsDigit(cpf[i]))
        {
          var digito = cpf[i] - '0';
          if (previous == -1)
            previous = digito;
          else
            if (previous != digito)
            return false;
        }
      }
      return true;
    }
    private bool IsValidCPF(string cpf)
    {
      if (cpf.Length != 11)
        return false;
      if (VerficarSeTodosOsDigitosSaoIdenticos(cpf))
        return false;

      int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      string tempCpf;
      string digito;
      int soma;
      int resto;
      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");

      tempCpf = cpf.Substring(0, 9);
      soma = 0;
      for (int i = 0; i < 9; i++)
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
      resto = soma % 11;
      if (resto < 2)
        resto = 0;
      else
        resto = 11 - resto;
      digito = resto.ToString();
      tempCpf = tempCpf + digito;
      soma = 0;
      for (int i = 0; i < 10; i++)
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
      resto = soma % 11;
      if (resto < 2)
        resto = 0;
      else
        resto = 11 - resto;
      digito = digito + resto.ToString();
      return cpf.EndsWith(digito);
    }
    #endregion

  }
}
