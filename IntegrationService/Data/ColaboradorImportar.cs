using IntegrationService.Enumns;
using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Globalization;

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

        DateTime? dataNascimento = FieldDate(list[(int)EnumLayoutManualBasicV1.DataNascimento]);
        DateTime? dataAdmissao = FieldDate(list[(int)EnumLayoutManualBasicV1.DataAdmissao]);
        DateTime? dataDemissao = FieldDate(list[(int)EnumLayoutManualBasicV1.DataDemissao]);
        Empresa = FormatedFieldKey(list[(int)EnumLayoutManualBasicV1.NomeEmpresa]);
        NomeEmpresa = FormatedField(list[(int)EnumLayoutManualBasicV1.NomeEmpresa]);
        Estabelecimento = "1";
        NomeEstabelecimento = "Estabelecimento Padrão";
        Documento = list[(int)EnumLayoutManualBasicV1.Cpf].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = list[(int)EnumLayoutManualBasicV1.Matricula].Trim();
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
        DocumentoGestor = list[(int)EnumLayoutManualBasicV1.CpfGestor].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty);
        EstabelecimentoGestor = "1";
        NomeEstabelecimentoGestor = "Estabelecimento Padrão";
        MatriculaGestor = string.Empty;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualBasicV1.MatriculaGestor]))
          MatriculaGestor = list[(int)EnumLayoutManualBasicV1.MatriculaGestor].Trim();
        ValidDataColaborator();
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
        DateTime? dataNascimento = FieldDate(list[(int)EnumLayoutManualCompleteV1.DataNascimento]);
        DateTime? dataAdmissao = FieldDate(list[(int)EnumLayoutManualCompleteV1.DataAdmissao]);
        DateTime? dataRetornoFerias = FieldDate(list[(int)EnumLayoutManualCompleteV1.DataRetornoFerias]);
        DateTime? dataDemissao = FieldDate(list[(int)EnumLayoutManualCompleteV1.DataDemissao]);
        DateTime? dataUltimaTrocaCargo = FieldDate(list[(int)EnumLayoutManualCompleteV1.DataUltimaTrocaCargo]);
        DateTime? dataUltimoReajuste = FieldDate(list[(int)EnumLayoutManualCompleteV1.DataUltimoReajuste]);
        Empresa = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.NomeEmpresa]);
        NomeEmpresa = FormatedField(list[(int)EnumLayoutManualCompleteV1.NomeEmpresa]);
        Estabelecimento = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento]);
        NomeEstabelecimento = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento]);
        Documento = list[(int)EnumLayoutManualCompleteV1.Cpf].Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = list[(int)EnumLayoutManualCompleteV1.Matricula].Trim();
        Nome = FormatedField(list[(int)EnumLayoutManualCompleteV1.Nome]);
        Email = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.Email]);
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
        Cargo = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.DescricaoCargo]);
        NomeCargo = FormatedField(list[(int)EnumLayoutManualCompleteV1.DescricaoCargo]);
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao]);
        NomeGrauInstrucao = FormatedField(list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao]);
        SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutManualCompleteV1.Salario].Trim());
        DataUltimoReajuste = dataUltimoReajuste;
        DocumentoGestor = list[(int)EnumLayoutManualCompleteV1.CpfGestor].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty);
        EmpresaGestor = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor]);
        NomeEmpresaGestor = FormatedField(list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor]);
        EstabelecimentoGestor = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimentoGestor]);
        NomeEstabelecimentoGestor = FormatedFieldKey(list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimentoGestor]);
        MatriculaGestor = string.Empty;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor]))
          MatriculaGestor = list[(int)EnumLayoutManualCompleteV1.MatriculaGestor].Trim();
        ValidDataColaborator();
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
        DateTime? dataNascimento = FieldDate(list[(int)EnumLayoutSystemBasicV1.DataNascimento]);
        DateTime? dataAdmissao = FieldDate(list[(int)EnumLayoutSystemBasicV1.DataAdmissao]);
        DateTime? dataDemissao = FieldDate(list[(int)EnumLayoutSystemBasicV1.DataDemissao]);
        Empresa = FormatedFieldKey(list[(int)EnumLayoutSystemBasicV1.Empresa]);
        NomeEmpresa = FormatedField(list[(int)EnumLayoutSystemBasicV1.NomeEmpresa]);
        Estabelecimento = "1";
        NomeEstabelecimento = "Estabelecimento Padrão";
        Documento = list[(int)EnumLayoutSystemBasicV1.Cpf].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = list[(int)EnumLayoutSystemBasicV1.Matricula].Trim();
        Nome = FormatedField(list[(int)EnumLayoutSystemBasicV1.Nome]);
        Email = FormatedFieldKey(list[(int)EnumLayoutSystemBasicV1.Email]);
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
        Cargo = FormatedFieldKey(list[(int)EnumLayoutSystemBasicV1.Cargo]);
        NomeCargo = FormatedField(list[(int)EnumLayoutSystemBasicV1.DescricaoCargo]);
        DataUltimaTrocaCargo = null;
        GrauInstrucao = FormatedFieldKey(list[(int)EnumLayoutSystemBasicV1.GrauInstrucao]);
        NomeGrauInstrucao = FormatedField(list[(int)EnumLayoutSystemBasicV1.DescricaoGrauInstrucao]);
        SalarioNominal = 0;
        DataUltimoReajuste = null;
        DocumentoGestor = list[(int)EnumLayoutSystemBasicV1.CpfGestor].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty);
        EmpresaGestor = FormatedFieldKey(list[(int)EnumLayoutSystemBasicV1.EmpresaGestor]);
        NomeEmpresaGestor = FormatedField(list[(int)EnumLayoutSystemBasicV1.NomeEmpresaGestor]);
        EstabelecimentoGestor = "1";
        NomeEstabelecimentoGestor = "Estabelecimento Padrão";
        MatriculaGestor = string.Empty;
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor]))
          MatriculaGestor = list[(int)EnumLayoutSystemBasicV1.MatriculaGestor].Trim();
        ValidDataColaborator();
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
        DateTime? dataNascimento = FieldDate(list[(int)EnumLayoutSystemCompleteV1.DataNascimento]);
        DateTime? dataAdmissao = FieldDate(list[(int)EnumLayoutSystemCompleteV1.DataAdmissao]);
        DateTime? dataRetornoFerias = FieldDate(list[(int)EnumLayoutSystemCompleteV1.DataRetornoFerias]);
        DateTime? dataDemissao = FieldDate(list[(int)EnumLayoutSystemCompleteV1.DataDemissao]);
        DateTime? dataUltimaTrocaCargo = FieldDate(list[(int)EnumLayoutSystemCompleteV1.DataUltimaTrocaCargo]);
        DateTime? dataUltimoReajuste = FieldDate(list[(int)EnumLayoutSystemCompleteV1.DataUltimoReajuste]);
        Empresa = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.Empresa]);
        NomeEmpresa = FormatedField(list[(int)EnumLayoutSystemCompleteV1.NomeEmpresa]);
        Estabelecimento = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.Estabelecimento]);
        NomeEstabelecimento = FormatedField(list[(int)EnumLayoutSystemCompleteV1.NomeEstabelecimento]);
        Documento = list[(int)EnumLayoutSystemCompleteV1.Cpf].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = list[(int)EnumLayoutSystemCompleteV1.Matricula].Trim();
        Nome = FormatedField(list[(int)EnumLayoutSystemCompleteV1.Nome]);
        Email = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.Email]);
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
        Cargo = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.Cargo]);
        NomeCargo = FormatedField(list[(int)EnumLayoutSystemCompleteV1.DescricaoCargo]);
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.GrauInstrucao]);
        NomeGrauInstrucao = FormatedField(list[(int)EnumLayoutSystemCompleteV1.DescricaoGrauInstrucao]);
        SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutSystemCompleteV1.Salario].Trim());
        DataUltimoReajuste = dataUltimoReajuste;
        EmpresaGestor = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.EmpresaGestor]);
        NomeEmpresaGestor = FormatedField(list[(int)EnumLayoutSystemCompleteV1.NomeEmpresaGestor]);
        EstabelecimentoGestor = FormatedFieldKey(list[(int)EnumLayoutSystemCompleteV1.EstabelecimentoGestor]);
        NomeEstabelecimentoGestor = FormatedField(list[(int)EnumLayoutSystemCompleteV1.NomeEstabelecimentoGestor]);
        DocumentoGestor = list[(int)EnumLayoutSystemCompleteV1.CpfGestor].Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty);
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        MatriculaGestor = string.Empty;
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor]))
          MatriculaGestor = list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor].Trim();
        ValidDataColaborator();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorImportar(ViewIntegrationUnimedNers view, EnumLayoutSystemCompleteV1 enumeration)
    {
      try
      {
        DateTime? dataNascimento = FieldDate(view.dat_nascimento);
        DateTime? dataAdmissao = FieldDate(view.dat_admissao);
        DateTime? dataRetornoFerias = FieldDate(view.dat_ret_ferias);
        DateTime? dataDemissao = FieldDate(view.dat_demissao);
        DateTime? dataUltimaTrocaCargo = FieldDate(view.dat_ult_troca_cargo);
        DateTime? dataUltimoReajuste = FieldDate(view.dat_ult_reajuste);
        Empresa = FormatedFieldKey(view.cdn_empresa);
        NomeEmpresa = FormatedField(view.nom_empresa);
        Estabelecimento = FormatedFieldKey(view.cdn_estab);
        NomeEstabelecimento = FormatedField(view.nom_estab);
        Documento = view.cdn_cpf.Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0');
        Matricula = view.cdn_matricula.ToString();
        Nome = FormatedField(view.nom_pessoa);
        Email = FormatedFieldKey(view.nom_email);
        DataNascimento = dataNascimento;
        Celular = view.num_celular.Trim();
        Telefone = view.num_telefone.Trim();
        Identidade = view.cdn_identidade.Trim();
        CarteiraProfissional = view.cdn_cart_prof.Trim();
        Sexo = view.tip_sexo.Trim();
        DataAdmissao = dataAdmissao;
        Situacao = view.sit_funcionario.Trim();
        DataRetornoFerias = dataRetornoFerias;
        MotivoAfastamento = view.des_mot_afastamento.Trim();
        DataDemissao = dataDemissao;
        Cargo = FormatedFieldKey(view.cdn_cargo.ToString());
        NomeCargo = FormatedField(view.des_cargo);
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = FormatedFieldKey(view.cdn_grau_instrucao.ToString());
        NomeGrauInstrucao = FormatedField(view.nom_grau_instrucao);
        SalarioNominal = view.val_salario_atual;
        DataUltimoReajuste = dataUltimoReajuste;
        //EmpresaGestor = FormatedFieldKey(view.cdn_empresa_chefe);
        //NomeEmpresaGestor = FormatedField(view.nom_empresa_chefe);
        //EstabelecimentoGestor = FormatedFieldKey(view.cdn_estab_chefe);
        //NomeEstabelecimentoGestor = FormatedField(view.nom_estab_chefe);
        //DocumentoGestor = view.cdn_cpf_chefe.Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty);
        //if (!string.IsNullOrEmpty(DocumentoGestor))
        //  DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        //MatriculaGestor = string.Empty;
        //if (view.cdn_matricula_chefe != 0)
        //  MatriculaGestor = view.cdn_matricula_chefe.ToString();
        EmpresaGestor = string.Empty;
        NomeEmpresaGestor = string.Empty;
        EstabelecimentoGestor = string.Empty;
        NomeEstabelecimentoGestor = string.Empty;
        DocumentoGestor = string.Empty;
        MatriculaGestor = string.Empty;
        Apelido = view.cod_usuario_ad;
        ValidDataColaborator();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorImportar(ViewIntegrationMetadadosV1 view, EnumLayoutSystemCompleteV1 enumeration)
    {
      try
      {
        DateTime? dataNascimento = null;
        DateTime? dataAdmissao = FieldDate(view.DataAdmissao);
        DateTime? dataDemissao = FieldDate(view.DataDemissao);
        DateTime? dataUltimaTrocaCargo = FieldDate(view.DataTrocaCargo);
        DateTime? dataUltimoReajuste = FieldDate(view.DataUltimoReajuste);
        Empresa = FormatedFieldKey(view.Empresa);
        NomeEmpresa = FormatedField(view.NomeEmpresa);
        Estabelecimento = FormatedFieldKey(view.Estabelecimento);
        NomeEstabelecimento = FormatedField(view.NomeEstabelecimento);
        Documento = view.Cpf.Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0');
        Matricula = view.Matricula.ToString();
        Nome = FormatedField(view.Nome);
        Email = FormatedFieldKey(view.Email);
        Celular = null;
        Telefone = null;
        DataNascimento = dataNascimento;
        Sexo = view.Sexo.Trim();
        DataAdmissao = dataAdmissao;
        Situacao = view.Situacao.Trim();
        DataRetornoFerias = null;
        MotivoAfastamento = null;
        DataDemissao = dataDemissao;
        Cargo = FormatedFieldKey(view.Cargo.ToString());
        NomeCargo = FormatedField(view.NomeCargo);
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = FormatedFieldKey(view.GrauInstrucao.ToString());
        NomeGrauInstrucao = FormatedField(view.NomeGrauInstrucao);
        SalarioNominal = view.SalarioNominal;
        DataUltimoReajuste = dataUltimoReajuste;
        //EmpresaGestor = FormatedFieldKey(view.cdn_empresa_chefe);
        //NomeEmpresaGestor = FormatedField(view.nom_empresa_chefe);
        //EstabelecimentoGestor = FormatedFieldKey(view.cdn_estab_chefe);
        //NomeEstabelecimentoGestor = FormatedField(view.nom_estab_chefe);
        //DocumentoGestor = view.cdn_cpf_chefe.Trim().ToLower().Replace(".", string.Empty).Replace("-", string.Empty);
        //if (!string.IsNullOrEmpty(DocumentoGestor))
        //  DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        //MatriculaGestor = string.Empty;
        //if (view.cdn_matricula_chefe != 0)
        //  MatriculaGestor = view.cdn_matricula_chefe.ToString();
        EmpresaGestor = string.Empty;
        NomeEmpresaGestor = string.Empty;
        EstabelecimentoGestor = string.Empty;
        NomeEstabelecimentoGestor = string.Empty;
        DocumentoGestor = string.Empty;
        MatriculaGestor = string.Empty;
        Apelido = view.UsuarioAd;
        ValidDataColaborator();
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
        result = result.Replace(" Da "," da ").Replace(" De ", " de ").Replace(" Do ", " do ").Replace(" Dos ", " dos ")
          .Replace(" Iii", " III").Replace(" Ii", " II").Replace(" Em "," em ").Replace(" Ti", " TI")
          .Replace(" Pl", " PL").Replace(" Jr", " JR").Replace(" Sr", " SR");
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
    private DateTime? FieldDate(string data)
    {
      try
      {
        DateTime? result = null;
        DateTime.TryParse(data.Substring(0, 10), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.AdjustToUniversal, out DateTime resultParse);
        if (!string.IsNullOrEmpty(data))
          result = resultParse.ToUniversalTime();
        return result;

      }
      catch (Exception)
      {
        return null;
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
        if (string.IsNullOrEmpty(Matricula))
          Message = string.Concat(Message, "Matricula não informada!;");
        // Nome
        if (string.IsNullOrEmpty(Nome))
          Message = string.Concat(Message, "Nome não informado!;");
        // E-mail
        //if (string.IsNullOrEmpty(Email))
        //  Message = string.Concat(Message, "E-mail não informado!;");
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
          NomeGrauInstrucao = "Ensino Médio Completo";
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
