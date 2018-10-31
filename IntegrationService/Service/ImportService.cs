using IntegrationService.Api;
using IntegrationService.Core;
using IntegrationService.Data;
using IntegrationService.Enumns;
using IntegrationService.Tools;
using Manager.Views.Enumns;
using Manager.Views.Integration;
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

    private List<Colaborador> Colaboradores;
    private List<ControleColaborador> ControleColaboradores;
    private readonly ViewPersonLogin Person;
    private List<ViewIntegrationMapOfV1> Schoolings;
    private List<ViewIntegrationMapOfV1> Companys;
    private List<ViewIntegrationMapOfV1> Establishments;
    private List<ViewIntegrationMapOfV1> Occupations;
    private List<ViewIntegrationMapManagerV1> Managers;
    //    private ViewIntegrationParameter Param;
    private ConfigurationService service;
    private readonly string LogFileName;
    private readonly Version VersionProgram;
    public ImportService(ViewPersonLogin person, ConfigurationService serviceConfiguration)
    {
      try
      {
        Person = person;
        service = serviceConfiguration;
        Message = string.Empty;
        Status = EnumStatusService.Ok;
        Colaboradores = new List<Colaborador>();
        if (!Directory.Exists(string.Format("{0}/integration", Person.IdAccount)))
          Directory.CreateDirectory(string.Format("{0}/integration", Person.IdAccount));
        // Limpeza de arquivos LOG

        DirectoryInfo diretorio = new DirectoryInfo(string.Format("{0}/integration", Person.IdAccount));
        List<FileInfo> Arquivos = diretorio.GetFiles("*.log").Where(p => p.CreationTime.Date < DateTime.Now.Date).ToList();
        //Comea a listar o(s) arquivo(s)
        foreach (FileInfo fileinfo in Arquivos)
          File.Delete(fileinfo.FullName);

        LogFileName = string.Format("{0}/integration/{1}.log", Person.IdAccount, DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        Assembly assem = Assembly.GetEntryAssembly();
        AssemblyName assemName = assem.GetName();
        VersionProgram = assemName.Version;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void Execute()
    {
      try
      {
        switch (service.Param.Process)
        {
          case EnumIntegrationProcess.Manual:
            switch (service.Param.Mode)
            {
              case EnumIntegrationMode.DataBaseV1:
                Message = "Modo banco de dados não suportado no processo manual!";
                throw new Exception(Message);
              case EnumIntegrationMode.FileCsvV1:
                FileCsvV1();
                break;
              case EnumIntegrationMode.FileExcelV1:
                break;
              case EnumIntegrationMode.Custom:
                Message = "Modo customizado não suportado no processo manual!";
                throw new Exception(Message);
            }
            break;
          case EnumIntegrationProcess.System:
            switch (service.Param.Mode)
            {
              case EnumIntegrationMode.DataBaseV1:
                DatabaseV1();
                break;
              case EnumIntegrationMode.FileCsvV1:
                FileCsvV1();
                break;
              case EnumIntegrationMode.FileExcelV1:
                break;
              case EnumIntegrationMode.Custom:
                Message = "Modo customizado não suportado no processo manual!";
                throw new Exception(Message);
            }
            break;
          case EnumIntegrationProcess.Executable:
            break;
          default:
            break;
        }
        if (Status == EnumStatusService.CriticalError)
          throw new Exception(Message);

        if (Colaboradores.Count == 0)
        {
          Message = "Lista de colaboradores vazia!";
          throw new Exception(Message);
        }
        LoadLists();
        FinalImport();
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
        if (string.IsNullOrEmpty(Message))
          Message = ex.ToString();
        service.SetParameter(new ViewIntegrationParameterExecution()
        {
          CriticalError = Message,
          MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME"),
          StatusExecution = "Critical Error",
          CustomVersionExecution = string.Empty,
          UploadNextLog = false,
          ProgramVersionExecution = VersionProgram.ToString()
        });
        throw;
      }
    }

    #region Preparação Banco de Dados V1
    private void DatabaseV1()
    {
      try
      {
        if (service.Param.Type == EnumIntegrationType.Custom)
        {
          Message = "Rotina deve ser do tipo customizada!!!";
          throw new Exception(Message);
        }

        //if (service.Param.Process != EnumIntegrationProcess.System)
        //  throw new Exception("Apenas processo de sistema pode utilizar banco de dados!!!");

        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.ConnectionString))
        {
          Message = "Sem string de conexão!!!";
          throw new Exception(Message);
        }

        if (string.IsNullOrEmpty(service.Param.SqlCommand))
        {
          Message = "Sem comando de leitura do banco de dados!!!";
          throw new Exception(Message);
        }

        // Ler banco de dados
        DataTable readData = ReadData();
        if (readData.Rows.Count == 0)
        {
          Message = "Não tem nenhum colaborador como retorno da consulta!!!";
          throw new Exception(Message);
        }

        // Tratar os dados
        switch (service.Param.Type)
        {
          case EnumIntegrationType.Basic:
            Message = ValidColumnSystemBasicV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
            break;
          case EnumIntegrationType.Complete:
            Message = ValidColumnSystemCompleteV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
            break;
        }
        if (!string.IsNullOrEmpty(Message))
        {
          throw new Exception(Message);
        }
        // Carregar Lista de Colaboradores
        Colaboradores = new List<Colaborador>();
        foreach (DataRow row in readData.Rows)
        {
          List<string> teste = row.ItemArray.Select(x => x.ToString()).ToList();
          switch (service.Param.Type)
          {
            case EnumIntegrationType.Basic:
              Colaboradores.Add(ValidDataColaborator(new Colaborador(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemBasicV1())));
              break;
            case EnumIntegrationType.Complete:
              Colaboradores.Add(ValidDataColaborator(new Colaborador(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemCompleteV1())));
              break;
          }
        }
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    private DataTable ReadData()
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
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    #endregion

    #region Preparação Arquivo CSV V1
    private void FileCsvV1()
    {
      try
      {
        if (service.Param.Process == EnumIntegrationProcess.Executable)
        {
          Message = "Processo deve ser do tipo executável!!!";
          throw new Exception(Message);
        }

        if (service.Param.Type == EnumIntegrationType.Custom)
        {
          Message = "Rotina deve ser do tipo customizada!!!";
          throw new Exception(Message);
        }

        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.FilePathLocal))
        {
          Message = "Sem arquivo de importação definido!!!";
          throw new Exception(Message);
        }

        if (!File.Exists(service.Param.FilePathLocal))
        {
          Message = string.Format("Arquivo {0} não encontrado!!!", service.Param.FilePathLocal);
          throw new Exception(Message);
        }

        // Tratar os dados
        StreamReader rd = new StreamReader(service.Param.FilePathLocal, Encoding.Default, true);
        List<string> readLine = null;
        readLine = rd.ReadLine().Split(';').ToList();
        // Tratar os dados
        switch (service.Param.Process)
        {
          case EnumIntegrationProcess.Manual:
            switch (service.Param.Type)
            {
              case EnumIntegrationType.Basic:
                Message = ValidColumnManualBasicV1(readLine);
                break;
              case EnumIntegrationType.Complete:
                Message = ValidColumnManualCompleteV1(readLine);
                break;
            }
            break;
          case EnumIntegrationProcess.System:
            switch (service.Param.Type)
            {
              case EnumIntegrationType.Basic:
                Message = ValidColumnSystemBasicV1(readLine);
                break;
              case EnumIntegrationType.Complete:
                Message = ValidColumnSystemCompleteV1(readLine);
                break;
            }
            break;
        }
        if (!string.IsNullOrEmpty(Message))
          throw new Exception(Message);

        // Carregar Lista de Colaboradores
        Colaboradores = new List<Colaborador>();
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
                  if (readLine.Count == Enum.GetNames(typeof(EnumLayoutManualBasicV1)).Length)
                    Colaboradores.Add(ValidDataColaborator(new Colaborador(readLine, new EnumLayoutManualBasicV1())));
                  break;
                case EnumIntegrationType.Complete:
                  if (readLine.Count == Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).Length)
                    Colaboradores.Add(ValidDataColaborator(new Colaborador(readLine, new EnumLayoutManualCompleteV1())));
                  break;
              }
              break;
            case EnumIntegrationProcess.System:
              switch (service.Param.Type)
              {
                case EnumIntegrationType.Basic:
                  Colaboradores.Add(ValidDataColaborator(new Colaborador(readLine, new EnumLayoutSystemBasicV1())));
                  break;
                case EnumIntegrationType.Complete:
                  Colaboradores.Add(ValidDataColaborator(new Colaborador(readLine, new EnumLayoutSystemCompleteV1())));
                  break;
              }
              break;
            default:
              break;
          }
        }
        rd.Close();
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    #endregion

    #region Preparação Arquivo Excel V1
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
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Nome da empresa vazia!;");
        // Valid Cpf
        if (string.IsNullOrEmpty(colaborador.Documento))
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "CPF vazio!");
        else
          if (!IsValidCPF(colaborador.Documento))
            colaborador.Mensagem = string.Concat(colaborador.Mensagem, "CPF inválido!");
        // Matricula obrigatória
        if (colaborador.Matricula == 0)
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Matricula não informada!;");
        // Nome
        if (string.IsNullOrEmpty(colaborador.Nome))
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Nome não informado!");
        // E-mail
        if (string.IsNullOrEmpty(colaborador.Email))
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "E-mail não informado!");
        // Data admissão
        if (colaborador.DataAdmissao == null)
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Data de admissão inválida!");
        // Situação
        if (!colaborador.Situacao.Equals("Ativo", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Férias", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Ferias", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Afastado", StringComparison.InvariantCultureIgnoreCase) &&
            !colaborador.Situacao.Equals("Demitido", StringComparison.InvariantCultureIgnoreCase))
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Situação diferente de Ativo/Férias/Afastado/Demitido!");
        if (colaborador.Situacao.Equals("Demitido") && colaborador.DataDemissao == null)
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Data de demissão deve ser informada!;");
        // Nome do Cargo
        if (string.IsNullOrEmpty(colaborador.NomeCargo))
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Nome do cargo vazio!;");
        // Grau de Instrução, Nome do Grau de Instrução
        if (string.IsNullOrEmpty(colaborador.NomeGrauInstrucao))
          colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Nome do grau de instrução vazio!;");
        // Valid Cpf chefe
        if (!string.IsNullOrEmpty(colaborador.DocumentoGestor))
          if (!IsValidCPF(colaborador.DocumentoGestor))
            colaborador.Mensagem = string.Concat(colaborador.Mensagem, "Documento do gestor é inválido!");
        return colaborador;
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

    #region Importação Final Completa

    #region Preparação e Validação
    private void LoadLists()
    {
      try
      {
        Companys = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/empresa.txt", Person.IdAccount)))
          Companys = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/empresa.txt", Person.IdAccount)));
        Establishments = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount)))
          Establishments = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount)));
        Occupations = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/cargo.txt", Person.IdAccount)))
          Occupations = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/cargo.txt", Person.IdAccount)));
        Schoolings = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/grau.txt", Person.IdAccount)))
          Schoolings = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/grau.txt", Person.IdAccount)));
        Managers = new List<ViewIntegrationMapManagerV1>();
        if (File.Exists(string.Format("{0}/integration/gestor.txt", Person.IdAccount)))
          Managers = JsonConvert.DeserializeObject<List<ViewIntegrationMapManagerV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/gestor.txt", Person.IdAccount)));
        ControleColaboradores = new List<ControleColaborador>();
        if (File.Exists(string.Format("{0}/integration/colaborador.txt", Person.IdAccount)))
          ControleColaboradores = JsonConvert.DeserializeObject<List<ControleColaborador>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount)));
      }
      catch (Exception)
      {
        throw;
      }
    }
    private void SaveLists()
    {
      try
      {
        string saveObject = JsonConvert.SerializeObject(Companys);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/empresa.txt", Person.IdAccount), saveObject, false);
        saveObject = JsonConvert.SerializeObject(Establishments);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount), saveObject, false);
        saveObject = JsonConvert.SerializeObject(Occupations);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/cargo.txt", Person.IdAccount), saveObject, false);
        saveObject = JsonConvert.SerializeObject(Schoolings);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/grau.txt", Person.IdAccount), saveObject, false);
        saveObject = JsonConvert.SerializeObject(Managers);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/gestor.txt", Person.IdAccount), saveObject, false);
        saveObject = JsonConvert.SerializeObject(ControleColaboradores);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);
      }
      catch (Exception)
      {
        throw;
      }
    }
    private ViewIntegrationMapOfV1 VerifyMapOf(List<ViewIntegrationMapOfV1> listMap, EnumValidKey key, string keySearch, string name, string idCompany)
    {
      if (string.IsNullOrEmpty(keySearch.Replace(";", string.Empty)))
        return null;
      // Validar Empresa
      ViewIntegrationMapOfV1 search = listMap.Find(p => p.Key == keySearch);
      if (search == null || string.IsNullOrEmpty(search.Id))
      {
        ViewIntegrationMapOfV1 searchNew = new ViewIntegrationMapOfV1()
        {
          Key = keySearch,
          Name = name,
          IdCompany = idCompany,
          Id = string.Empty,
          Message = string.Empty
        };
        PersonIntegration personIntegration = new PersonIntegration(Person);
        searchNew = personIntegration.GetByKey(key, searchNew);
        if (search == null)
          listMap.Add(searchNew);
        else
          listMap.Find(p => p.Key == keySearch).Id = searchNew.Id;
        return searchNew;
      }
      return search;
    }
    private ViewIntegrationMapPersonV1 VerifyMapPerson(string idCompany, string document, long registration)
    {
      // Validar Empresa
      ViewIntegrationMapPersonV1 search = new ViewIntegrationMapPersonV1()
      {
        IdCompany = idCompany,
        Document = document,
        Registration = registration,
        IdPerson = string.Empty,
        IdContract = string.Empty,
        Message = string.Empty
      };
      PersonIntegration personIntegration = new PersonIntegration(Person);
      return personIntegration.GetPersonByKey(search);
    }
    private ViewIntegrationMapManagerV1 VerifyMapManager(List<ViewIntegrationMapManagerV1> listMap, ViewIntegrationMapOfV1 company, string keySearch, string document, long registration)
    {
      if (company == null)
          return null;
      if (string.IsNullOrEmpty(keySearch.Replace(";", string.Empty).Replace("0",string.Empty)))
        return null;
      // Validar Empresa
      ViewIntegrationMapManagerV1 search = listMap.Find(p => p.Key == keySearch);
      if (search == null || string.IsNullOrEmpty(search.IdPerson))
      {
        ViewIntegrationMapManagerV1 searchNew = new ViewIntegrationMapManagerV1()
        {
          Key = keySearch,
          IdCompany = company.Id,
          Document = document,
          Registration = registration,
          IdPerson = string.Empty,
          IdContract = string.Empty,
          Message = string.Empty
        };
        PersonIntegration personIntegration = new PersonIntegration(Person);
        searchNew = personIntegration.GetManagerByKey(searchNew);
        if (search == null)
          listMap.Add(searchNew);
        else
          listMap.Find(p => p.Key == keySearch).IdPerson = searchNew.IdPerson;
        return searchNew;
      }
      return search;
    }
    #endregion

    private void FinalImport()
    {
      try
      {
        PersonIntegration personIntegration = new PersonIntegration(Person);
        foreach (var colaborador in Colaboradores.Where(p => string.IsNullOrEmpty(p.Mensagem)))
        {
          int search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
          if (search == -1)
          {
            ControleColaboradores.Add(new ControleColaborador()
            {
              ChaveColaborador = colaborador.ChaveColaborador,
              Colaborador = null,
              Company = null,
              CompanyManager = null,
              Establishment = null,
              Manager = null,
              Occupation = null,
              Schooling = null,
              IdPerson = string.Empty,
              IdContract = string.Empty
            });
            search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
          }
          // Validar Empresa
          if (ControleColaboradores[search].Company == null || string.IsNullOrEmpty(ControleColaboradores[search].Company.Id) || !ControleColaboradores[search].Company.Key.Equals(colaborador.ChaveEmpresa))
            ControleColaboradores[search].Company = VerifyMapOf(Companys, EnumValidKey.Company, colaborador.ChaveEmpresa, colaborador.NomeEmpresa, string.Empty);
          // Validar Estabelecimento
          if (!string.IsNullOrEmpty(colaborador.NomeEstabelecimento))
            if (ControleColaboradores[search].Establishment == null || string.IsNullOrEmpty(ControleColaboradores[search].Establishment.Id) || !ControleColaboradores[search].Establishment.Key.Equals(colaborador.ChaveEstabelecimento))
              ControleColaboradores[search].Establishment = VerifyMapOf(Establishments, EnumValidKey.Establishment, colaborador.ChaveEstabelecimento, colaborador.NomeEstabelecimento, ControleColaboradores[search].Company.Id);
          // Validar Grau Instrução
          if (ControleColaboradores[search].Schooling == null || string.IsNullOrEmpty(ControleColaboradores[search].Schooling.Id) || !ControleColaboradores[search].Schooling.Key.Equals(colaborador.ChaveGrauInstrucao))
            ControleColaboradores[search].Schooling = VerifyMapOf(Schoolings, EnumValidKey.Schooling, colaborador.ChaveGrauInstrucao, colaborador.NomeGrauInstrucao, string.Empty);
          // Validar Cargo
          if (ControleColaboradores[search].Occupation == null || string.IsNullOrEmpty(ControleColaboradores[search].Occupation.Id) || !ControleColaboradores[search].Occupation.Key.Equals(colaborador.ChaveCargo))
            ControleColaboradores[search].Occupation = VerifyMapOf(Occupations, EnumValidKey.Occupation, colaborador.ChaveCargo, colaborador.NomeCargo, ControleColaboradores[search].Company.Id);
          // Validar Empresa Gestor
          if (ControleColaboradores[search].CompanyManager == null || string.IsNullOrEmpty(ControleColaboradores[search].CompanyManager.Id) || !ControleColaboradores[search].CompanyManager.Key.Equals(colaborador.ChaveEmpresaGestor))
            ControleColaboradores[search].CompanyManager = VerifyMapOf(Companys, EnumValidKey.Company, colaborador.ChaveEmpresaGestor, colaborador.NomeEmpresaGestor, string.Empty);
          // Validar Gestor
          if (ControleColaboradores[search].Manager == null || string.IsNullOrEmpty(ControleColaboradores[search].Manager.IdPerson) || !ControleColaboradores[search].Manager.Key.Equals(colaborador.ChaveGestor))
            ControleColaboradores[search].Manager = VerifyMapManager(Managers, ControleColaboradores[search].CompanyManager, colaborador.ChaveGestor, colaborador.DocumentoGestor, colaborador.MatriculaGestor);

          // Validar Colaborador (Pessoa)
          if (string.IsNullOrEmpty(ControleColaboradores[search].IdPerson))
          {
            // Verificar se a pessoa já existe
            ViewIntegrationMapPersonV1 view = personIntegration.GetPersonByKey(new ViewIntegrationMapPersonV1()
            {
              IdCompany = ControleColaboradores[search].Company.Id,
              Document = colaborador.Documento,
              Registration = colaborador.Matricula,
              IdPerson = string.Empty,
              IdContract = string.Empty,
              Message = string.Empty
            });
            ControleColaboradores[search].IdPerson = view.IdPerson;
            ControleColaboradores[search].IdContract = view.IdContract;
          }
        }

        SaveLists();

        Message = string.Empty;
        foreach (ViewIntegrationMapOfV1 item in Companys)
          if (string.IsNullOrEmpty(item.Id))
          {
            FileClass.SaveLog(LogFileName, string.Format("Falta Integração da Empresa {0}", item.Key), EnumTypeLog.Error);
            Message = "Log de integração!!!";
          }
        foreach (ViewIntegrationMapOfV1 item in Establishments)
          if (string.IsNullOrEmpty(item.Id))
          {
            FileClass.SaveLog(LogFileName, string.Format("Falta Integração de Estabelecimentos {0}", item.Key), EnumTypeLog.Error);
            Message = "Log de integração!!!";
          }
        foreach (ViewIntegrationMapOfV1 item in Occupations)
          if (string.IsNullOrEmpty(item.Id))
          {
            FileClass.SaveLog(LogFileName, string.Format("Falta Integração de Cargo {0}", item.Key), EnumTypeLog.Error);
            Message = "Log de integração!!!";
          }
        foreach (ViewIntegrationMapOfV1 item in Schoolings)
          if (string.IsNullOrEmpty(item.Id))
          {
            FileClass.SaveLog(LogFileName, string.Format("Falta Integração de Grau de Instrução {0}", item.Key), EnumTypeLog.Error);
            Message = "Log de integração!!!";
          }

        if (!string.IsNullOrEmpty(Message))
          throw new Exception(Message);

        UpdatePerson();
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }

    #region Atualizar Pessoas
    private void UpdatePerson()
    {
      bool hasLogFile = false;
      foreach (var colaborador in Colaboradores.Where(p => !string.IsNullOrEmpty(p.Mensagem)))
      {
        // Gravar LOG de colaboradores com mensagem
        FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, colaborador.Mensagem), EnumTypeLog.Warning);
        Status = EnumStatusService.Error;
        hasLogFile = true;
      }

      int search = -1;
      int searchManager = -1;
      foreach (var colaborador in Colaboradores.Where(p => string.IsNullOrEmpty(p.Mensagem)))
      {
        search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
        EnumStatusUser situacao = EnumStatusUser.Enabled;
        switch (colaborador.Situacao.ToLower())
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
        searchManager = Managers.FindIndex(p => p.Key == colaborador.ChaveColaborador);
        if (searchManager != -1)
          typeUser = EnumTypeUser.Manager;

        if (ControleColaboradores[search].Colaborador != null)
        {
          ControleColaboradores[search].Colaborador.TypeUser = typeUser;
          colaborador.TypeUser = typeUser;
        }

        // Testar diferença de colaboradores
        if (string.IsNullOrEmpty(ControleColaboradores[search].IdPerson))
        {
          // Novo Colaborador
          PersonIntegration personIntegration = new PersonIntegration(Person);
          ViewIntegrationPersonV1 newPerson = new ViewIntegrationPersonV1()
          {
            Name = colaborador.Nome,
            Mail = colaborador.Email,
            Document = colaborador.Documento,
            DateBirth = colaborador.DataNascimento,
            Phone = colaborador.Celular,
            PhoneFixed = colaborador.Telefone,
            DocumentID = colaborador.Identidade,
            DocumentCTPF = colaborador.CarteiraProfissional,
            Sex = colaborador.Sexo.StartsWith("M") ? EnumSex.Male : colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others,
            Schooling = ControleColaboradores[search].Schooling,
            Contract = new ViewIntegrationContractV1()
            {
              Document = colaborador.Documento,
              Company = ControleColaboradores[search].Company,
              Registration = colaborador.Matricula,
              Establishment = ControleColaboradores[search].Establishment,
              DateAdm = colaborador.DataAdmissao,
              StatusUser = situacao,
              HolidayReturn = colaborador.DataRetornoFerias,
              MotiveAside = colaborador.MotivoAfastamento,
              DateResignation = colaborador.DataDemissao,
              Occupation = ControleColaboradores[search].Occupation,
              DateLastOccupation = colaborador.DataUltimaTrocaCargo,
              Salary = colaborador.SalarioNominal,
              DateLastReadjust = colaborador.DataUltimoReajuste,
              TypeUser = typeUser,
              TypeJourney = EnumTypeJourney.OnBoarding,
              _IdManager = ControleColaboradores[search].Manager?.IdPerson,
              DocumentManager = colaborador.DocumentoGestor,
              CompanyManager = ControleColaboradores[search].CompanyManager,
              RegistrationManager = colaborador.MatriculaGestor
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(newPerson);
          if (!string.IsNullOrEmpty(viewReturn.Message))
            FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, string.Format("{0} {1}","Pessoa não incluída.", viewReturn.Message)), EnumTypeLog.Information);
          else
            FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, string.Format("Pessoa {0} incluída.",viewReturn.IdPerson)), EnumTypeLog.Information);
          ControleColaboradores[search].IdPerson = viewReturn.IdPerson;
          ControleColaboradores[search].IdContract = viewReturn.IdContract;
          ControleColaboradores[search].Colaborador = colaborador;
        }
        else
        {
          string fieldChange = string.Empty;
          if (ControleColaboradores[search].Colaborador != null)
            fieldChange = ControleColaboradores[search].Colaborador.TestarMudanca(colaborador);

          // Alteração de colaborador
          if (ControleColaboradores[search].Colaborador == null || !string.IsNullOrEmpty(fieldChange))
          {
            PersonIntegration personIntegration = new PersonIntegration(Person);
            ViewIntegrationPersonV1 changePerson = new ViewIntegrationPersonV1()
            {
              _id = ControleColaboradores[search].IdPerson,
              Name = colaborador.Nome,
              Mail = colaborador.Email,
              Document = colaborador.Documento,
              DateBirth = colaborador.DataNascimento,
              Phone = colaborador.Celular,
              PhoneFixed = colaborador.Telefone,
              DocumentID = colaborador.Identidade,
              DocumentCTPF = colaborador.CarteiraProfissional,
              Sex = colaborador.Sexo.StartsWith("M") ? EnumSex.Male : colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others,
              Schooling = ControleColaboradores[search].Schooling,
              Contract = new ViewIntegrationContractV1()
              {
                Document = colaborador.Documento,
                Company = ControleColaboradores[search].Company,
                Registration = colaborador.Matricula,
                Establishment = ControleColaboradores[search].Establishment,
                DateAdm = colaborador.DataAdmissao,
                StatusUser = situacao,
                HolidayReturn = colaborador.DataRetornoFerias,
                MotiveAside = colaborador.MotivoAfastamento,
                DateResignation = colaborador.DataDemissao,
                Occupation = ControleColaboradores[search].Occupation,
                DateLastOccupation = colaborador.DataUltimaTrocaCargo,
                Salary = colaborador.SalarioNominal,
                DateLastReadjust = colaborador.DataUltimoReajuste,
                TypeUser = typeUser,
                _IdManager = ControleColaboradores[search].Manager?.IdPerson,
                DocumentManager = colaborador.DocumentoGestor,
                CompanyManager = ControleColaboradores[search].CompanyManager,
                RegistrationManager = colaborador.MatriculaGestor
              }
            };
            ViewIntegrationMapPersonV1 viewReturn = personIntegration.PutPerson(changePerson);
            if (!string.IsNullOrEmpty(viewReturn.Message))
              FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, string.Format("{0} {1}", "Pessoa não atualizada.", viewReturn.Message)), EnumTypeLog.Information);
            else
              FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, string.Format("Pessoa {0} atualizada com as seguintes alterações ({1}).", viewReturn.IdPerson, fieldChange)), EnumTypeLog.Information);
            ControleColaboradores[search].Colaborador = colaborador;
          }
          else
            FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, "Pessoa sem alterações."), EnumTypeLog.Information);
        }
      }
      string saveObject = JsonConvert.SerializeObject(ControleColaboradores);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);

      Status = EnumStatusService.Ok;
      if (hasLogFile)
      {
        Message = "Fim de integração com LOG!";
        Status = EnumStatusService.Error;
      }
    }
    #endregion
    #endregion
  }
}
