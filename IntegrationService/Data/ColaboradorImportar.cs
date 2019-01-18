﻿using IntegrationService.Enumns;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace IntegrationService.Data
{
  public class ColaboradorImportar : Colaborador
  {
    public ColaboradorImportar() {}
    public ColaboradorImportar(List<string> list, EnumLayoutManualBasicV1 enumeration)
    {
      try
      {
        Empresa = string.Empty;
        NomeEmpresa = list[(int)EnumLayoutManualBasicV1.NomeEmpresa].Trim();
        Estabelecimento = string.Empty;
        NomeEstabelecimento = string.Empty;
        Documento = list[(int)EnumLayoutManualBasicV1.Cpf].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutManualBasicV1.Matricula].ToString().Trim());
        Nome = list[(int)EnumLayoutManualBasicV1.Nome].ToString().Trim();
        Email = list[(int)EnumLayoutManualBasicV1.Email].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataNascimento].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutManualBasicV1.Celular].ToString().Trim();
        Telefone = list[(int)EnumLayoutManualBasicV1.Telefone].ToString().Trim();
        Identidade = string.Empty;
        CarteiraProfissional = string.Empty;
        Sexo = list[(int)EnumLayoutManualBasicV1.Sexo].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataAdmissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutManualBasicV1.Situacao].ToString().Trim();
        DataRetornoFerias = null;
        MotivoAfastamento = string.Empty;
        DateTime.TryParse(list[(int)EnumLayoutManualBasicV1.DataDemissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DataDemissao = dataDemissao;
        Cargo = string.Empty;
        NomeCargo = list[(int)EnumLayoutManualBasicV1.DescricaoCargo].ToString().Trim();
        DataUltimaTrocaCargo = null;
        GrauInstrucao = string.Empty;
        NomeGrauInstrucao = list[(int)EnumLayoutManualBasicV1.DescricaoGrauInstrucao].ToString().Trim();
        SalarioNominal = 0;
        DataUltimoReajuste = null;
        EmpresaGestor = string.Empty;
        DocumentoGestor = list[(int)EnumLayoutManualBasicV1.CpfGestor].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        NomeEmpresaGestor = list[(int)EnumLayoutManualBasicV1.NomeEmpresaGestor].ToString().Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualBasicV1.MatriculaGestor].ToString()))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutManualBasicV1.MatriculaGestor].ToString().Trim());
        TypeUser = (int)EnumTypeUser.Employee;
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
        Empresa = string.Empty;
        NomeEmpresa = list[(int)EnumLayoutManualCompleteV1.NomeEmpresa].Trim();
        Estabelecimento = string.Empty;
        NomeEstabelecimento = list[(int)EnumLayoutManualCompleteV1.NomeEstabelecimento].Trim();
        Documento = list[(int)EnumLayoutManualCompleteV1.Cpf].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutManualCompleteV1.Matricula].ToString().Trim());
        Nome = list[(int)EnumLayoutManualCompleteV1.Nome].ToString().Trim();
        Email = list[(int)EnumLayoutManualCompleteV1.Email].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataNascimento].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutManualCompleteV1.Celular].ToString().Trim();
        Telefone = list[(int)EnumLayoutManualCompleteV1.Telefone].ToString().Trim();
        Identidade = list[(int)EnumLayoutManualCompleteV1.Identidade].ToString().Trim();
        CarteiraProfissional = list[(int)EnumLayoutManualCompleteV1.CarteiraProfissional].ToString().Trim();
        Sexo = list[(int)EnumLayoutManualCompleteV1.Sexo].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataAdmissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutManualCompleteV1.Situacao].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataRetornoFerias].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataRetornoFerias);
        DataRetornoFerias = dataRetornoFerias;
        MotivoAfastamento = list[(int)EnumLayoutManualCompleteV1.MotivoAfastamento].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataDemissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DataDemissao = dataDemissao;
        Cargo = string.Empty;
        NomeCargo = list[(int)EnumLayoutManualCompleteV1.DescricaoCargo].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataUltimaTrocaCargo].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimaTrocaCargo);
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = string.Empty;
        NomeGrauInstrucao = list[(int)EnumLayoutManualCompleteV1.DescricaoGrauInstrucao].ToString().Trim();
        SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutManualCompleteV1.Salario].ToString().Trim());
        DateTime.TryParse(list[(int)EnumLayoutManualCompleteV1.DataUltimoReajuste].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimoReajuste);
        DataUltimoReajuste = dataUltimoReajuste;
        DocumentoGestor = list[(int)EnumLayoutManualCompleteV1.CpfGestor].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        EmpresaGestor = string.Empty;
        NomeEmpresaGestor = list[(int)EnumLayoutManualCompleteV1.NomeEmpresaGestor].ToString().Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor].ToString()))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutManualCompleteV1.MatriculaGestor].ToString().Trim());
        TypeUser = (int)EnumTypeUser.Employee;
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
        Empresa = list[(int)EnumLayoutSystemBasicV1.Empresa].Trim();
        NomeEmpresa = list[(int)EnumLayoutSystemBasicV1.NomeEmpresa].Trim();
        Estabelecimento = string.Empty;
        NomeEstabelecimento = string.Empty;
        Documento = list[(int)EnumLayoutSystemBasicV1.Cpf].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutSystemBasicV1.Matricula].ToString().Trim());
        Nome = list[(int)EnumLayoutSystemBasicV1.Nome].ToString().Trim();
        Email = list[(int)EnumLayoutSystemBasicV1.Email].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataNascimento].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutSystemBasicV1.Celular].ToString().Trim();
        Telefone = list[(int)EnumLayoutSystemBasicV1.Telefone].ToString().Trim();
        Identidade = string.Empty;
        CarteiraProfissional = string.Empty;
        Sexo = list[(int)EnumLayoutSystemBasicV1.Sexo].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataAdmissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutSystemBasicV1.Situacao].ToString().Trim();
        DataRetornoFerias = null;
        MotivoAfastamento = string.Empty;
        DateTime.TryParse(list[(int)EnumLayoutSystemBasicV1.DataDemissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DataDemissao = dataDemissao;
        Cargo = list[(int)EnumLayoutSystemBasicV1.Cargo].Trim(); ;
        NomeCargo = list[(int)EnumLayoutSystemBasicV1.DescricaoCargo].ToString().Trim();
        DataUltimaTrocaCargo = null;
        GrauInstrucao = list[(int)EnumLayoutSystemBasicV1.GrauInstrucao].Trim();
        NomeGrauInstrucao = list[(int)EnumLayoutSystemBasicV1.DescricaoGrauInstrucao].ToString().Trim();
        SalarioNominal = 0;
        DataUltimoReajuste = null;
        DocumentoGestor = list[(int)EnumLayoutSystemBasicV1.CpfGestor].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        EmpresaGestor = list[(int)EnumLayoutSystemBasicV1.EmpresaGestor].Trim();
        NomeEmpresaGestor = list[(int)EnumLayoutSystemBasicV1.NomeEmpresaGestor].ToString().Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor].ToString()))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutSystemBasicV1.MatriculaGestor].ToString().Trim());
        TypeUser = (int)EnumTypeUser.Employee;
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
        Empresa = list[(int)EnumLayoutSystemCompleteV1.Empresa].Trim();
        NomeEmpresa = list[(int)EnumLayoutSystemCompleteV1.NomeEmpresa].Trim();
        Estabelecimento = list[(int)EnumLayoutSystemCompleteV1.Estabelecimento].Trim();
        NomeEstabelecimento = list[(int)EnumLayoutSystemCompleteV1.NomeEstabelecimento].Trim();
        Documento = list[(int)EnumLayoutSystemCompleteV1.Cpf].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11,'0');
        Matricula = Convert.ToInt64(list[(int)EnumLayoutSystemCompleteV1.Matricula].ToString().Trim());
        Nome = list[(int)EnumLayoutSystemCompleteV1.Nome].ToString().Trim();
        Email = list[(int)EnumLayoutSystemCompleteV1.Email].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataNascimento].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataNascimento);
        DataNascimento = dataNascimento;
        Celular = list[(int)EnumLayoutSystemCompleteV1.Celular].ToString().Trim();
        Telefone = list[(int)EnumLayoutSystemCompleteV1.Telefone].ToString().Trim();
        Identidade = list[(int)EnumLayoutSystemCompleteV1.Identidade].ToString().Trim();
        CarteiraProfissional = list[(int)EnumLayoutSystemCompleteV1.CarteiraProfissional].ToString().Trim();
        Sexo = list[(int)EnumLayoutSystemCompleteV1.Sexo].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataAdmissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataAdmissao);
        DataAdmissao = dataAdmissao;
        Situacao = list[(int)EnumLayoutSystemCompleteV1.Situacao].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataRetornoFerias].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataRetornoFerias);
        DataRetornoFerias = dataRetornoFerias;
        MotivoAfastamento = list[(int)EnumLayoutSystemCompleteV1.MotivoAfastamento].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataDemissao].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataDemissao);
        DataDemissao = dataDemissao;
        Cargo = list[(int)EnumLayoutSystemCompleteV1.Cargo].Trim();
        NomeCargo = list[(int)EnumLayoutSystemCompleteV1.DescricaoCargo].ToString().Trim();
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataUltimaTrocaCargo].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimaTrocaCargo);
        DataUltimaTrocaCargo = dataUltimaTrocaCargo;
        GrauInstrucao = list[(int)EnumLayoutSystemCompleteV1.GrauInstrucao].Trim();
        NomeGrauInstrucao = list[(int)EnumLayoutSystemCompleteV1.DescricaoGrauInstrucao].ToString().Trim();
        SalarioNominal = Convert.ToDecimal(list[(int)EnumLayoutSystemCompleteV1.Salario].ToString().Trim());
        DateTime.TryParse(list[(int)EnumLayoutSystemCompleteV1.DataUltimoReajuste].ToString(), CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dataUltimoReajuste);
        DataUltimoReajuste = dataUltimoReajuste;
        DocumentoGestor = list[(int)EnumLayoutSystemCompleteV1.CpfGestor].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        if (!string.IsNullOrEmpty(DocumentoGestor))
          DocumentoGestor = DocumentoGestor.PadLeft(11, '0');
        EmpresaGestor = list[(int)EnumLayoutSystemCompleteV1.EmpresaGestor].Trim();
        NomeEmpresaGestor = list[(int)EnumLayoutSystemCompleteV1.NomeEmpresaGestor].ToString().Trim();
        MatriculaGestor = 0;
        if (!string.IsNullOrEmpty(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor].ToString()))
          MatriculaGestor = Convert.ToInt64(list[(int)EnumLayoutSystemCompleteV1.MatriculaGestor].ToString().Trim());
        TypeUser = (int)EnumTypeUser.Employee;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}