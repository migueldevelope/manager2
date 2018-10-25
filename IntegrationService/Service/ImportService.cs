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
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IntegrationService.Service
{
  public class ImportService
  {
    public string Message { get; set; }
    public EnumStatusService Status { get; private set; }

    private List<Colaborador> Lista;
    private readonly ViewPersonLogin Person;
    private List<ViewIntegrationMapOfV1> Schoolings;
    private List<ViewIntegrationMapOfV1> Companys;
    private List<ViewIntegrationMapOfV1> Establishments;
    private List<ViewIntegrationMapOfV1> Occupations;
    private List<ViewIntegrationMapManagerV1> Managers;
    private List<MapPerson> Collaborators;
//    private ViewIntegrationParameter Param;
    private readonly string LogFileName;
    private readonly Version VersionProgram;
    public ImportService(ViewPersonLogin person)
    {
      try
      {
        Person = person;
        Message = string.Empty;
        Status = EnumStatusService.Ok;
        Lista = new List<Colaborador>();
        LogFileName = string.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        Assembly assem = Assembly.GetEntryAssembly();
        AssemblyName assemName = assem.GetName();
        VersionProgram = assemName.Version;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #region Preparação Banco de Dados V1
    public void DatabaseV1(ConfigurationService service)
    {
      try
      {
        if (service.Param.Type == EnumIntegrationType.Custom)
          throw new Exception("Rotina deve ser do tipo customizada!!!");

        //if (service.Param.Process != EnumIntegrationProcess.System)
        //  throw new Exception("Apenas processo de sistema pode utilizar banco de dados!!!");

        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.ConnectionString))
          throw new Exception("Sem string de conexão!!!");

        if (string.IsNullOrEmpty(service.Param.SqlCommand))
          throw new Exception("Sem comando de leitura do banco de dados!!!");

        // Ler banco de dados
        DataTable readData = ReadData(service);
        if (readData.Rows.Count == 0)
          throw new Exception("Não tem nenhum colaborador como retorno da consulta!!!");

        // Tratar os dados
        string columnsOk = string.Empty;
        switch (service.Param.Type)
        {
          case EnumIntegrationType.Basic:
            columnsOk = ValidColumnSystemBasicV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
            break;
          case EnumIntegrationType.Complete:
            columnsOk = ValidColumnSystemCompleteV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
            break;
        }
        if (!string.IsNullOrEmpty(columnsOk))
          throw new Exception(columnsOk);
        // Carregar Lista de Colaboradores
        Lista = new List<Colaborador>();
        Colaborador colaborador = null;
        foreach (DataRow row in readData.Rows)
        {
          List<string> teste = row.ItemArray.Select(x => x.ToString()).ToList();
          switch (service.Param.Type)
          {
            case EnumIntegrationType.Basic:
              colaborador = new Colaborador(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemBasicV1());
              break;
            case EnumIntegrationType.Complete:
              colaborador = new Colaborador(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemCompleteV1());
              break;
          }
          Lista.Add(ValidDataColaborator(colaborador));
        }
      }
      catch (Exception ex)
      {
        service.SetParameter(new ViewIntegrationParameterExecution()
        {
          CriticalError = ex.ToString(),
          MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME"),
          StatusExecution = "Error",
          CustomVersionExecution = string.Empty,
          UploadNextLog = false,
          ProgramVersionExecution = VersionProgram.ToString()
        });
        Status = EnumStatusService.Error;
        Message = ex.ToString();
      }
    }

    private DataTable ReadData(ConfigurationService service)
    {
      try
      {
        Boolean oracle = service.Param.ConnectionString.Split(';')[0].Equals("Oracle");
        string hostname = service.Param.ConnectionString.Split(';')[1];
        string user = service.Param.ConnectionString.Split(';')[2];
        string password = service.Param.ConnectionString.Split(';')[3];
        string baseDefault = service.Param.ConnectionString.Split(';')[4];
        ConnectionString conn;
        if (oracle)
          conn = new ConnectionString(hostname, user, password)
          {
            Sql = service.Param.SqlCommand
          };
        else
          conn = new ConnectionString(hostname, user, password, baseDefault)
          {
            Sql = service.Param.SqlCommand
          };
        GetPersonSystem gps = new GetPersonSystem(conn);
        return gps.GetPerson();
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Preparação Arquivo CSV V1
    public void FileCsvV1(ConfigurationService service)
    {
      try
      {
        if (service.Param.Process == EnumIntegrationProcess.Executable)
          throw new Exception("Processo deve ser do tipo executável!!!");

        if (service.Param.Type == EnumIntegrationType.Custom)
          throw new Exception("Rotina deve ser do tipo customizada!!!");

        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.FilePathLocal))
          throw new Exception("Sem arquivo de importação definido!!!");

        if (!File.Exists(service.Param.FilePathLocal))
          throw new Exception(string.Format("Arquivo {0} não encontrado!!!", service.Param.FilePathLocal));

        // Tratar os dados
        StreamReader rd = new StreamReader(service.Param.FilePathLocal, Encoding.Default, true);
        List<string> readLine = null;
        readLine = rd.ReadLine().Split(';').ToList();
        // Tratar os dados
        string columnsOk = string.Empty;
        switch (service.Param.Process)
        {
          case EnumIntegrationProcess.Manual:
            switch (service.Param.Type)
            {
              case EnumIntegrationType.Basic:
                columnsOk = ValidColumnManualBasicV1(readLine);
                break;
              case EnumIntegrationType.Complete:
                columnsOk = ValidColumnManualCompleteV1(readLine);
                break;
            }
            break;
          case EnumIntegrationProcess.System:
            switch (service.Param.Type)
            {
              case EnumIntegrationType.Basic:
                columnsOk = ValidColumnSystemBasicV1(readLine);
                break;
              case EnumIntegrationType.Complete:
                columnsOk = ValidColumnSystemCompleteV1(readLine);
                break;
            }
            break;
        }
        if (!string.IsNullOrEmpty(columnsOk))
          throw new Exception(columnsOk);

        // Carregar Lista de Colaboradores
        Lista = new List<Colaborador>();
        Colaborador colaborador = null;
        string register;
        while ((register = rd.ReadLine()) != null)
        {
          readLine = register.Split(';').ToList();
          switch (service.Param.Process)
          {
            case EnumIntegrationProcess.Manual:
              switch (service.Param.Type)
              {
                case EnumIntegrationType.Basic:
                  colaborador = new Colaborador(readLine, new EnumLayoutManualBasicV1());
                  break;
                case EnumIntegrationType.Complete:
                  colaborador = new Colaborador(readLine, new EnumLayoutManualCompleteV1());
                  break;
              }
              break;
            case EnumIntegrationProcess.System:
              switch (service.Param.Type)
              {
                case EnumIntegrationType.Basic:
                  colaborador = new Colaborador(readLine, new EnumLayoutSystemBasicV1());
                  break;
                case EnumIntegrationType.Complete:
                  colaborador = new Colaborador(readLine, new EnumLayoutSystemCompleteV1());
                  break;
              }
              break;
            default:
              break;
          }
          Lista.Add(ValidDataColaborator(colaborador));
        }
        rd.Close();
      }
      catch (Exception ex)
      {
        service.SetParameter(new ViewIntegrationParameterExecution()
        {
          CriticalError = ex.ToString(),
          MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME"),
          StatusExecution = "Error",
          CustomVersionExecution = string.Empty,
          UploadNextLog = false,
          ProgramVersionExecution = VersionProgram.ToString()
        });
        Status = EnumStatusService.Error;
        Message = ex.ToString();
      }
    }
    #endregion

    #region Preparação Arquivo Excel V1
    #endregion

    #region Adicionar colaborador customizado
    public void AddColaborador(Colaborador colaborador)
    {
      try
      {
        Lista.Add(ValidDataColaborator(colaborador));
      }
      catch (Exception ex)
      {
        Status = EnumStatusService.Error;
        Message = ex.ToString();
      }
    }
    #endregion

    #region Validação de Colunas
    private string ValidColumnManualBasicV1(List<string> columns)
    {
      try
      {
        if (columns.Count != Enum.GetNames(typeof(EnumLayoutManualBasicV1)).Length)
          return string.Format("Número de colunas inválida. Deveria ter {0} e tem {1}.", Enum.GetNames(typeof(EnumLayoutManualBasicV1)).Length,columns.Count);

        MemberInfo[] mebers = null;
        DescriptionAttribute[] attribs = null;

        string message = string.Empty;
        foreach (EnumLayoutManualBasicV1 item in Enum.GetValues(typeof(EnumLayoutManualBasicV1)))
        {
          mebers = item.GetType().GetMember(item.ToString());
          attribs = (mebers != null && mebers.Length > 0) ? (System.ComponentModel.DescriptionAttribute[])mebers[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) : null;
          if (!attribs[0].Description.Contains(columns[(int)item]))
            message = string.Format("{0}{1} Coluna {2} não encontrada na posição {3}", message, Environment.NewLine, attribs[0].Description, (int)item+1);
        }
        return message;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
    private string ValidColumnManualCompleteV1(List<string> columns)
    {
      try
      {
        if (columns.Count != Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).Length)
          return string.Format("Número de colunas inválida. Deveria ter {0} e tem {1}.", Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).Length, columns.Count);

        MemberInfo[] mia = null;
        DescriptionAttribute[] atribs = null;

        string message = string.Empty;
        foreach (EnumLayoutManualCompleteV1 item in Enum.GetValues(typeof(EnumLayoutManualCompleteV1)))
        {
          mia = item.GetType().GetMember(item.ToString());
          atribs = (mia != null && mia.Length > 0) ? (System.ComponentModel.DescriptionAttribute[])mia[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) : null;
          if (!atribs[0].Description.Contains(columns[(int)item]))
            message = string.Format("{0}{1} Coluna {2} não encontrada na posição {3}", message, Environment.NewLine, atribs[0].Description, (int)item + 1);
        }
        return message;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
    private string ValidColumnSystemBasicV1(List<string> columns)
    {
      try
      {
        if (columns.Count != Enum.GetNames(typeof(EnumLayoutSystemBasicV1)).Length)
          return string.Format("Número de colunas inválida. Deveria ter {0} e tem {1}.", Enum.GetNames(typeof(EnumLayoutSystemBasicV1)).Length, columns.Count);

        MemberInfo[] mia = null;
        DescriptionAttribute[] atribs = null;

        string message = string.Empty;
        foreach (EnumLayoutSystemBasicV1 item in Enum.GetValues(typeof(EnumLayoutSystemBasicV1)))
        {
          mia = item.GetType().GetMember(item.ToString());
          atribs = (mia != null && mia.Length > 0) ? (System.ComponentModel.DescriptionAttribute[])mia[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) : null;
          if (!atribs[0].Description.Contains(columns[(int)item]))
            message = string.Format("{0}{1} Coluna {2} não encontrada na posição {3}", message, Environment.NewLine, atribs[0].Description, (int)item + 1);
        }
        return message;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
    private string ValidColumnSystemCompleteV1(List<string> columns)
    {
      try
      {
        if (columns.Count != Enum.GetNames(typeof(EnumLayoutSystemCompleteV1)).Length)
          return string.Format("Número de colunas inválida. Deveria ter {0} e tem {1}.", Enum.GetNames(typeof(EnumLayoutSystemCompleteV1)).Length, columns.Count);

        MemberInfo[] mia = null;
        DescriptionAttribute[] atribs = null;

        string message = string.Empty;
        foreach (EnumLayoutSystemCompleteV1 item in Enum.GetValues(typeof(EnumLayoutSystemCompleteV1)))
        {
          mia = item.GetType().GetMember(item.ToString());
          atribs = (mia != null && mia.Length > 0) ? (System.ComponentModel.DescriptionAttribute[])mia[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) : null;
          if (!atribs[0].Description.Contains(columns[(int)item]))
            message = string.Format("{0}{1} Coluna {2} não encontrada na posição {3}", message, Environment.NewLine, atribs[0].Description, (int)item + 1);
        }
        return message;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
    #endregion

    #region Validação de dados do Colaborador
    private Colaborador ValidDataColaborator(Colaborador colaborador)
    {
      try
      {
        // Nome da Empresa
        if (string.IsNullOrEmpty(colaborador.NomeEmpresa))
          colaborador.Message = string.Concat(colaborador.Message, "Nome da empresa vazia!;");
        // Valid Cpf
        if (string.IsNullOrEmpty(colaborador.Documento))
          colaborador.Message = string.Concat(colaborador.Message, "CPF vazio!");
        else
          if (!IsValidCPF(colaborador.Documento))
            colaborador.Message = string.Concat(colaborador.Message, "CPF inválido!");
        // Matricula obrigatória
        if (colaborador.Matricula == 0)
          colaborador.Message = string.Concat(colaborador.Message, "Matricula não informada!;");
        // Nome
        if (string.IsNullOrEmpty(colaborador.Nome))
          colaborador.Message = string.Concat(colaborador.Message, "Nome não informado!");
        // E-mail
        if (string.IsNullOrEmpty(colaborador.Email))
          colaborador.Message = string.Concat(colaborador.Message, "E-mail não informado!");
        // Data admissão
        if (colaborador.DataAdmissao == null)
          colaborador.Message = string.Concat(colaborador.Message, "Data de admissão inválida!");
        // Situação
        if (!colaborador.Situacao.Equals("Ativo", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Férias", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Ferias", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Afastado", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Demitido", StringComparison.InvariantCultureIgnoreCase))
          colaborador.Message = string.Concat(colaborador.Message, "Situação diferente de Ativo/Férias/Afastado/Demitido!");
        if (colaborador.Situacao.Equals("Demitido") && colaborador.DataDemissao == null)
          colaborador.Message = string.Concat(colaborador.Message, "Data de demissão deve ser informada!;");
        // Nome do Cargo
        if (string.IsNullOrEmpty(colaborador.NomeCargo))
          colaborador.Message = string.Concat(colaborador.Message, "Nome do cargo vazio!;");
        // Grau de Instrução, Nome do Grau de Instrução
        if (string.IsNullOrEmpty(colaborador.NomeGrauInstrucao))
          colaborador.Message = string.Concat(colaborador.Message, "Nome do grau de instrução vazio!;");
        // Valid Cpf chefe
        if (!string.IsNullOrEmpty(colaborador.DocumentoChefe))
          if (!IsValidCPF(colaborador.DocumentoChefe))
            colaborador.Message = string.Concat(colaborador.Message, "Documento do gestor é inválido!");
        return colaborador;
      }
      catch (Exception)
      {
        throw;
      }
    }
    private bool IsValidCPF(string cpf)
    {
      int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      string tempCpf;
      string digito;
      int soma;
      int resto;
      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");
      if (cpf.Length != 11)
        return false;
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

    #region Importação Final Completa
    public void FinalImport(ConfigurationService service)
    {
      try
      {
        if (Status == EnumStatusService.Error)
          return;
        if (Lista.Count == 0)
          throw new Exception("Lista de colaboradores vazia!");
        LoadLists();
        ImportExecute();
        service.SetParameter(new ViewIntegrationParameterExecution()
        {
          CriticalError = string.Empty,
          MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME"),
          StatusExecution = "Ok",
          CustomVersionExecution = string.Empty,
          UploadNextLog = false,
          ProgramVersionExecution = VersionProgram.ToString()
        });
      }
      catch (Exception ex)
      {
        service.SetParameter(new ViewIntegrationParameterExecution()
        {
          CriticalError = ex.ToString(),
          MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME"),
          StatusExecution = "Error",
          CustomVersionExecution = string.Empty,
          UploadNextLog = false,
          ProgramVersionExecution = VersionProgram.ToString()
        });
        Status = EnumStatusService.Error;
        Message = ex.ToString();
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
    private void ImportExecute()
    {
      try
      {
        ViewIntegrationMapOfV1 establishment = null;
        foreach (var colaborador in Lista)
        {
          PersonIntegration personIntegration = new PersonIntegration(Person);
          // Validar Grau Instrução, Empresa, Estabelecimento, Cargo
          ViewIntegrationMapOfV1 schooling = VerifyMapOf(Schoolings, EnumValidKey.Schooling, colaborador.GrauInstrucao, colaborador.NomeGrauInstrucao);
          ViewIntegrationMapOfV1 company = VerifyMapOf(Companys, EnumValidKey.Company, colaborador.Empresa, colaborador.NomeEmpresa);
          if (!string.IsNullOrEmpty(colaborador.NomeEstabelecimento))
            establishment = VerifyMapOf(Establishments, EnumValidKey.Establishment, colaborador.Estabelecimento, colaborador.NomeEstabelecimento, company.Id);
          ViewIntegrationMapOfV1 occupation = VerifyMapOf(Occupations, EnumValidKey.Occupation, colaborador.Cargo, colaborador.NomeCargo, company.Id);
          ViewIntegrationMapOfV1 companyManager = null;
          ViewIntegrationMapManagerV1 manager = null;
          // Validar chefe (manager)
          if (colaborador.DocumentoChefe != null && colaborador.NomeEmpresaChefe != null && colaborador.MatriculaChefe != 0)
          {
            companyManager = VerifyMapOf(Companys, EnumValidKey.Company, string.Empty, colaborador.NomeEmpresaChefe);
            ViewIntegrationMapManagerV1 viewManager = new ViewIntegrationMapManagerV1()
            {
              Document = colaborador.DocumentoChefe,
              CompanyName = companyManager.Name,
              Registration = colaborador.MatriculaChefe,
              IdContract = null,
              IdPerson = null,
              Name = string.Empty,
              Message = string.Empty
            };
            int searchManager = Managers.FindIndex(p => p.Document == colaborador.DocumentoChefe && p.CompanyName == colaborador.NomeEmpresaChefe && p.Registration == colaborador.MatriculaChefe);
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
                  CompanyName = companyManager.Name,
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
          int search = Collaborators.FindIndex(p => p.Documento == colaborador.Documento && p.NomeEmpresa == colaborador.NomeEmpresa && p.Matricula == colaborador.Matricula);
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
            Message = string.Format("{0}{1}Grau de instrução {2}", Message, Environment.NewLine, item.Name);
        foreach (ViewIntegrationMapOfV1 item in Companys)
          if (string.IsNullOrEmpty(item.Id))
            Message = string.Format("{0}{1}Empresa {2}", Message, Environment.NewLine, item.Name);
        foreach (ViewIntegrationMapOfV1 item in Establishments)
          if (string.IsNullOrEmpty(item.Id))
            Message = string.Format("{0}{1}Estabelecimento {2}", Message, Environment.NewLine, item.Name);
        foreach (ViewIntegrationMapOfV1 item in Occupations)
          if (string.IsNullOrEmpty(item.Id))
            Message = string.Format("{0}{1}Cargo {2}", Message, Environment.NewLine, item.Name);

        if (!string.IsNullOrEmpty(Message))
          throw new Exception(Message);

        UpdatePerson();
      }
      catch (Exception)
      {
        throw;
      }
    }
    private ViewIntegrationMapOfV1 VerifyMapOf(List<ViewIntegrationMapOfV1> lista, EnumValidKey key, string code, string name)
    {
      // Validar Empresa
      ViewIntegrationMapOfV1 search;
      if (!string.IsNullOrEmpty(code))
        search = lista.Find(p => p.Code == code);
      else
        search = lista.Find(p => p.Name == name);

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
      ViewIntegrationMapOfV1 search;
      if (!string.IsNullOrEmpty(code))
        search = lista.Find(p => p.Code == code && p.IdCompany == idCompany);
      else
        search = lista.Find(p => p.Name == name && p.IdCompany == idCompany);

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
        EnumStatusUser situacao = 0;
        switch (collaborator.Colaborador.Situacao.ToLower())
        {
          case "férias":
            situacao = EnumStatusUser.Vacation;
            break;
          case "afastado":
            situacao = EnumStatusUser.Away;
            break;
          case "demitido":
            situacao = EnumStatusUser.Disabled;
            break;
          default:
            situacao = 0;
            break;
        }
        EnumTypeUser typeUser = EnumTypeUser.Employee;
        int searchManager = Managers.FindIndex(p => p.Document == collaborator.Documento && p.CompanyName == collaborator.NomeEmpresa && p.Registration == collaborator.Matricula);
        if (searchManager != -1)
          typeUser = EnumTypeUser.Manager;

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
              RegistrationManager = collaborator.Colaborador.MatriculaChefe
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(newPerson);
          int search = Collaborators.FindIndex(p => p.Documento == collaborator.Colaborador.Documento && p.NomeEmpresa == collaborator.Colaborador.NomeEmpresa && p.Matricula == collaborator.Colaborador.Matricula);
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
              RegistrationManager = collaborator.Colaborador.MatriculaChefe
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(changePerson);
          int search = Collaborators.FindIndex(p => p.Documento == collaborator.Colaborador.Documento && p.NomeEmpresa == collaborator.Colaborador.NomeEmpresa && p.Matricula == collaborator.Colaborador.Matricula);
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
