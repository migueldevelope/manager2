using IntegrationService.Api;
using IntegrationService.Data;
using IntegrationService.Enumns;
using IntegrationService.Tools;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace IntegrationService.Service
{
  public class ImportService
  {
    private readonly string pathLogs;
    public string Message { get; set; }
    public EnumStatusService Status { get; private set; }

    #region Private V1
    private List<ColaboradorImportar> Colaboradores;
    private List<ControleColaborador> ControleColaboradores;
    #endregion

    #region Private V2
    private List<ColaboradorV2Completo> ColaboradoresV2;
    #endregion

    private readonly ViewPersonLogin Person;
    //    private ViewIntegrationParameter Param;
    private ConfigurationService service;
    private PersonIntegration personIntegration;
    private string LogFileName;
    private readonly Version VersionProgram;
    private bool hasLogFile;
    private bool executeDemission;

    #region Construtores
    public ImportService(ViewPersonLogin person, ConfigurationService serviceConfiguration, DateTime initialTime)
    {
      try
      {
        Person = person;
        service = serviceConfiguration;
        Message = string.Empty;
        Status = EnumStatusService.Ok;
        pathLogs = string.Format("{0}_{1}/integration", Person.NameAccount, Person.IdAccount);
        if (!Directory.Exists(pathLogs))
        {
          Directory.CreateDirectory(pathLogs);
        }
        // Limpeza de arquivos LOG
        DirectoryInfo diretorio = new DirectoryInfo(pathLogs);
        List<FileInfo> Arquivos = diretorio.GetFiles("*.log").Where(p => p.CreationTime.Date < DateTime.Now.Date.AddDays(-30)).ToList();
        //Comea a listar o(s) arquivo(s)
        foreach (FileInfo fileinfo in Arquivos)
        {
          File.Delete(fileinfo.FullName);
        }
        LogFileName = string.Format("{0}/{1}.log", pathLogs, initialTime.ToString("yyyyMMdd_HHmmss"));
        Assembly assem = Assembly.GetEntryAssembly();
        AssemblyName assemName = assem.GetName();
        VersionProgram = assemName.Version;
        personIntegration = new PersonIntegration(Person);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Shared
    private DataTable ReadData()
    {
      try
      {
        bool oracle = false;
        ConnectionString conn;
        if (service.Param.ConnectionString.StartsWith("ODBC"))
        {
          conn = new ConnectionString(service.Param.ConnectionString.Split('|')[1])
          {
            Sql = service.Param.SqlCommand
          };
        }
        else
        {
          oracle = service.Param.ConnectionString.Split(';')[0].Equals("Oracle");
          string hostname = service.Param.ConnectionString.Split(';')[1];
          string user = service.Param.ConnectionString.Split(';')[2];
          string password = service.Param.ConnectionString.Split(';')[3];
          string baseDefault = service.Param.ConnectionString.Split(';')[4];
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
        }
        GetPersonSystem gps = new GetPersonSystem(conn);
        return gps.GetPerson();
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    private string ValidColumns(List<string> validColumns, List<string> columns)
    {
      try
      {
        if (columns.Count != validColumns.Count)
          return string.Format("Número de colunas inválida. Deveria ter {0} e tem {1}.", validColumns.Count, columns.Count);

        MemberInfo[] mebers = null;
        DescriptionAttribute[] attribs = null;

        string message = string.Empty;
        int pos = 0;
        foreach (string item in validColumns)
        {
          mebers = item.GetType().GetMember(item.ToString());
          attribs = (mebers != null && mebers.Length > 0) ? (DescriptionAttribute[])mebers[0].GetCustomAttributes(typeof(DescriptionAttribute), false) : null;
          if (!attribs[0].Description.ToLower().Contains(item.ToLower()))
            message = string.Format("{0}{1} Coluna {2} não encontrada na posição {3}", message, Environment.NewLine, item, pos);
          pos++;
        }
        return message;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
    #endregion

    #region V1

    #region Ponto de Entrada para Importação de Colaboradores V1
    public void Execute()
    {
      try
      {
        // Ver se tem pendências de integração
        personIntegration.GetStatusIntegration();

        switch (service.Param.Mode)
        {
          case EnumIntegrationMode.DataBase:
            Message = "Modo banco de dados não suportado no processo manual!";
            throw new Exception(Message);
          case EnumIntegrationMode.FileCsv:
            FileCsvV1();
            break;
          case EnumIntegrationMode.FileExcel:
            FileExcelV1();
            break;
          case EnumIntegrationMode.ApplicationInterface:
            CallApiMode();
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
        service.Param.CriticalError = string.Empty;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Ok";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);

      }
      catch (Exception ex)
      {
        if (string.IsNullOrEmpty(Message))
          Message = ex.Message;
        service.Param.CriticalError = Message;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Critical Error";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
        throw;
      }
    }
    public void Execute(DateTime initial, DateTime final)
    {
      try
      {
        switch (service.Param.Mode)
        {
          case EnumIntegrationMode.ApplicationInterface:
            CallApiMode(initial, final);
            break;
          default:
            throw new Exception("Apenas chamadas a APIs");
        }
        LogFileName = string.Format("{0}/{1}_demissao.log", pathLogs, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        Demission();
        service.Param.CriticalError = string.Empty;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Ok";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
      }
      catch (Exception ex)
      {
        if (string.IsNullOrEmpty(Message))
          Message = ex.Message;
        service.Param.CriticalError = Message;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Critical Error";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
        throw;
      }
    }
    #endregion

    #region Api Region V1
    private void CallApiMode()
    {
      try
      {
        if (service.Param.ApiIdentification == "UNIMEDNERS")
        {
          ApiUnimedNers();
        }
        else
        {
          Message = "Identificação da API inválida";
          throw new Exception(Message);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void CallApiMode(DateTime initial, DateTime final)
    {
      try
      {
        if (service.Param.ApiIdentification == "UNIMEDNERS")
        {
          ApiUnimedNers(initial, final);
        }
        else
        {
          Message = "Identificação da API inválida";
          throw new Exception(Message);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void ApiUnimedNers()
    {
      try
      {
        ApiUnimedNers unimedNers = new ApiUnimedNers();

        List<ViewIntegrationUnimedNers> colaboradoresUnimed = unimedNers.GetUnimedEmployee();

        Colaboradores = new List<ColaboradorImportar>();
        // Carregar Lista de Colaboradores
        foreach (ViewIntegrationUnimedNers colaboradorUnimed in colaboradoresUnimed)
        {
          Colaboradores.Add(new ColaboradorImportar(colaboradorUnimed, new EnumLayoutSystemCompleteV1()));
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void ApiUnimedNers(DateTime initial, DateTime final)
    {
      try
      {
        ApiUnimedNers unimedNers = new ApiUnimedNers();
        List<ViewIntegrationUnimedNers> colaboradoresUnimed = unimedNers.GetUnimedEmployee(initial, final);
        Colaboradores = new List<ColaboradorImportar>();
        // Carregar Lista de Colaboradores
        foreach (ViewIntegrationUnimedNers colaboradoreUnimed in colaboradoresUnimed)
        {
          Colaboradores.Add(new ColaboradorImportar(colaboradoreUnimed, new EnumLayoutSystemCompleteV1()));
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Listas V1
    private void LoadLists()
    {
      try
      {
        ControleColaboradores = new List<ControleColaborador>();
        //if (File.Exists(string.Format("{0}/colaborador.txt", pathLogs)))
        //  ControleColaboradores = JsonConvert.DeserializeObject<List<ControleColaborador>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/colaborador.txt", pathLogs)));
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
        string saveObject = JsonConvert.SerializeObject(ControleColaboradores);
        FileClass.WriteToBinaryFile(string.Format("{0}/colaborador.txt", pathLogs), saveObject, false);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Preparação Banco de Dados V1
    private void DatabaseV1()
    {
      try
      {
        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.ConnectionString))
          throw new Exception("Sem string de conexão.");

        if (string.IsNullOrEmpty(service.Param.SqlCommand))
          throw new Exception("Sem comando de leitura do banco de dados.");

        // Ler banco de dados
        DataTable readData = ReadData();
        if (readData.Rows.Count == 0)
          throw new Exception("Não tem nenhum colaborador como retorno da consulta.");

        throw new Exception("Validar importação.");
        // Tratar os dados
        //switch (service.Param.Type)
        //{
        //  case EnumIntegrationType.Basic:
        //    Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutSystemBasicV1)).ToList(), readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
        //    break;
        //  case EnumIntegrationType.Complete:
        //    Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutSystemCompleteV1)).ToList(), readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
        //    break;
        //}
        //if (!string.IsNullOrEmpty(Message))
        //  throw new Exception(Message);

        //Colaboradores = new List<ColaboradorImportar>();
        //// Carregar Lista de Colaboradores
        //foreach (DataRow row in readData.Rows)
        //{
        //  List<string> teste = row.ItemArray.Select(x => x.ToString()).ToList();
        //  switch (service.Param.Type)
        //  {
        //    case EnumIntegrationType.Basic:
        //      Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemBasicV1()));
        //      break;
        //    case EnumIntegrationType.Complete:
        //      Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemCompleteV1()));
        //      break;
        //  }
        //}
      }
      catch (Exception ex)
      {
        Status = EnumStatusService.CriticalError;
        throw ex;
      }
    }
    #endregion

    #region Preparação Arquivo CSV V1
    private void FileCsvV1()
    {
      try
      {
        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.FilePathLocal))
          throw new Exception("Sem arquivo de importação definido.");

        if (!File.Exists(service.Param.FilePathLocal))
          throw new Exception(string.Format("Arquivo {0} não encontrado.", service.Param.FilePathLocal));

        throw new Exception("Validar importação.");
        //// Tratar os dados
        //StreamReader rd = new StreamReader(service.Param.FilePathLocal, Encoding.Default, true);
        //List<string> readLine = null;
        //readLine = rd.ReadLine().Split(';').ToList();
        //// Tratar os dados
        //switch (service.Param.Process)
        //{
        //  case EnumIntegrationProcess.Manual:
        //    switch (service.Param.Type)
        //    {
        //      case EnumIntegrationType.Basic:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutManualBasicV1)).ToList(), readLine);
        //        break;
        //      case EnumIntegrationType.Complete:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).ToList(), readLine);
        //        break;
        //    }
        //    break;
        //  case EnumIntegrationProcess.System:
        //    switch (service.Param.Type)
        //    {
        //      case EnumIntegrationType.Basic:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutSystemBasicV1)).ToList(), readLine);
        //        break;
        //      case EnumIntegrationType.Complete:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutSystemCompleteV1)).ToList(),readLine);
        //        break;
        //    }
        //    break;
        //}
        //if (!string.IsNullOrEmpty(Message))
        //  throw new Exception(Message);

        //// Carregar Lista de Colaboradores
        //Colaboradores = new List<ColaboradorImportar>();
        //string register;
        //while ((register = rd.ReadLine()) != null)
        //{
        //  readLine = register.Split(';').ToList();
        //  switch (service.Param.Process)
        //  {
        //    case EnumIntegrationProcess.Manual:
        //      switch (service.Param.Type)
        //      {
        //        case EnumIntegrationType.Basic:
        //          if (readLine.Count == Enum.GetNames(typeof(EnumLayoutManualBasicV1)).Length)
        //            Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutManualBasicV1()));
        //          break;
        //        case EnumIntegrationType.Complete:
        //          if (readLine.Count == Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).Length)
        //            Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutManualCompleteV1()));
        //          break;
        //      }
        //      break;
        //    case EnumIntegrationProcess.System:
        //      switch (service.Param.Type)
        //      {
        //        case EnumIntegrationType.Basic:
        //          Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutSystemBasicV1()));
        //          break;
        //        case EnumIntegrationType.Complete:
        //          Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutSystemCompleteV1()));
        //          break;
        //      }
        //      break;
        //    default:
        //      break;
        //  }
        //}
        //rd.Close();
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    #endregion

    #region Preparação Arquivo Excel V1
    private void FileExcelV1()
    {
      try
      {
        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.FilePathLocal))
          throw new Exception("Sem arquivo de importação definido.");

        if (!File.Exists(service.Param.FilePathLocal))
          throw new Exception(string.Format("Arquivo {0} não encontrado.", service.Param.FilePathLocal));

        // Ler banco de dados
        DataTable readData = ReadDataXls();
        if (readData.Rows.Count == 0)
          throw new Exception("Não tem nenhum colaborador como retorno da consulta.");

        throw new Exception("Validar Importação.");
        //// Tratar os dados
        //switch (service.Param.Process)
        //{
        //  case EnumIntegrationProcess.Manual:
        //    switch (service.Param.Type)
        //    {
        //      case EnumIntegrationType.Basic:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutManualBasicV1)).ToList(), readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
        //        break;
        //      case EnumIntegrationType.Complete:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).ToList(), readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
        //        break;
        //    }
        //    break;
        //  case EnumIntegrationProcess.System:
        //    switch (service.Param.Type)
        //    {
        //      case EnumIntegrationType.Basic:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutSystemBasicV1)).ToList(), readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
        //        break;
        //      case EnumIntegrationType.Complete:
        //        Message = ValidColumns(Enum.GetNames(typeof(EnumLayoutSystemCompleteV1)).ToList(), readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
        //        break;
        //    }
        //    break;
        //}
        //if (!string.IsNullOrEmpty(Message))
        //{
        //  throw new Exception(Message);
        //}
        //Colaboradores = new List<ColaboradorImportar>();
        //// Carregar Lista de Colaboradores
        //foreach (DataRow row in readData.Rows)
        //{
        //  List<string> teste = row.ItemArray.Select(x => x.ToString()).ToList();
        //  switch (service.Param.Process)
        //  {
        //    case EnumIntegrationProcess.Manual:
        //      switch (service.Param.Type)
        //      {
        //        case EnumIntegrationType.Basic:
        //          Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutManualBasicV1()));
        //          break;
        //        case EnumIntegrationType.Complete:
        //          Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutManualCompleteV1()));
        //          break;
        //      }
        //      break;
        //    case EnumIntegrationProcess.System:
        //      switch (service.Param.Type)
        //      {
        //        case EnumIntegrationType.Basic:
        //          Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemBasicV1()));
        //          break;
        //        case EnumIntegrationType.Complete:
        //          Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemCompleteV1()));
        //          break;
        //      }
        //      break;
        //  }
        //}
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    private DataTable ReadDataXls()
    {
      try
      {
        string connectionString;
        if (Path.GetExtension(service.Param.FilePathLocal) == ".xlsx")
          connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", service.Param.FilePathLocal);
        else
          connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", service.Param.FilePathLocal);

        DataTable dataTable = new DataTable();
        using (OleDbConnection connection = new OleDbConnection(connectionString))
        {
          connection.Open();
          OleDbCommand command = new OleDbCommand(string.Format("select * from [{0}$]", service.Param.SheetName), connection);
          dataTable.Load(command.ExecuteReader(CommandBehavior.SingleResult));
        }
        return dataTable;
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    #endregion

    #region Carregar Lista de Controle V1
    private void FinalImport()
    {
      try
      {
        int search;

        foreach (var colaborador in Colaboradores.Where(p => !string.IsNullOrEmpty(p.Message)))
        {
          search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
          if (search == -1)
          {
            ControleColaboradores.Add(new ControleColaborador(colaborador));
            search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
            ControleColaboradores[search].Situacao = EnumColaboradorSituacao.LocalError;
            FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, colaborador.Message), EnumTypeLineOpportunityg.Warning);
            Status = EnumStatusService.Error;
            hasLogFile = true;
          }
        }

        ViewIntegrationColaborador viewColaborador;
        foreach (var colaborador in Colaboradores.Where(p => string.IsNullOrEmpty(p.Message)))
        {
          search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
          if (search == -1)
          {
            ControleColaboradores.Add(new ControleColaborador(colaborador));
            search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == colaborador.ChaveColaborador);
          }
          else
            ControleColaboradores[search].SetColaborador(colaborador);

          switch (ControleColaboradores[search].Situacao)
          {
            case EnumColaboradorSituacao.NoChange:
              FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", ControleColaboradores[search].Colaborador.ChaveColaborador, ControleColaboradores[search].Colaborador.Nome, "Pessoa sem alteração."), EnumTypeLineOpportunityg.Warning);
              break;
            case EnumColaboradorSituacao.SendServer:
              viewColaborador = new ViewIntegrationColaborador()
              {
                Colaborador = ControleColaboradores[search].Colaborador,
                CamposAlterados = ControleColaboradores[search].CamposAlterados,
                IdContract = ControleColaboradores[search].IdContract,
                IdPerson = ControleColaboradores[search].IdPerson,
                Situacao = ControleColaboradores[search].Situacao,
                Message = string.Empty
              };
              viewColaborador = personIntegration.PutPerson(viewColaborador);
              search = ControleColaboradores.FindIndex(p => p.ChaveColaborador == ControleColaboradores[search].Colaborador.ChaveColaborador);
              ControleColaboradores[search].Message = viewColaborador.Message;
              ControleColaboradores[search].Situacao = viewColaborador.Situacao;
              if (viewColaborador.Situacao == EnumColaboradorSituacao.Atualized)
              {
                ControleColaboradores[search].IdPerson = viewColaborador.IdPerson;
                ControleColaboradores[search].IdContract = viewColaborador.IdContract;
                ControleColaboradores[search].Message = viewColaborador.Message;
                FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", ControleColaboradores[search].Colaborador.ChaveColaborador, ControleColaboradores[search].Colaborador.Nome, viewColaborador.Message), EnumTypeLineOpportunityg.Information);
              }
              else
              {
                ControleColaboradores[search].Message = viewColaborador.Message;
                FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", ControleColaboradores[search].Colaborador.ChaveColaborador, ControleColaboradores[search].Colaborador.Nome, string.Format("Pessoa não atualizada. {0}", viewColaborador.Message)), EnumTypeLineOpportunityg.Warning);
                FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", viewColaborador.Colaborador.Matricula)), string.Format("Token: {0}", Person.Token),EnumTypeLineOpportunityg.Register);
                FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", viewColaborador.Colaborador.Matricula)), "Json PutPerson", EnumTypeLineOpportunityg.Register);
                FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", viewColaborador.Colaborador.Matricula)), JsonConvert.SerializeObject(viewColaborador), EnumTypeLineOpportunityg.Register);
                hasLogFile = true;
              }
              break;
            case EnumColaboradorSituacao.LocalError:
              break;
            case EnumColaboradorSituacao.ServerError:
              break;
            case EnumColaboradorSituacao.Atualized:
              break;
            default:
              break;
          }
        }
        SaveLists();
        Status = EnumStatusService.Ok;
        Message = "Fim de integração!";
        if (hasLogFile)
        {
          Message = "Fim de integração com LOG!";
          Status = EnumStatusService.Error;
        }
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    private void Demission()
    {
      try
      {
        ViewIntegrationColaborador viewColaborador;
        foreach (var colaborador in Colaboradores)
        {
          viewColaborador = new ViewIntegrationColaborador()
          {
            Colaborador = colaborador,
            Message = string.Empty
          };
          viewColaborador = personIntegration.PutPersonDemission(viewColaborador);
          if (viewColaborador.Situacao == EnumColaboradorSituacao.Atualized)
          {
            FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, viewColaborador.Message), EnumTypeLineOpportunityg.Information);
          }
          else
          {
            FileClass.SaveLog(LogFileName, string.Format("{0},{1},{2}", colaborador.ChaveColaborador, colaborador.Nome, string.Format("Pessoa não demitida. {0}", viewColaborador.Message)), EnumTypeLineOpportunityg.Warning);
            hasLogFile = true;
          }
        }
        Status = EnumStatusService.Ok;
        Message = "Fim de integração!";
        if (hasLogFile)
        {
          Message = "Fim de integração com LOG!";
          Status = EnumStatusService.Error;
        }
      }
      catch (Exception)
      {
        Status = EnumStatusService.CriticalError;
        throw;
      }
    }
    #endregion

    #endregion

    #region V2

    #region Inicio da integração
    public void ExecuteV2(bool jsonLog)
    {
      try
      {
        executeDemission = true;
        FileClass.SaveLog(LogFileName, string.Format("Iniciando o processo de integração."), EnumTypeLineOpportunityg.Information);
        LoadEmploee();
        if (ColaboradoresV2.Count == 0)
        {
          throw new Exception("Lista de colaboradores vazia.");
        }
        CleanEmploee();
        // Rotina de Importação
        ColaboradorV2Retorno viewRetorno;
        ProgressBarMaximun = ColaboradoresV2.Count;
        ProgressBarValue = 0;
        ProgressMessage = "Atualizando colaboradores 2/2...";
        OnRefreshProgressBar(EventArgs.Empty);
        foreach (ColaboradorV2Completo colaborador in ColaboradoresV2)
        {
          if (jsonLog)
          {
            if (!File.Exists(LogFileName.Replace(".log", "_api.log")))
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_api.log"), string.Format("Token: {0}", Person.Token), EnumTypeLineOpportunityg.Register);
            }
            FileClass.SaveLog(LogFileName.Replace(".log", "_api.log"), JsonConvert.SerializeObject(colaborador), EnumTypeLineOpportunityg.Register);
          }
          viewRetorno = personIntegration.PostV2Completo(colaborador);
          if (string.IsNullOrEmpty(viewRetorno.IdUser) || string.IsNullOrEmpty(viewRetorno.IdContract))
          {
            FileClass.SaveLog(LogFileName.Replace(".log", "_waring.log"), string.Format("{0};{1};{2};{3};{4};{5}", colaborador.Colaborador.Cpf, colaborador.Nome, colaborador.Colaborador.NomeEmpresa,
              colaborador.Colaborador.NomeEstabelecimento, colaborador.Colaborador.Matricula, string.Join(";", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Warning);
            FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Colaborador.Matricula)), string.Format("Token: {0}", Person.Token), EnumTypeLineOpportunityg.Register);
            FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Colaborador.Matricula)), "Json Post integrationserver/person/v2/completo", EnumTypeLineOpportunityg.Register);
            FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Colaborador.Matricula)), JsonConvert.SerializeObject(colaborador), EnumTypeLineOpportunityg.Register);
            hasLogFile = true;
          }
          else
          {
            FileClass.SaveLog(LogFileName, string.Format("{0};{1};{2};{3};{4};{5}", colaborador.Colaborador.Cpf, colaborador.Nome, colaborador.Colaborador.NomeEmpresa,
              colaborador.Colaborador.NomeEstabelecimento, colaborador.Colaborador.Matricula, string.Join(";", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Information);
          }
          ProgressBarValue++;
          OnRefreshProgressBar(EventArgs.Empty);
        }
        Status = EnumStatusService.Ok;
        Message = "Fim de integração!";
        if (hasLogFile)
        {
          Message = "Fim de integração com LOG!";
          Status = EnumStatusService.Error;
        }
        // até aqui
        service.Param.CriticalError = string.Empty;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Ok v2";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
        FileClass.SaveLog(LogFileName, string.Format("Finalizando o processo de integração."), EnumTypeLineOpportunityg.Information);
        if (executeDemission)
        {
          //ExecuteDemissionAbsenceV2(jsonLog);
        }
      }
      catch (Exception ex)
      {
        Status = EnumStatusService.CriticalError;
        Message = ex.Message;
        FileClass.SaveLog(LogFileName, string.Format("Erro de integração critico: {0}", ex.Message), EnumTypeLineOpportunityg.Error);
        service.Param.CriticalError = ex.Message;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Critical Error";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
        throw ex;
      }
    }
    private void LoadEmploee()
    {
      try
      {
        switch (service.Param.Mode)
        {
          case EnumIntegrationMode.DataBase:
            DatabaseV2();
            break;
          case EnumIntegrationMode.FileCsv:
            throw new Exception("Modo CSV não implementado no processo de sistema.");
          case EnumIntegrationMode.FileExcel:
            ExcelV2();
            break;
          case EnumIntegrationMode.ApplicationInterface:
            switch (service.Param.ApiIdentification.ToUpper())
            {
              case "UNIMEDNERS":
                ApiUnimedNersV2();
                break;
              default:
                throw new Exception("Identificação da API inválida (UNIMEDNERS).");
            }
            break;
          default:
            throw new Exception("Nenhum modo implementado no processo de sistema.");
        }
        if (Status == EnumStatusService.CriticalError)
        {
          throw new Exception(Message);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void CleanEmploee()
    {
      try
      {
        if (Person.IdAccount.Equals("5b91299a17858f95ffdb79f6")) // Unimed Nordeste Rs
        {
          // Limpeza de matriculas vazias
          int work = ColaboradoresV2.FindIndex(p => p.Colaborador.Matricula == null);
          while (work != -1)
          {
            FileClass.SaveLog(LogFileName.Replace(".log", "_clean.log"), string.Format("{0};{1};{2};{3};{4};{5};{6}", "Sem matricula!",
              ColaboradoresV2[work].Colaborador.Cpf,
              ColaboradoresV2[work].Colaborador.Empresa, ColaboradoresV2[work].Colaborador.NomeEmpresa,
              ColaboradoresV2[work].Colaborador.Estabelecimento, ColaboradoresV2[work].Colaborador.NomeEstabelecimento,
              ColaboradoresV2[work].Nome), EnumTypeLineOpportunityg.Warning);
            ColaboradoresV2.RemoveAt(work);
            work = ColaboradoresV2.FindIndex(p => p.Colaborador.Matricula == null);
            executeDemission = false;
          }
          // Limpeza de Jovens Aprendizes
          work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Contains("JOVEM APRENDIZ"));
          while (work != -1)
          {
            FileClass.SaveLog(LogFileName.Replace(".log", "_clean.log"), string.Format("{0};{1};{2};{3};{4};{5};{6}", "Jovem Aprendiz removido!",
              ColaboradoresV2[work].Colaborador.Cpf,
              ColaboradoresV2[work].Colaborador.Empresa, ColaboradoresV2[work].Colaborador.NomeEmpresa,
              ColaboradoresV2[work].Colaborador.Estabelecimento, ColaboradoresV2[work].Colaborador.NomeEstabelecimento,
              ColaboradoresV2[work].Nome), EnumTypeLineOpportunityg.Register);
            ColaboradoresV2.RemoveAt(work);
            work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Contains("JOVEM APRENDIZ"));
          }
        }
        if (Person.IdAccount.Equals("5cb8bbfb27a5e8f3ef548b1f")) // Bertolini
        {
          // remover uma determinada lista de cargos
          List<string> cleanOccupations = new List<string>
          {
            "Cargo 1",
            "Cargo 2",
            "Cargo 3",
            "Cargo 4",
            "Cargo 5"
          };
          foreach (string occupation in cleanOccupations)
          {
            int work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Equals(occupation.ToUpper()));
            while (work != -1)
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_waring.log"), string.Format("{0};{1};{2};{3};{4};{5}", "Cargo não autorizado!", ColaboradoresV2[work].Colaborador.Cpf, ColaboradoresV2[work].Colaborador.NomeEmpresa, ColaboradoresV2[work].Colaborador.NomeEstabelecimento, ColaboradoresV2[work].Nome, string.Empty), EnumTypeLineOpportunityg.Warning);
              ColaboradoresV2.RemoveAt(work);
              work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Equals(occupation.ToUpper()));
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public void ExecuteDemissionAbsenceV2(bool jsonLog)
    {
      try
      {
        LogFileName = LogFileName.Replace(".log", "_demissao.log");
        FileClass.SaveLog(LogFileName, string.Format("Iniciando o processo de integração."), EnumTypeLineOpportunityg.Information);
        if (ColaboradoresV2 == null || ColaboradoresV2.Count == 0)
        {
          LoadEmploee();
          if (ColaboradoresV2.Count == 0)
          {
            throw new Exception("Lista de colaboradores vazia.");
          }
          CleanEmploee();
        }
        // Rotina de Demissão por Ausencia
        List<ColaboradorV2Base> colaboradores = personIntegration.GetActiveV2();
        ProgressBarMaximun = colaboradores.Count;
        ProgressBarValue = 0;
        ProgressMessage = "Verificando demissão de colaboradores 2/2...";
        OnRefreshProgressBar(EventArgs.Empty);
        ColaboradorV2Demissao demissao;
        ColaboradorV2Retorno viewRetorno;
        ColaboradorV2Completo view;
        foreach (ColaboradorV2Base colaborador in colaboradores)
        {
          if (service.Param.IntegrationKey == EnumIntegrationKey.CompanyEstablishment)
          {
            view = ColaboradoresV2.Find(p => p.Colaborador.Chave1 == colaborador.Chave1);
          }
          else
          {
            view = ColaboradoresV2.Find(p => p.Colaborador.Chave2 == colaborador.Chave2);
          };
          if (view == null)
          {
            demissao = new ColaboradorV2Demissao()
            {
              Colaborador = colaborador,
              DataDemissao = DateTime.Now.Date
            };
            viewRetorno = personIntegration.PutV2Demissao(demissao);
            if (string.IsNullOrEmpty(viewRetorno.IdUser) || string.IsNullOrEmpty(viewRetorno.IdContract))
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_waring.log"), string.Format("{0};{1};{2};{3};{4}", colaborador.Cpf, colaborador.NomeEmpresa,
                colaborador.NomeEstabelecimento, colaborador.Matricula, string.Join(";", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Warning);
              hasLogFile = true;
              FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Matricula)), string.Format("Token: {0}", Person.Token), EnumTypeLineOpportunityg.Register);
              FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Matricula)), "Json Post integrationserver/person/v2/demissao", EnumTypeLineOpportunityg.Register);
              FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Matricula)), JsonConvert.SerializeObject(demissao), EnumTypeLineOpportunityg.Register);
            }
            else
            {
              FileClass.SaveLog(LogFileName, string.Format("{0};{1};{2};{3};{4}", colaborador.Cpf, colaborador.NomeEmpresa,
                colaborador.NomeEstabelecimento, colaborador.Matricula, string.Join(";", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Information);
            }
          }
          ProgressBarValue++;
          OnRefreshProgressBar(EventArgs.Empty);
        }
        Status = EnumStatusService.Ok;
        Message = "Fim de integração!";
        if (hasLogFile)
        {
          Message = "Fim de integração com LOG!";
          Status = EnumStatusService.Error;
        }
        FileClass.SaveLog(LogFileName, string.Format("Finalizando o processo de integração."), EnumTypeLineOpportunityg.Information);
      }
      catch (Exception ex)
      {
        Status = EnumStatusService.CriticalError;
        Message = ex.Message;
        FileClass.SaveLog(LogFileName, string.Format("Erro de integração critico: {0}", ex.Message), EnumTypeLineOpportunityg.Error);
        service.Param.CriticalError = ex.Message;
        service.Param.LastExecution = DateTime.UtcNow;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Critical Error";
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
        throw ex;
      }
    }

    #region Leitura de Colaboradores
    private void ApiUnimedNersV2()
    {
      try
      {
        ApiMetadados apiMetadados = new ApiMetadados();
        ColaboradoresV2 = new List<ColaboradorV2Completo>();
        List<ViewIntegrationMetadadosV1> colaboradores;
        int offset = 0;
        int limit = 100;
        int register = 0;
        bool nextPage = true;
        while (nextPage)
        {
          colaboradores = new List<ViewIntegrationMetadadosV1>();
          colaboradores = apiMetadados.GetEmployee(service.Param.ApiToken, offset, limit);
          // Carregar Lista de Colaboradores
          foreach (ViewIntegrationMetadadosV1 colaborador in colaboradores)
          {
            ColaboradoresV2.Add(new ColaboradorV2Completo(colaborador, service.Param.CultureDate));
            // FileClass.SaveLog("juremir.txt", string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}", register, offset, colaborador.Cpf, colaborador.Empresa, colaborador.NomeEmpresa, colaborador.Estabelecimento, colaborador.NomeEstabelecimento, colaborador.Matricula, colaborador.Nome, colaborador.NomeCargo, colaborador.NomeClassContabil, colaborador.Email), EnumTypeLineOpportunityg.Register);
            register++;
          }
          offset = offset + limit;
          if (colaboradores.Count < limit)
          {
            nextPage = false;
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void DatabaseV2()
    {
      try
      {
        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.ConnectionString))
        {
          throw new Exception("Sem string de conexão.");
        }
        if (string.IsNullOrEmpty(service.Param.SqlCommand))
        {
          throw new Exception("Sem comando de leitura do banco de dados.");
        }
        // Ler banco de dados
        DataTable readData = ReadData();
        if (readData.Rows.Count == 0)
        {
          throw new Exception("Não tem nenhum colaborador para integração.");
        }
        ColaboradoresV2 = new List<ColaboradorV2Completo>();
        // Carregar Lista de Colaboradores
        bool validColumn = true;
        List<string> columnsValidV2 = new List<string>
        {
          "cpf", "empresa", "nome_empresa", "estabelecimento", "nome_estabelecimento", "matricula", "nome", "email", "sexo",
          "data_nascimento", "celular", "grau_instrucao", "nome_grau_instrucao", "apelido", "situacao", "data_admissao",
          "data_demissao", "cargo", "nome_cargo", "data_ultima_troca_cargo", "cpf_gestor", "empresa_gestor", "nome_empresa_gestor",
          "estabelecimento_gestor", "nome_estabelecimento_gestor", "matricula_gestor", "nome_gestor", "centro_custo", "nome_centro_custo",
          "data_troca_centro_custo", "salario_nominal", "carga_horaria", "data_ultimo_reajuste", "motivo_ultimo_reajuste"
        };
        ProgressBarMaximun = readData.Rows.Count;
        ProgressBarValue = 0;
        ProgressMessage = "Carregando colaboradores 1/2...";
        OnRefreshProgressBar(EventArgs.Empty);
        foreach (DataRow row in readData.Rows)
        {
          if (validColumn)
          {
            foreach (string columnName in readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList())
            {
              if (columnsValidV2.FindIndex(p => p.Equals(columnName.ToLower())) == -1)
              {
                throw new Exception(string.Format("Coluna {0} está na lista mas não será utilizada. Verifique o nome ou retire da lista!", columnName));
              }
            }
            validColumn = false;
          }
          ColaboradoresV2.Add(new ColaboradorV2Completo(row.ItemArray.Select(x => x.ToString()).ToList(),
            readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList(), service.Param.CultureDate));
          ProgressBarValue++;
          OnRefreshProgressBar(EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void ExcelV2()
    {
      try
      {
        // Validar parâmetros
        if (string.IsNullOrEmpty(service.Param.FilePathLocal))
        {
          throw new Exception("Sem nome de pasta Excel informada.");
        }
        if (string.IsNullOrEmpty(service.Param.SheetName))
        {
          throw new Exception("Sem nome de planilha informada.");
        }
        if (!File.Exists(service.Param.FilePathLocal))
        {
          throw new Exception("Pasta Microsoft Excel não localizada.");
        }
        // Ler planilha de dados
        Excel.Application excelApp = new Excel.Application
        {
          DisplayAlerts = false,
          Visible = true
        };
        Excel.Workbook excelPst = excelApp.Workbooks.Open(service.Param.FilePathLocal, false);
        Excel.Worksheet excelPln = excelPst.Worksheets[service.Param.SheetName];
        excelPln.Activate();
        excelPln.Range["A1"].Select();
        excelApp.Selection.End[Excel.XlDirection.xlDown].Select();
        int rowCount = excelApp.ActiveCell.Row + 1;
        excelPln.Range["A1"].Select();
        excelApp.Selection.End[Excel.XlDirection.xlToRight].Select();
        int collumnCount = excelApp.ActiveCell.Column + 1;
        excelPln.Range["A1"].Select();
        ColaboradoresV2 = new List<ColaboradorV2Completo>();
        // Carregar Lista de Colaboradores
        List<string> columnsValidV2 = new List<string>
        {
          "cpf", "empresa", "nome_empresa", "estabelecimento", "nome_estabelecimento", "matricula", "nome", "email", "sexo",
          "data_nascimento", "celular", "grau_instrucao", "nome_grau_instrucao", "apelido", "situacao", "data_admissao",
          "data_demissao", "cargo", "nome_cargo", "data_ultima_troca_cargo", "cpf_gestor", "empresa_gestor", "nome_empresa_gestor",
          "estabelecimento_gestor", "nome_estabelecimento_gestor", "matricula_gestor", "nome_gestor", "centro_custo", "nome_centro_custo",
          "data_troca_centro_custo", "salario_nominal", "carga_horaria", "data_ultimo_reajuste", "motivo_ultimo_reajuste", "acao",
          // COmpatibilidade V1
          "telefone", "identidade", "carteira_profissional", "retorno_ferias", "motivo_afastamento",
          "empresa_chefe", "nome_empresa_chefe", "estabelecimento_chefe", "nome_estabelecimento_chefe", "cpf_chefe", "matricula_chefe"
        };
        ProgressBarMaximun = rowCount;
        ProgressBarValue = 0;
        ProgressMessage = "Carregando colaboradores 1/2...";
        OnRefreshProgressBar(EventArgs.Empty);
        List<string> headers = new List<string>();
        string work;
        for (int collumn = 1; collumn < collumnCount; collumn++)
        {
          work = excelPln.Cells[1, collumn].Value.Trim().ToLower();
          if (columnsValidV2.FindIndex(p => p.Equals(work)) == -1)
          {
            throw new Exception(string.Format("Coluna {0} está na lista mas não será utilizada. Verifique o nome ou retire da lista!", work));
          }
          headers.Add(work);
        }
        ProgressBarValue++;
        OnRefreshProgressBar(EventArgs.Empty);
        List<string> rowData;
        string collumName;
        dynamic workData;
        for (int row = 2; row < rowCount; row++)
        {
          rowData = new List<string>();

          for (int collumn = 1; collumn < collumnCount; collumn++)
          {
            collumName = GetStandardExcelColumnName(collumn);
            workData = excelPln.Range[string.Format("{0}{1}", collumName, row)].Value;
            rowData.Add(workData == null ? string.Empty : workData.ToString().Trim());
          }
          ColaboradoresV2.Add(new ColaboradorV2Completo(rowData, headers, service.Param.CultureDate));
          ProgressBarValue++;
          OnRefreshProgressBar(EventArgs.Empty);
        }
        excelPst.Close(false);
        excelApp.Workbooks.Close();
        excelApp.Quit();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public static string GetStandardExcelColumnName(int columnNumberOneBased)
    {
      int baseValue = Convert.ToInt32('A');
      int columnNumberZeroBased = columnNumberOneBased - 1;

      string ret = "";

      if (columnNumberOneBased > 26)
      {
        ret = GetStandardExcelColumnName(columnNumberZeroBased / 26);
      }
      return ret + Convert.ToChar(baseValue + (columnNumberZeroBased % 26));
    }

    #endregion

    #endregion

    #endregion

    #region ToolBar
    public int ProgressBarMaximun;
    public int ProgressBarValue;
    public string ProgressMessage;

    public event EventHandler RefreshProgressBar;

    protected virtual void OnRefreshProgressBar(EventArgs e)
    {
      if (RefreshProgressBar != null)
      {
        EventHandler handler = RefreshProgressBar;
        handler(this, e);
      }
    }

    #endregion
  }
}
