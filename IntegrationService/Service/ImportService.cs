using IntegrationService.Api;
using IntegrationService.Core;
using IntegrationService.Data;
using IntegrationService.Enumns;
using IntegrationService.Tools;
using IntegrationService.Views;
using IntegrationService.Views.Person;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace IntegrationService.Service
{
  public class ImportService
  {
    private List<Colaborador> Lista;
    private readonly ViewPersonLogin Person;
    public string Message { get; set; }
    private List<ViewIntegrationMapOfV1> Schoolings;
    private List<ViewIntegrationMapOfV1> Companys;
    private List<ViewIntegrationMapOfV1> Establishments;
    private List<ViewIntegrationMapOfV1> Occupations;
    private List<ViewIntegrationMapManagerV1> Managers;
    private List<MapPerson> Collaborators;
    private ViewIntegrationParameter Param;
    public ImportService(ViewPersonLogin person)
    {
      try
      {
        Person = person;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #region Preparação Banco de Dados V1
    public void DatabaseV1(ViewIntegrationParameter param)
    {
      try
      {
        Param = param;
        // Validar parâmetros
        if (string.IsNullOrEmpty(Param.ConnectionString))
          throw new Exception("Sem string de conexão!!!");

        if (string.IsNullOrEmpty(Param.SqlCommand))
          throw new Exception("Sem comando de leitura do banco de dados!!!");

        if (param.Type == EnumIntegrationType.Custom)
          throw new Exception("Rotina deve ser do tipo customizada!!!");

        // Ler banco de dados
        DataTable readData = ReadData();
        if (readData.Rows.Count == 0)
          throw new Exception("Não tem nenhum colaborador como retorno da consulta!!!");

        // Tratar os dados
        string columnsOk = string.Empty;
        switch (param.Type)
        {
          case EnumIntegrationType.Basic:
            columnsOk = ValidColumnBasicV1(readData.Columns);
            break;
          case EnumIntegrationType.Complete:
            columnsOk = ValidColumnCompleteV1(readData.Columns);
            break;
        }
        if (!string.IsNullOrEmpty(columnsOk))
          throw new Exception(columnsOk);
        // Carregar Lista de Colaboradores
        List<Colaborador> lista = new List<Colaborador>();
        Colaborador colaborador;
        foreach (DataRow row in readData.Rows)
        {
          DateTime.TryParse(row["data_nascimento"].ToString().Trim(), out DateTime dataNascimento);
          DateTime.TryParse(row["data_admissao"].ToString().Trim(), out DateTime dataAdmissao);
          colaborador = new Colaborador()
          {
            Empresa = row["empresa"].ToString().Trim(),
            NomeEmpresa = row["nome_empresa"].ToString().Trim(),
            Estabelecimento = row["empresa"].ToString().Trim(),
            NomeEstabelecimento = row["nome_empresa"].ToString().Trim(),
            Documento = row["cpf"].ToString().Trim().Replace(".",string.Empty).Replace("-",string.Empty),
            Matricula = Convert.ToInt64(row["empresa"].ToString().Trim()),
            Nome = row["nome"].ToString().Trim(),
            Email = row["email"].ToString().Trim(),
            DataNascimento = dataNascimento,
            Celular = row["celular"].ToString().Trim(),
            Telefone = row["telefone"].ToString().Trim(),
            Identidade = string.Empty,
            CarteiraProfissional = string.Empty,
            Sexo = row["sexo"].ToString().Trim(),
            DataAdmissao = dataAdmissao,
            Situacao = row["situacao"].ToString().Trim(),
            DataRetornoFerias = null,
            MotivoAfastamento = string.Empty,
            DataDemissao = null,
            Cargo = string.Empty,
            NomeCargo = row["nome_cargo"].ToString().Trim(),
            DataUltimaTrocaCargo = null,
            GrauInstrucao = string.Empty,
            NomeGrauInstrucao = row["nome_grau_instrucao"].ToString().Trim(),
            SalarioNominal = 0,
            DataUltimoReajuste = null,
            DocumentoChefe = row["cpf_chefe"].ToString().Trim().Replace(".", string.Empty).Replace("-", string.Empty),
            EmpresaChefe = row["empresa_chefe"].ToString().Trim(),
            NomeEmpresaChefe = string.Empty,
            MatriculaChefe = Convert.ToInt64(row["matricula_chefe"].ToString().Trim()),
            NomeChefe = row["nome_chefe"].ToString().Trim(),
          };
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    private DataTable ReadData()
    {
      try
      {
        Boolean oracle = Param.ConnectionString.Split(';')[0].Equals("Oracle");
        string hostname = Param.ConnectionString.Split(';')[1];
        string user = Param.ConnectionString.Split(';')[2];
        string password = Param.ConnectionString.Split(';')[3];
        string baseDefault = Param.ConnectionString.Split(';')[4];
        ConnectionString conn;
        if (oracle)
          conn = new ConnectionString(hostname, user, password)
          {
            Sql = Param.SqlCommand
          };
        else
          conn = new ConnectionString(hostname, user, password, baseDefault)
          {
            Sql = Param.SqlCommand
          };
        GetPersonSystem gps = new GetPersonSystem(conn);
        return gps.GetPerson();
      }
      catch (Exception)
      {
        throw;
      }
    }
    private string ValidColumnBasicV1(DataColumnCollection columns)
    {
      try
      {
        List<string> columnValids = new List<string>()
        {
          "empresa", "nome_empresa", "cpf", "matricula", "nome", "email", "data_nascimento", "celular",
          "telefone", "sexo", "data_admissão", "situacao", "data_demissao", "cargo", "nome_cargo",
          "grau_instrucao", "nome_grau_instrucao", "empresa_chefe", "cpf_chefe", "matricula_chefe", "nome_chefe"
        };
        if (columns.Count != 21)
          return "Não tem 21 colunas!!!";
        return ValidColumns(columns, columnValids);
      }
      catch (Exception)
      {
        throw;
      }
    }
    private string ValidColumnCompleteV1(DataColumnCollection columns)
    {
      try
      {
        List<string> columnValids = new List<string>()
        {
          "empresa", "nome_empresa", "estabelecimento", "nome_estabelecimento", "cpf", "matricula", "nome", "email",
          "data_nascimento", "celular", "telefone", "identidade", "carteira_profissional", "sexo", "data_admissão",
          "situacao", "retorno_ferias", "motivo_afastamento", "data_demissao", "cargo", "nome_cargo",
          "data_ultima_troca_cargo", "grau_instrucao", "nome_grau_instrucao", "salario", "data_ultimo_reajuste",
          "empresa_chefe", "cpf_chefe", "matricula_chefe", "nome_chefe"

        };
        if (columns.Count != 30)
          return "Não tem 21 colunas!!!";
        return ValidColumns(columns, columnValids);
      }
      catch (Exception)
      {
        throw;
      }
    }

    private string ValidColumns(DataColumnCollection columns, List<string> columnValids)
    {
      try
      {
        List<string> wrongNames = new List<string>();
        foreach (DataColumn column in columns)
        {
          if (!columnValids.Contains(column.Caption.ToLower()))
            wrongNames.Add(column.Caption);
        }
        List<string> missingNames = new List<string>();
        foreach (string columnNameValid in columnValids)
        {
          if (!columns.Contains(columnNameValid))
            missingNames.Add(columnNameValid);
        }
        string messageReturn = string.Empty;
        if (wrongNames.Count > 0)
          messageReturn = string.Concat(Environment.NewLine, string.Format("Nomes de colunas errados ({0}) !!!", string.Join(",", wrongNames)));

        if (missingNames.Count > 0)
          messageReturn = string.Concat(messageReturn, Environment.NewLine, string.Format("Nomes de colunas faltantes ({0}) !!!", string.Join(",", missingNames)));

        return messageReturn;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Preparação Arquivo CSV V1
    #endregion

    #region Preparação Arquivo Excel V1
    #endregion

    #region Importação Final Completa
    public void FinalImport(List<Colaborador> lista)
    {
      try
      {
        Lista = lista;
        LoadLists();
        Import();
      }
      catch (Exception)
      {
        throw;
      }
    }
    private void LoadLists()
    {
      try
      {
        Companys = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/empresa.txt", Person.IdAccount)))
          Companys = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/empresa.txt", Person.IdAccount)));
        Schoolings = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/grau.txt", Person.IdAccount)))
          Schoolings = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/grau.txt", Person.IdAccount)));
        Establishments = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount)))
          Establishments = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount)));
        Occupations = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/cargo.txt", Person.IdAccount)))
          Occupations = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/cargo.txt", Person.IdAccount)));
        Collaborators = new List<MapPerson>();
        if (File.Exists(string.Format("{0}/integration/colaborador.txt", Person.IdAccount)))
          Collaborators = JsonConvert.DeserializeObject<List<MapPerson>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount)));
        Managers = new List<ViewIntegrationMapManagerV1>();
        if (File.Exists(string.Format("{0}/integration/gestor.txt", Person.IdAccount)))
          Managers = JsonConvert.DeserializeObject<List<ViewIntegrationMapManagerV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/gestor.txt", Person.IdAccount)));
      }
      catch (Exception)
      {
        throw;
      }
    }
    private void Import()
    {
      foreach (var colaborador in Lista)
      {
        PersonIntegration personIntegration = new PersonIntegration(Person);
        // Validar Grau Instrução, Empresa, Estabelecimento, Cargo
        ViewIntegrationMapOfV1 schooling = VerifyMapOf(Schoolings, EnumValidKey.Schooling, colaborador.GrauInstrucao, colaborador.NomeGrauInstrucao);
        ViewIntegrationMapOfV1 company = VerifyMapOf(Companys, EnumValidKey.Company, colaborador.Empresa, colaborador.NomeEmpresa);
        ViewIntegrationMapOfV1 establishment = VerifyMapOf(Establishments, EnumValidKey.Establishment, colaborador.Estabelecimento, colaborador.NomeEstabelecimento, company.Id);
        ViewIntegrationMapOfV1 occupation = VerifyMapOf(Occupations, EnumValidKey.Occupation, colaborador.Cargo, colaborador.NomeCargo, company.Id);
        ViewIntegrationMapOfV1 companyManager = null;
        ViewIntegrationMapManagerV1 manager = null;
        // Validar chefe (manager)
        if (colaborador.DocumentoChefe != null && colaborador.NomeEmpresaChefe != null && colaborador.MatriculaChefe != 0)
        {
          companyManager = VerifyMapOf(Companys, EnumValidKey.Company, colaborador.EmpresaChefe, colaborador.NomeEmpresaChefe);
          ViewIntegrationMapManagerV1 viewManager = new ViewIntegrationMapManagerV1()
          {
            Document = colaborador.DocumentoChefe,
            CompanyId = companyManager.Id,
            CompanyCode = companyManager.Code,
            Registration = colaborador.MatriculaChefe,
            IdContract = null,
            IdPerson = null,
            Name = string.Empty,
            Message = string.Empty
          };
          int searchManager = Managers.FindIndex(p => p.Document == colaborador.DocumentoChefe && p.CompanyCode == colaborador.EmpresaChefe && p.Registration == colaborador.MatriculaChefe);
          if (searchManager == -1)
          {
            manager = personIntegration.GetManagerByKey(viewManager);
            if (manager != null)
              Managers.Add(manager);
          }
          else
          {
            if (string.IsNullOrEmpty(Managers[searchManager].IdPerson))
            {
              viewManager = new ViewIntegrationMapManagerV1()
              {
                Document = colaborador.DocumentoChefe,
                CompanyId = companyManager.Id,
                CompanyCode = companyManager.Code,
                Registration = colaborador.MatriculaChefe,
                IdContract = null,
                IdPerson = null,
                Name = string.Empty,
                Message = string.Empty
              };
              manager = personIntegration.GetManagerByKey(viewManager);
              if (manager != null)
                Managers[searchManager] = manager;
            }
          }
        }
        // Validar Colaborador
        int search = Collaborators.FindIndex(p => p.Documento == colaborador.Documento && p.Empresa == colaborador.Empresa && p.Matricula == colaborador.Matricula);
        if (search == -1)
        {
          ValidMapCollaborator validMapCollaborator = new ValidMapCollaborator(Person, colaborador.Documento, company, Convert.ToInt64(colaborador.Matricula), colaborador.Nome);
          validMapCollaborator.Map.Colaborador = colaborador;
          validMapCollaborator.Map.ColaboradorAnterior = colaborador;
          validMapCollaborator.Map.Schooling = schooling;
          validMapCollaborator.Map.Company = company;
          validMapCollaborator.Map.Establishment = establishment;
          validMapCollaborator.Map.Occupation = occupation;
          validMapCollaborator.Map.CompanyManager = companyManager;
          validMapCollaborator.Map.Manager = manager;
          // Verificar se a pessoa já existe
          ViewIntegrationMapPersonV1 view = personIntegration.GetCollaboratorByKey(new ViewIntegrationMapPersonV1()
          {
            Document = colaborador.Documento,
            IdCompany = company.Id,
            Registration = colaborador.Matricula,
            Name = colaborador.Nome,
            Id = string.Empty,
            Person = null,
            Message = string.Empty
          });
          if (view.Person != null)
            validMapCollaborator.Map.Person = view.Person;
          Collaborators.Add(validMapCollaborator.Map);
        }
        else
        {
          Collaborators[search].Schooling = schooling;
          Collaborators[search].Company = company;
          Collaborators[search].Establishment = establishment;
          Collaborators[search].Occupation = occupation;
          Collaborators[search].Colaborador = colaborador;
          Collaborators[search].CompanyManager = companyManager;
          Collaborators[search].Manager = manager;
          if (Collaborators[search].Person == null)
          {
            // Verificar se a pessoa já existe
            ViewIntegrationMapPersonV1 view = personIntegration.GetCollaboratorByKey(new ViewIntegrationMapPersonV1()
            {
              Document = colaborador.Documento,
              IdCompany = company.Id,
              Registration = colaborador.Matricula,
              Name = colaborador.Nome,
              Id = string.Empty,
              Person = null,
              Message = string.Empty
            });
            if (view.Person != null)
              Collaborators[search].Person = view.Person;
          }
        }
      }

      string saveObject = JsonConvert.SerializeObject(Schoolings);
      if (!Directory.Exists(string.Format("{0}/integration", Person.IdAccount)))
        Directory.CreateDirectory(string.Format("{0}/integration", Person.IdAccount));
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/grau.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Companys);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/empresa.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Establishments);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/estabelecimentos.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Occupations);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/cargo.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Collaborators);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Managers);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/gestor.txt", Person.IdAccount), saveObject, false);

      Message = string.Empty;
      foreach (ViewIntegrationMapOfV1 item in Schoolings)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de grau de instrução!",Message,Environment.NewLine);
      foreach (ViewIntegrationMapOfV1 item in Companys)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de empresa!", Message, Environment.NewLine);
      foreach (ViewIntegrationMapOfV1 item in Establishments)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de estabelecimento!", Message, Environment.NewLine);
      foreach (ViewIntegrationMapOfV1 item in Occupations)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de cargo!", Message, Environment.NewLine);

      if (string.IsNullOrEmpty(Message))
        UpdatePerson();
    }
    private ViewIntegrationMapOfV1 VerifyMapOf(List<ViewIntegrationMapOfV1> lista, EnumValidKey key, string code, string name)
    {
      // Validar Empresa
      ViewIntegrationMapOfV1 search = lista.Find(p => p.Name == name);
      ValidMapOf validMapOf = null;
      if (search == null)
      {
        validMapOf = new ValidMapOf(Person, key, code, name, string.Empty);
        lista.Add(validMapOf.Map);
      }
      else
      {
        if (string.IsNullOrEmpty(search.Id))
        {
          validMapOf = new ValidMapOf(Person, key, code, name, string.Empty);
          lista.Find(p => p.Name == name).Id = validMapOf.Map.Id;
          lista.Find(p => p.Name == name).Message = string.Empty;
        }
        else
          return search;
      }
      return validMapOf.Map;
    }
    private ViewIntegrationMapOfV1 VerifyMapOf(List<ViewIntegrationMapOfV1> lista, EnumValidKey key, string code, string name, string idCompany)
    {
      // Validar Empresa
      ViewIntegrationMapOfV1 search = lista.Find(p => p.Name == name && p.IdCompany == idCompany);
      ValidMapOf validMapOf = null;
      if (search == null)
      {
        validMapOf = new ValidMapOf(Person, key, code, name, idCompany);
        lista.Add(validMapOf.Map);
      }
      else
      {
        if (string.IsNullOrEmpty(search.Id))
        {
          validMapOf = new ValidMapOf(Person, key, code, name, idCompany);
          lista.Find(p => p.Name == name).Id = validMapOf.Map.Id;
        }
        else
          return search;
      }
      return validMapOf.Map;
    }
    private void UpdatePerson()
    {
      foreach (MapPerson collaborator in Collaborators)
      {
        // Novo Colaborador
        int situacao = 0;
        switch (collaborator.Colaborador.Situacao.ToLower())
        {
          case "férias":
            situacao = 3;
            break;
          case "afastado":
            situacao = 2;
            break;
          case "demitido":
            situacao = 2;
            break;
          default:
            situacao = 0;
            break;
        }
        int typeUser = 3;
        int searchManager = Managers.FindIndex(p => p.Document == collaborator.Documento && p.CompanyCode == collaborator.Empresa && p.Registration == collaborator.Matricula);
        if (searchManager != -1)
          typeUser = 2;

        if (collaborator.Person == null)
        {
          PersonIntegration personIntegration = new PersonIntegration(Person);
          ViewIntegrationPersonV1 newPerson = new ViewIntegrationPersonV1()
          {
            Name = collaborator.Colaborador.Nome,
            Mail = collaborator.Colaborador.Email,
            Document = collaborator.Colaborador.Documento,
            DateBirth = collaborator.Colaborador.DataNascimento,
            Phone = collaborator.Colaborador.Celular,
            PhoneFixed = collaborator.Colaborador.Telefone,
            DocumentID = collaborator.Colaborador.Identidade,
            DocumentCTPF = collaborator.Colaborador.CarteiraProfissional,
            Sex = collaborator.Colaborador.Sexo,
            Schooling = collaborator.Schooling,
            Contract = new ViewIntegrationContractV1()
            {
              Document = collaborator.Colaborador.Documento,
              Company  = collaborator.Company,
              Registration = collaborator.Colaborador.Matricula,
              Establishment = collaborator.Establishment,
              DateAdm = collaborator.Colaborador.DataAdmissao,
              StatusUser = situacao,
              HolidayReturn = collaborator.Colaborador.DataRetornoFerias,
              MotiveAside = collaborator.Colaborador.MotivoAfastamento,
              DateResignation =  collaborator.Colaborador.DataDemissao,
              Occupation = collaborator.Occupation,
              DateLastOccupation =  collaborator.Colaborador.DataUltimaTrocaCargo,
              Salary = collaborator.Colaborador.SalarioNominal,
              DateLastReadjust = collaborator.Colaborador.DataUltimoReajuste,
              TypeUser = typeUser,
              TypeJourney = 0,
              _IdManager = collaborator.Manager?.IdPerson,
              DocumentManager = collaborator.Colaborador.DocumentoChefe,
              CompanyManager = collaborator.CompanyManager,
              RegistrationManager = collaborator.Colaborador.MatriculaChefe,
              NameManager = collaborator.Colaborador.NomeChefe
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(newPerson);
          int search = Collaborators.FindIndex(p => p.Documento == collaborator.Colaborador.Documento && p.Empresa == collaborator.Colaborador.Empresa && p.Matricula == collaborator.Colaborador.Matricula);
          Collaborators[search].Person = viewReturn.Person;
        }
        // Forçar troca de gestor se não estiver marcado
        bool forceChange = false;
        if (searchManager != -1)
        {
          if (collaborator.Person.Contract.TypeUser != typeUser)
            forceChange = true;
        }
        if (!collaborator.Colaborador.TestarMudanca(collaborator.ColaboradorAnterior) || forceChange)
        {
          // Testar troca de gestor
          PersonIntegration personIntegration = new PersonIntegration(Person);
          ViewIntegrationPersonV1 changePerson = new ViewIntegrationPersonV1()
          {
            _id = collaborator.Person._id,
            Name = collaborator.Colaborador.Nome,
            Mail = collaborator.Colaborador.Email,
            Document = collaborator.Colaborador.Documento,
            DateBirth = collaborator.Colaborador.DataNascimento,
            Phone = collaborator.Colaborador.Celular,
            PhoneFixed = collaborator.Colaborador.Telefone,
            DocumentID = collaborator.Colaborador.Identidade,
            DocumentCTPF = collaborator.Colaborador.CarteiraProfissional,
            Sex = collaborator.Colaborador.Sexo,
            Schooling = collaborator.Schooling,
            Contract = new ViewIntegrationContractV1()
            {
              Document = collaborator.Colaborador.Documento,
              Company = collaborator.Company,
              Registration = collaborator.Colaborador.Matricula,
              Establishment = collaborator.Establishment,
              DateAdm = collaborator.Colaborador.DataAdmissao,
              StatusUser = situacao,
              HolidayReturn = collaborator.Colaborador.DataRetornoFerias,
              MotiveAside = collaborator.Colaborador.MotivoAfastamento,
              DateResignation = collaborator.Colaborador.DataDemissao,
              Occupation = collaborator.Occupation,
              DateLastOccupation = collaborator.Colaborador.DataUltimaTrocaCargo,
              Salary = collaborator.Colaborador.SalarioNominal,
              DateLastReadjust = collaborator.Colaborador.DataUltimoReajuste,
              TypeUser = typeUser,
              TypeJourney = 0,
              _IdManager = collaborator.Manager?.IdPerson,
              DocumentManager = collaborator.Colaborador.DocumentoChefe,
              CompanyManager = collaborator.CompanyManager,
              RegistrationManager = collaborator.Colaborador.MatriculaChefe,
              NameManager = collaborator.Colaborador.NomeChefe
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(changePerson);
          int search = Collaborators.FindIndex(p => p.Documento == collaborator.Colaborador.Documento && p.Empresa == collaborator.Colaborador.Empresa && p.Matricula == collaborator.Colaborador.Matricula);
          Collaborators[search].Person = viewReturn.Person;
          Collaborators[search].ColaboradorAnterior = Collaborators[search].Colaborador;
        }
      }
      string saveObject = JsonConvert.SerializeObject(Collaborators);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);
      Message = "Fim de integração!";
    }
    #endregion

  }
}
