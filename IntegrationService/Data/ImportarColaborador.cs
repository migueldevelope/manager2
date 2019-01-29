using IntegrationService.Enumns;
using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService.Data
{
  public class ImportarColaborador
  {
    public ViewColaborador Colaborador { get; set; }
    public string Message { get; set; }
    private ViewColaborador ColaboradorAnterior { get; set; }
    public ImportarColaborador() {}

    #region Construtores
    public ImportarColaborador(List<string> list, EnumLayoutManualBasicV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        Colaborador = new ViewColaborador
        {
          Empresa = list[(int)EnumLayoutManualBasicV1.NomeEmpresa].Trim().ToLower(),
          NomeEmpresa = list[(int)EnumLayoutManualBasicV1.NomeEmpresa].Trim(),
          Estabelecimento = "1",
          NomeEstabelecimento = "Estabelecimento Padrão",
          Documento = list[(int)EnumLayoutManualBasicV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0'),
          Matricula = Convert.ToInt64(list[(int)EnumLayoutManualBasicV1.Matricula].Trim()),
          Nome = list[(int)EnumLayoutManualBasicV1.Nome].Trim(),
          Email = list[(int)EnumLayoutManualBasicV1.Email].Trim().ToLower(),
          DataNascimento = dataNascimento,
          Celular = list[(int)EnumLayoutManualBasicV1.Celular].Trim(),
          Telefone = list[(int)EnumLayoutManualBasicV1.Telefone].Trim(),
          Identidade = string.Empty,
          CarteiraProfissional = string.Empty,
          Sexo = list[(int)EnumLayoutManualBasicV1.Sexo].Trim(),
          DataAdmissao = dataAdmissao,
          Situacao = list[(int)EnumLayoutManualBasicV1.Situacao].Trim(),
          DataRetornoFerias = null,
          MotivoAfastamento = string.Empty,
          DataDemissao = dataDemissao,
          Cargo = list[(int)EnumLayoutManualBasicV1.DescricaoCargo].Trim().ToLower(),
          NomeCargo = list[(int)EnumLayoutManualBasicV1.DescricaoCargo].Trim(),
          DataUltimaTrocaCargo = null,
          GrauInstrucao = list[(int)EnumLayoutManualBasicV1.DescricaoGrauInstrucao].Trim().ToLower(),
          NomeGrauInstrucao = list[(int)EnumLayoutManualBasicV1.DescricaoGrauInstrucao].Trim(),
          SalarioNominal = 0,
          DataUltimoReajuste = null,
          EmpresaGestor = list[(int)EnumLayoutManualBasicV1.NomeEmpresaGestor].Trim().ToLower(),
          NomeEmpresaGestor = list[(int)EnumLayoutManualBasicV1.NomeEmpresaGestor].Trim(),
          DocumentoGestor = list[(int)EnumLayoutManualBasicV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty),
          MatriculaGestor = 0
        };
        if (!string.IsNullOrEmpty(Colaborador.DocumentoGestor))
          Colaborador.DocumentoGestor = Colaborador.DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualBasicV1.MatriculaGestor]))
          Colaborador.MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutManualBasicV1.MatriculaGestor].Trim());
        ValidarColaborador();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ImportarColaborador(List<string> list, EnumLayoutManualCompleteV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataRetornoFerias], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataRetornoFerias);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataUltimaTrocaCargo], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimaTrocaCargo);
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataUltimoReajuste], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimoReajuste);
        Colaborador = new ViewColaborador
        {
          Empresa = list[(int)EnumLayoutManualCompleteV1.NomeEmpresa].Trim().ToLower(),
          NomeEmpresa = list[(int)EnumLayoutManualCompleteV1.NomeEmpresa].Trim(),
          Estabelecimento = list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento].Trim().ToLower(),
          NomeEstabelecimento = list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento].Trim(),
          Documento = list[(int)EnumLayoutManualCompleteV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0'),
          Matricula = Convert.ToInt64(list[(int)EnumLayoutManualCompleteV1.Matricula].Trim()),
          Nome = list[(int)EnumLayoutManualCompleteV1.Nome].Trim(),
          Email = list[(int)EnumLayoutManualCompleteV1.Email].Trim().ToLower(),
          DataNascimento = dataNascimento,
          Celular = list[(int)EnumLayoutManualCompleteV1.Celular].Trim(),
          Telefone = list[(int)EnumLayoutManualCompleteV1.Telefone].Trim(),
          Identidade = list[(int)EnumLayoutManualCompleteV1.Identidade].Trim(),
          CarteiraProfissional = list[(int)EnumLayoutManualCompleteV1.CarteiraProfissional].Trim(),
          Sexo = list[(int)EnumLayoutManualCompleteV1.Sexo].Trim(),
          DataAdmissao = dataAdmissao,
          Situacao = list[(int)EnumLayoutManualCompleteV1.Situacao].Trim(),
          DataRetornoFerias = dataRetornoFerias,
          MotivoAfastamento = list[(int)EnumLayoutManualCompleteV1.MotivoAfastamento].Trim(),
          DataDemissao = dataDemissao,
          Cargo = list[(int)EnumLayoutManualCompleteV1.DescricaoCargo].Trim().ToLower(),
          NomeCargo = list[(int)EnumLayoutManualCompleteV1.DescricaoCargo].Trim(),
          DataUltimaTrocaCargo = dataUltimaTrocaCargo,
          GrauInstrucao = list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao].Trim().ToLower(),
          NomeGrauInstrucao = list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao].Trim(),
          SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutManualCompleteV1.Salario].Trim()),
          DataUltimoReajuste = dataUltimoReajuste,
          DocumentoGestor = list[(int)EnumLayoutManualCompleteV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty),
          EmpresaGestor = list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor].Trim().ToLower(),
          NomeEmpresaGestor = list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor].Trim(),
          MatriculaGestor = 0
        };
        if (!string.IsNullOrEmpty(Colaborador.DocumentoGestor))
          Colaborador.DocumentoGestor = Colaborador.DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor]))
          Colaborador.MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor].Trim());
        ValidarColaborador();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ImportarColaborador(List<string> list, EnumLayoutSystemBasicV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        Colaborador = new ViewColaborador
        {
          Empresa = list[(int)EnumLayoutSystemBasicV1.Empresa].Trim().ToLower(),
          NomeEmpresa = list[(int)EnumLayoutSystemBasicV1.NomeEmpresa].Trim(),
          Estabelecimento = "1",
          NomeEstabelecimento = "Estabelecimento Padrão",
          Documento = list[(int)EnumLayoutSystemBasicV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0'),
          Matricula = Convert.ToInt64(list[(int)EnumLayoutSystemBasicV1.Matricula].Trim()),
          Nome = list[(int)EnumLayoutSystemBasicV1.Nome].Trim(),
          Email = list[(int)EnumLayoutSystemBasicV1.Email].Trim().ToLower(),
          DataNascimento = dataNascimento,
          Celular = list[(int)EnumLayoutSystemBasicV1.Celular].Trim(),
          Telefone = list[(int)EnumLayoutSystemBasicV1.Telefone].Trim(),
          Identidade = string.Empty,
          CarteiraProfissional = string.Empty,
          Sexo = list[(int)EnumLayoutSystemBasicV1.Sexo].Trim(),
          DataAdmissao = dataAdmissao,
          Situacao = list[(int)EnumLayoutSystemBasicV1.Situacao].Trim(),
          DataRetornoFerias = null,
          MotivoAfastamento = string.Empty,
          DataDemissao = dataDemissao,
          Cargo = list[(int)EnumLayoutSystemBasicV1.Cargo].Trim()
        };
        ;
        Colaborador.NomeCargo = list[(int)EnumLayoutSystemBasicV1.DescricaoCargo].Trim();
        Colaborador.DataUltimaTrocaCargo = null;
        Colaborador.GrauInstrucao = list[(int)EnumLayoutSystemBasicV1.GrauInstrucao].Trim();
        Colaborador.NomeGrauInstrucao = list[(int)EnumLayoutSystemBasicV1.DescricaoGrauInstrucao].Trim();
        Colaborador.SalarioNominal = 0;
        Colaborador.DataUltimoReajuste = null;
        Colaborador.DocumentoGestor = list[(int)EnumLayoutSystemBasicV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        Colaborador.EmpresaGestor = list[(int)EnumLayoutSystemBasicV1.EmpresaGestor].Trim();
        Colaborador.NomeEmpresaGestor = list[(int)EnumLayoutSystemBasicV1.NomeEmpresaGestor].Trim();
        Colaborador.MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(Colaborador.DocumentoGestor))
          Colaborador.DocumentoGestor = Colaborador.DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor]))
          Colaborador.MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor].Trim());
        ValidarColaborador();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ImportarColaborador(List<string> list, EnumLayoutSystemCompleteV1 enumeration)
    {
      try
      {
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataNascimento], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataAdmissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataRetornoFerias], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataRetornoFerias);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataDemissao], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataUltimaTrocaCargo], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimaTrocaCargo);
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataUltimoReajuste], CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimoReajuste);
        Colaborador = new ViewColaborador
        {
          Empresa = list[(int)EnumLayoutSystemCompleteV1.Empresa].Trim().ToLower(),
          NomeEmpresa = list[(int)EnumLayoutSystemCompleteV1.NomeEmpresa].Trim(),
          Estabelecimento = list[(int)EnumLayoutSystemCompleteV1.Estabelecimento].Trim().ToLower(),
          NomeEstabelecimento = list[(int)EnumLayoutSystemCompleteV1.NomeEstabelecimento].Trim(),
          Documento = list[(int)EnumLayoutSystemCompleteV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0'),
          Matricula = Convert.ToInt64(list[(int)EnumLayoutSystemCompleteV1.Matricula].Trim()),
          Nome = list[(int)EnumLayoutSystemCompleteV1.Nome].Trim(),
          Email = list[(int)EnumLayoutSystemCompleteV1.Email].Trim().ToLower(),
          DataNascimento = dataNascimento,
          Celular = list[(int)EnumLayoutSystemCompleteV1.Celular].Trim(),
          Telefone = list[(int)EnumLayoutSystemCompleteV1.Telefone].Trim(),
          Identidade = list[(int)EnumLayoutSystemCompleteV1.Identidade].Trim(),
          CarteiraProfissional = list[(int)EnumLayoutSystemCompleteV1.CarteiraProfissional].Trim(),
          Sexo = list[(int)EnumLayoutSystemCompleteV1.Sexo].Trim(),
          DataAdmissao = dataAdmissao,
          Situacao = list[(int)EnumLayoutSystemCompleteV1.Situacao].Trim(),
          DataRetornoFerias = dataRetornoFerias,
          MotivoAfastamento = list[(int)EnumLayoutSystemCompleteV1.MotivoAfastamento].Trim(),
          DataDemissao = dataDemissao,
          Cargo = list[(int)EnumLayoutSystemCompleteV1.Cargo].Trim().ToLower(),
          NomeCargo = list[(int)EnumLayoutSystemCompleteV1.DescricaoCargo].Trim(),
          DataUltimaTrocaCargo = dataUltimaTrocaCargo,
          GrauInstrucao = list[(int)EnumLayoutSystemCompleteV1.GrauInstrucao].Trim(),
          NomeGrauInstrucao = list[(int)EnumLayoutSystemCompleteV1.DescricaoGrauInstrucao].Trim(),
          SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutSystemCompleteV1.Salario].Trim()),
          DataUltimoReajuste = dataUltimoReajuste,
          DocumentoGestor = list[(int)EnumLayoutSystemCompleteV1.CpfGestor].Trim().Replace(".", string.Empty).Replace("-", string.Empty),
          EmpresaGestor = list[(int)EnumLayoutSystemCompleteV1.EmpresaGestor].Trim().ToLower(),
          NomeEmpresaGestor = list[(int)EnumLayoutSystemCompleteV1.NomeEmpresaGestor].Trim(),
          MatriculaGestor = 0
        };
        if (!string.IsNullOrEmpty(Colaborador.DocumentoGestor))
          Colaborador.DocumentoGestor = Colaborador.DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor]))
          Colaborador.MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor].Trim());
        ValidarColaborador();
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Validação de dados do Colaborador
    private void ValidarColaborador()
    {
      try
      {
        Message = string.Empty;
        // Nome da Empresa
        if (string.IsNullOrEmpty(Colaborador.NomeEmpresa))
          Message = string.Concat(Message, "Nome da empresa vazia!;");
        // Valid Cpf
        if (string.IsNullOrEmpty(Colaborador.Documento))
          Message = string.Concat(Message, "CPF vazio!;");
        else
          if (!IsValidCPF(Colaborador.Documento))
          Message = string.Concat(Message, "CPF inválido!;");
        // Matricula obrigatória
        if (Colaborador.Matricula == 0)
          Message = string.Concat(Message, "Matricula não informada!;");
        // Nome
        if (string.IsNullOrEmpty(Colaborador.Nome))
          Message = string.Concat(Message, "Nome não informado!;");
        // E-mail
        if (string.IsNullOrEmpty(Colaborador.Email))
          Message = string.Concat(Message, "E-mail não informado!;");
        // Data admissão
        if (Colaborador.DataAdmissao == null)
          Message = string.Concat(Message, "Data de admissão inválida!;");
        // Situação
        if (!Colaborador.Situacao.Equals("Ativo", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Férias", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Ferias", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Afastado", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Demitido", StringComparison.InvariantCultureIgnoreCase))
          Message = string.Concat(Message, "Situação diferente de Ativo/Férias/Afastado/Demitido!;");
        if (Colaborador.Situacao.Equals("Demitido") && Colaborador.DataDemissao == null)
          Message = string.Concat(Message, "Data de demissão deve ser informada!;");
        // Nome do Cargo
        if (string.IsNullOrEmpty(Colaborador.NomeCargo))
          Message = string.Concat(Message, "Nome do cargo vazio!;");
        // Grau de Instrução, Nome do Grau de Instrução
        if (string.IsNullOrEmpty(Colaborador.NomeGrauInstrucao))
          Message = string.Concat(Message, "Nome do grau de instrução vazio!;");
        // Valid Cpf chefe
        if (!string.IsNullOrEmpty(Colaborador.DocumentoGestor))
          if (!IsValidCPF(Colaborador.DocumentoGestor))
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
