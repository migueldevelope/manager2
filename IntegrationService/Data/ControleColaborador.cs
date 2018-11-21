using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manager.Views.Enumns;

namespace IntegrationService.Data
{
  public class ControleColaborador
  {
    public string ChaveColaborador { get; set; }
    public ColaboradorImportar Colaborador { get; private set; }
    public ColaboradorImportar ColaboradorAnterior { get; private set; }
    public List<string> CamposAlterados { get; set; }
    public EnumColaboradorAcao Acao { get; set; }
    public EnumColaboradorSituacao Situacao { get; set; }
    public string Message { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }

    public ControleColaborador(ColaboradorImportar colaborador)
    {
      ChaveColaborador = colaborador.ChaveColaborador;
      Colaborador = colaborador;
      CamposAlterados = new List<string>();
      Acao = EnumColaboradorAcao.Insert;
      Situacao = EnumColaboradorSituacao.Pendent;
      Message = string.Empty;
      ValidDataColaborator();
    }

    public void SetColaborador(ColaboradorImportar colaborador)
    {
      Message = string.Empty;
      if (Situacao == EnumColaboradorSituacao.Atualized)
      {
        Acao = EnumColaboradorAcao.Update;
        Situacao = EnumColaboradorSituacao.Pendent;
        ColaboradorAnterior = Colaborador;
      }
      Colaborador = colaborador;
      ValidDataColaborator();
      if (string.IsNullOrEmpty(Message) && ColaboradorAnterior != null)
        TestarMudanca();
    }

    #region Validação de dados do Colaborador
    private void ValidDataColaborator()
    {
      try
      {
        // Nome da Empresa
        if (string.IsNullOrEmpty(Colaborador.NomeEmpresa))
          Message = string.Concat(Message, "Nome da empresa vazia!;");
        // Valid Cpf
        if (string.IsNullOrEmpty(Colaborador.Documento))
          Message = string.Concat(Message, "CPF vazio!");
        else
          if (!IsValidCPF(Colaborador.Documento))
          Message = string.Concat(Message, "CPF inválido!");
        // Matricula obrigatória
        if (Colaborador.Matricula == 0)
          Message = string.Concat(Message, "Matricula não informada!;");
        // Nome
        if (string.IsNullOrEmpty(Colaborador.Nome))
          Message = string.Concat(Message, "Nome não informado!");
        // E-mail
        if (string.IsNullOrEmpty(Colaborador.Email))
          Message = string.Concat(Message, "E-mail não informado!");
        // Data admissão
        if (Colaborador.DataAdmissao == null)
          Message = string.Concat(Message, "Data de admissão inválida!");
        // Situação
        if (!Colaborador.Situacao.Equals("Ativo", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Férias", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Ferias", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Afastado", StringComparison.InvariantCultureIgnoreCase) &&
            !Colaborador.Situacao.Equals("Demitido", StringComparison.InvariantCultureIgnoreCase))
          Message = string.Concat(Message, "Situação diferente de Ativo/Férias/Afastado/Demitido!");
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
            Message = string.Concat(Message, "Documento do gestor é inválido!");
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

    #region Testar mudança de propriedades
    public string TestarMudanca()
    {
      try
      {
        if (this != null && Colaborador != null)
        {
          var type = typeof(ColaboradorImportar);
          var unequalProperties =
              from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
              let selfValue = type.GetProperty(pi.Name).GetValue(ColaboradorAnterior, null)
              let toValue = type.GetProperty(pi.Name).GetValue(Colaborador, null)
              where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
              select new MudancaColaborador() { Campo = pi.Name, ValorAntigo = selfValue.ToString(), ValorNovo = toValue.ToString() };
          if (unequalProperties.Count() == 0)
          {
            CamposAlterados = new List<string>();
            Acao = EnumColaboradorAcao.Passed;
            Situacao = EnumColaboradorSituacao.Atualized;
          }
          else
          {
            foreach (var item in unequalProperties.ToList<MudancaColaborador>())
              CamposAlterados.Add(item.Campo);
          }
        }
        return string.Empty;
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }
    #endregion

  }
}
