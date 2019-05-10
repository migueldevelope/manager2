using IntegrationService.Api;
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
using System.Data.OleDb;
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

    private List<ColaboradorImportar> Colaboradores;
    private List<ControleColaborador> ControleColaboradores;
    private readonly ViewPersonLogin Person;
    //    private ViewIntegrationParameter Param;
    private ConfigurationService service;
    private PersonIntegration personIntegration;
    private readonly string LogFileName;
    private readonly Version VersionProgram;
    private bool hasLogFile;

    #region Construtores
    public ImportService(ViewPersonLogin person, ConfigurationService serviceConfiguration)
    {
      try
      {
        Person = person;
        service = serviceConfiguration;
        Message = string.Empty;
        Status = EnumStatusService.Ok;
        if (!Directory.Exists(string.Format("{0}/integration", Person.IdAccount)))
          Directory.CreateDirectory(string.Format("{0}/integration", Person.IdAccount));
        // Limpeza de arquivos LOG

        DirectoryInfo diretorio = new DirectoryInfo(string.Format("{0}/integration", Person.IdAccount));
        List<FileInfo> Arquivos = diretorio.GetFiles("*.log").Where(p => p.CreationTime.Date < DateTime.Now.Date).ToList();
        //Comea a listar o(s) arquivo(s)
        foreach (FileInfo fileinfo in Arquivos)
          File.Delete(fileinfo.FullName);

        LogFileName = string.Format("{0}/integration/{1}.log", Person.IdAccount, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
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

    #region Ponto de Entrada para Importação de Colaboradores
    public void Execute()
    {
      try
      {
        // Ver se tem pendências de integração
        personIntegration.GetStatusIntegration();

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
                FileExcelV1();
                break;
              case EnumIntegrationMode.ApplicationInterface:
                CallApiMode();
                break;
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
                FileExcelV1();
                break;
              case EnumIntegrationMode.ApplicationInterface:
                CallApiMode();
                break;
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
        service.Param.CriticalError = string.Empty;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Ok";
        service.Param.CustomVersionExecution = string.Empty;
        service.Param.UploadNextLog = false;
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);

      }
      catch (Exception ex)
      {
        if (string.IsNullOrEmpty(Message))
          Message = ex.Message;
        service.Param.CriticalError = Message;
        service.Param.MachineIdentity = Environment.GetEnvironmentVariable("COMPUTERNAME");
        service.Param.StatusExecution = "Critical Error";
        service.Param.CustomVersionExecution = string.Empty;
        service.Param.UploadNextLog = false;
        service.Param.ProgramVersionExecution = VersionProgram.ToString();
        service.SetParameter(service.Param);
        throw;
      }
    }
    #endregion

    #region Api Region
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

    private void ApiUnimedNers()
    {
      try
      {
        ApiUnimedNers unimedNers = new ApiUnimedNers();

        List<ViewIntegrationUnimedNers> colaboradoresUnimed = unimedNers.GetUnimedEmployee();

        Colaboradores = new List<ColaboradorImportar>();
        // Carregar Lista de Colaboradores
        foreach (ViewIntegrationUnimedNers colaboradoreUnimed in colaboradoresUnimed)
          Colaboradores.Add(new ColaboradorImportar(colaboradoreUnimed, new EnumLayoutSystemCompleteV1()));

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Listas
    private void LoadLists()
    {
      try
      {
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
        string saveObject = JsonConvert.SerializeObject(ControleColaboradores);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);
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
        if (service.Param.Type == EnumIntegrationType.Custom)
        {
          Message = "Rotina deve ser do tipo customizada!!!";
          throw new Exception(Message);
        }
        if (service.Param.Process != EnumIntegrationProcess.System)
        {
          Message = "Apenas processo de sistema pode utilizar banco de dados!!!";
          throw new Exception(Message);
        }
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
        Colaboradores = new List<ColaboradorImportar>();
        // Carregar Lista de Colaboradores
        foreach (DataRow row in readData.Rows)
        {
          List<string> teste = row.ItemArray.Select(x => x.ToString()).ToList();
          switch (service.Param.Type)
          {
            case EnumIntegrationType.Basic:
              Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemBasicV1()));
              break;
            case EnumIntegrationType.Complete:
              Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemCompleteV1()));
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
        Colaboradores = new List<ColaboradorImportar>();
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
                    Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutManualBasicV1()));
                  break;
                case EnumIntegrationType.Complete:
                  if (readLine.Count == Enum.GetNames(typeof(EnumLayoutManualCompleteV1)).Length)
                    Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutManualCompleteV1()));
                  break;
              }
              break;
            case EnumIntegrationProcess.System:
              switch (service.Param.Type)
              {
                case EnumIntegrationType.Basic:
                  Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutSystemBasicV1()));
                  break;
                case EnumIntegrationType.Complete:
                  Colaboradores.Add(new ColaboradorImportar(readLine, new EnumLayoutSystemCompleteV1()));
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
    private void FileExcelV1()
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
        // Ler banco de dados
        DataTable readData = ReadDataXls();
        if (readData.Rows.Count == 0)
        {
          Message = "Não tem nenhum colaborador como retorno da consulta!!!";
          throw new Exception(Message);
        }
        // Tratar os dados
        switch (service.Param.Process)
        {
          case EnumIntegrationProcess.Manual:
            switch (service.Param.Type)
            {
              case EnumIntegrationType.Basic:
                Message = ValidColumnManualBasicV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
                break;
              case EnumIntegrationType.Complete:
                Message = ValidColumnManualCompleteV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
                break;
            }
            break;
          case EnumIntegrationProcess.System:
            switch (service.Param.Type)
            {
              case EnumIntegrationType.Basic:
                Message = ValidColumnSystemBasicV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
                break;
              case EnumIntegrationType.Complete:
                Message = ValidColumnSystemCompleteV1(readData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
                break;
            }
            break;
        }
        if (!string.IsNullOrEmpty(Message))
        {
          throw new Exception(Message);
        }
        Colaboradores = new List<ColaboradorImportar>();
        // Carregar Lista de Colaboradores
        foreach (DataRow row in readData.Rows)
        {
          List<string> teste = row.ItemArray.Select(x => x.ToString()).ToList();
          switch (service.Param.Process)
          {
            case EnumIntegrationProcess.Manual:
              switch (service.Param.Type)
              {
                case EnumIntegrationType.Basic:
                  Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutManualBasicV1()));
                  break;
                case EnumIntegrationType.Complete:
                  Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutManualCompleteV1()));
                  break;
              }
              break;
            case EnumIntegrationProcess.System:
              switch (service.Param.Type)
              {
                case EnumIntegrationType.Basic:
                  Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemBasicV1()));
                  break;
                case EnumIntegrationType.Complete:
                  Colaboradores.Add(new ColaboradorImportar(row.ItemArray.Select(x => x.ToString()).ToList(), new EnumLayoutSystemCompleteV1()));
                  break;
              }
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
          if (!attribs[0].Description.ToLower().Contains(columns[(int)item].ToLower()))
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
          if (!atribs[0].Description.ToLower().Contains(columns[(int)item].ToLower()))
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
          if (!atribs[0].Description.ToLower().Contains(columns[(int)item].ToLower()))
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
          if (!atribs[0].Description.ToLower().Contains(columns[(int)item].ToLower()))
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

    #region Carregar Lista de Controle
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
    #endregion

  }
}
