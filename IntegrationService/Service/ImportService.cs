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
using Excel = Microsoft.Office.Interop.Excel;

namespace IntegrationService.Service
{
  public class ImportService
  {
    private readonly string pathLogs;
    public string Message { get; set; }
    public EnumStatusService Status { get; private set; }

    #region Private V2
    private List<ColaboradorV2Completo> ColaboradoresV2;
    private List<ColaboradorV2Base> GestoresV2;
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
        ProgressMessage = "Atualizando colaboradores 2/3...";
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
              colaborador.Colaborador.NomeEstabelecimento, colaborador.Colaborador.Matricula, string.Join("/", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Warning);
            if (!File.Exists(LogFileName.Replace(".log", "_json.log")))
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_json.log"), string.Format("Token: {0}", Person.Token), EnumTypeLineOpportunityg.Register);
              FileClass.SaveLog(LogFileName.Replace(".log", "_json.log"), "Json Post integrationserver/person/v2/completo", EnumTypeLineOpportunityg.Register);
            }
            FileClass.SaveLog(LogFileName.Replace(".log", "_json.log"), JsonConvert.SerializeObject(colaborador), EnumTypeLineOpportunityg.Register);
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
        ProgressBarMaximun = GestoresV2.Count;
        ProgressBarValue = 0;
        ProgressMessage = "Atualizando gestores 3/3...";
        OnRefreshProgressBar(EventArgs.Empty);
        // Atualização de gestores
        if (GestoresV2.Count > 0)
        {
          string result = string.Empty;
          foreach (ColaboradorV2Base gestor in GestoresV2)
          {
            result = personIntegration.PutV2PerfilGestor(gestor);
            FileClass.SaveLog(LogFileName.Replace(".log", "_gestor.log"), string.Format("{0};{1};{2};{3};{4}", gestor.Cpf, gestor.NomeEmpresa, gestor.NomeEstabelecimento,
              gestor.Matricula, result), EnumTypeLineOpportunityg.Register);
            ProgressBarValue++;
            OnRefreshProgressBar(EventArgs.Empty);
          }
        }
        Status = EnumStatusService.Ok;
        Message = "Fim de integração!";
        if (hasLogFile)
        {
          Message = "Fim de integração com LOG!";
          Status = EnumStatusService.Error;
        }
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
          List<string> cleanOccupations = new List<string>
          {
            "Jovem Aprendiz"
          };
          foreach (string occupation in cleanOccupations)
          {
            work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Equals(occupation.ToUpper()));
            while (work != -1)
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_clean.log"), string.Format("{0};{1};{2};{3};{4};{5};{6}", string.Format("{0} removido!",occupation),
                ColaboradoresV2[work].Colaborador.Cpf,
                ColaboradoresV2[work].Colaborador.Empresa, ColaboradoresV2[work].Colaborador.NomeEmpresa,
                ColaboradoresV2[work].Colaborador.Estabelecimento, ColaboradoresV2[work].Colaborador.NomeEstabelecimento,
                ColaboradoresV2[work].Nome), EnumTypeLineOpportunityg.Register);
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
        List<ColaboradorV2Ativo> colaboradores = personIntegration.GetActiveV2();
        ProgressBarMaximun = colaboradores.Count;
        ProgressBarValue = 0;
        ProgressMessage = "Verificando demissão de colaboradores 2/2...";
        OnRefreshProgressBar(EventArgs.Empty);
        ColaboradorV2Demissao demissao;
        ColaboradorV2Retorno viewRetorno;
        ColaboradorV2Completo view;
        bool fired = false;
        foreach (ColaboradorV2Ativo colaborador in colaboradores)
        {
          fired = true;
          foreach (var item in colaborador.Chaves)
          {
            if (service.Param.IntegrationKey == EnumIntegrationKey.CompanyEstablishment)
            {
              view = ColaboradoresV2.Find(p => p.Colaborador.Chave1() == string.Format("{0};{1};{2};{3}", colaborador.Cpf, item.Split(';')[0], item.Split(';')[1], colaborador.Matricula));
            }
            else
            {
              view = ColaboradoresV2.Find(p => p.Colaborador.Chave2() == string.Format("{0};{1};{2}", colaborador.Cpf, item.Split(';')[0], colaborador.Matricula));
            };
            if (view != null)
            {
              fired = false;
              break;
            }
          }
          if (fired)
          {
            demissao = new ColaboradorV2Demissao()
            {
              Colaborador = null,
              _id = colaborador._id,
              DataDemissao = DateTime.UtcNow
            };
            viewRetorno = personIntegration.PutV2Demissao(demissao);
            if (string.IsNullOrEmpty(viewRetorno.IdUser) || string.IsNullOrEmpty(viewRetorno.IdContract))
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_waring.log"), string.Format("{0};{1};{2}", colaborador.Cpf, colaborador.Matricula, string.Join(";", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Warning);
              hasLogFile = true;
              FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Matricula)), string.Format("Token: {0}", Person.Token), EnumTypeLineOpportunityg.Register);
              FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Matricula)), "Json Post integrationserver/person/v2/demissao", EnumTypeLineOpportunityg.Register);
              FileClass.SaveLog(LogFileName.Replace(".log", string.Format("_{0}.log", colaborador.Matricula)), JsonConvert.SerializeObject(demissao), EnumTypeLineOpportunityg.Register);
            }
            else
            {
              FileClass.SaveLog(LogFileName, string.Format("{0};{1};{2}", colaborador.Cpf, colaborador.Matricula, string.Join(";", viewRetorno.Mensagem)), EnumTypeLineOpportunityg.Information);
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
        ProgressBarMaximun = 100;
        ProgressBarValue = 0;
        ProgressMessage = "Carregando colaboradores 1/3...";
        OnRefreshProgressBar(EventArgs.Empty);
        ApiMetadados apiMetadados = new ApiMetadados();
        ColaboradoresV2 = new List<ColaboradorV2Completo>();
        GestoresV2 = new List<ColaboradorV2Base>();
        List<ViewIntegrationMetadadosV1> colaboradores;
        int offset = 0;
        int limit = 100;
        int register = 0;
        bool nextPage = true;
        ColaboradorV2Base gestor;
        while (nextPage)
        {
          colaboradores = new List<ViewIntegrationMetadadosV1>();
          colaboradores = apiMetadados.GetEmployee(service.Param.ApiToken, offset, limit);
          // Carregar Lista de Colaboradores
          foreach (ViewIntegrationMetadadosV1 colaborador in colaboradores)
          {
            ColaboradoresV2.Add(new ColaboradorV2Completo(colaborador, service.Param.CultureDate, Person.IdAccount));
            if (ColaboradoresV2[ColaboradoresV2.Count-1].Gestor != null)
            {
              gestor = ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor;
              if (GestoresV2.FindIndex(p => p.Empresa == gestor.Empresa && p.Estabelecimento == gestor.Estabelecimento && p.Cpf == gestor.Cpf && p.Matricula == gestor.Matricula) == -1)
              {
                GestoresV2.Add(ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor);
              }
            }
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
        GestoresV2 = new List<ColaboradorV2Base>();
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
        ProgressMessage = "Carregando colaboradores 1/3...";
        OnRefreshProgressBar(EventArgs.Empty);
        ColaboradorV2Base gestor;
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
          if (ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor != null)
          {
            gestor = ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor;
            if (GestoresV2.FindIndex(p => p.Empresa == gestor.Empresa && p.Estabelecimento == gestor.Estabelecimento && p.Cpf == gestor.Cpf && p.Matricula == gestor.Matricula) == -1)
            {
              GestoresV2.Add(ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor);
            }
          }
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
        int finalRow = excelApp.ActiveCell.Row;
        ProgressBarMaximun = finalRow + 1;
        ProgressBarValue = 0;
        ProgressMessage = "Carregando colaboradores 1/3...";
        OnRefreshProgressBar(EventArgs.Empty);

        #region Reformatação da planilha AEL
        if (Person.IdAccount.Equals("5d6ebda43501a40001d97db7")) 
        {
          excelPln.Range["A1"].Select();
          excelApp.Selection.End[Excel.XlDirection.xlDown].Select();
          // Buscar nome da empresa do gestor
          excelPln.Range["AN2"].Select();
          excelPln.Range["AN2"].FormulaR1C1 = "=VLOOKUP(RC[-2],C[-39]:C[-38],2,FALSE)";
          excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AN2:AN{0}", finalRow)]);
          excelPln.Range[string.Format("AN2:AN{0}", finalRow)].Select();
          excelApp.Selection.Copy();
          excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          // Buscar cpf do gestor
          excelPln.Range["AP2"].Select();
          excelPln.Range["AP2"].FormulaR1C1 = "=VLOOKUP(RC[-3],C[-39]:C[-19],21,FALSE)";
          excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AP2:AP{0}", finalRow)]);
          excelPln.Range[string.Format("AP2:AP{0}", finalRow)].Select();
          excelApp.Selection.Copy();
          excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          // Buscar estabelecimento do gestor
          excelPln.Range["AQ2"].Select();
          excelPln.Range["AQ2"].FormulaR1C1 = "=VLOOKUP(RC[-4],C[-40]:C[-9],32,FALSE)";
          excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AQ2:AQ{0}", finalRow)]);
          excelPln.Range[string.Format("AQ2:AQ{0}", finalRow)].Select();
          excelApp.Selection.Copy();
          excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          // Buscar nome do estabelecimento do gestor
          excelPln.Range["AR2"].Select();
          excelPln.Range["AR2"].FormulaR1C1 = "=VLOOKUP(RC[-5],C[-41]:C[-9],33,FALSE)";
          excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AR2:AR{0}", finalRow)]);
          excelPln.Range[string.Format("AR2:AR{0}", finalRow)].Select();
          excelApp.Selection.Copy();
          excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          excelPln.Columns[37].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[36].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[32].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[31].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[30].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[29].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[28].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[27].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[25].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[24].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[22].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[21].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[20].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[19].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[18].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[10].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[9].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Columns[5].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          excelPln.Cells[1, 1].Value = "empresa";
          excelPln.Cells[1, 2].Value = "nome_empresa";
          excelPln.Cells[1, 3].Value = "matricula";
          excelPln.Cells[1, 4].Value = "nome";
          excelPln.Cells[1, 5].Value = "situacao";
          excelPln.Cells[1, 6].Value = "centro_custo";
          excelPln.Cells[1, 7].Value = "nome_centro_custo";
          excelPln.Cells[1, 8].Value = "cargo";
          excelPln.Cells[1, 9].Value = "nome_cargo";
          excelPln.Cells[1, 10].Value = "salario_nominal";
          excelPln.Cells[1, 11].Value = "nome_grau_instrucao";
          excelPln.Cells[1, 12].Value = "grau_instrucao";
          excelPln.Cells[1, 13].Value = "data_admissao";
          excelPln.Cells[1, 14].Value = "data_nascimento";
          excelPln.Cells[1, 15].Value = "cpf";
          excelPln.Cells[1, 16].Value = "celular";
          excelPln.Cells[1, 17].Value = "sexo";
          excelPln.Cells[1, 18].Value = "estabelecimento";
          excelPln.Cells[1, 19].Value = "nome_estabelecimento";
          excelPln.Cells[1, 20].Value = "empresa_gestor";
          excelPln.Cells[1, 21].Value = "matricula_gestor";
          excelPln.Cells[1, 22].Value = "nome_empresa_gestor";
          excelPln.Cells[1, 23].Value = "email";
          excelPln.Cells[1, 24].Value = "cpf_gestor";
          excelPln.Cells[1, 25].Value = "estabelecimento_gestor";
          excelPln.Cells[1, 26].Value = "nome_estabelecimento_gestor";
        }
        #endregion

        excelPln.Range["A1"].Select();
        excelApp.Selection.End[Excel.XlDirection.xlToRight].Select();
        int collumnCount = excelApp.ActiveCell.Column + 1;
        excelPln.Range["A1"].Select();
        ColaboradoresV2 = new List<ColaboradorV2Completo>();
        GestoresV2 = new List<ColaboradorV2Base>();
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
        ColaboradorV2Base gestor;
        for (int row = 2; row < finalRow+1; row++)
        {
          rowData = new List<string>();

          for (int collumn = 1; collumn < collumnCount; collumn++)
          {
            collumName = GetStandardExcelColumnName(collumn);
            workData = excelPln.Range[string.Format("{0}{1}", collumName, row)].Value;
            rowData.Add(workData == null ? string.Empty : workData.ToString().Trim());
          }
          ColaboradoresV2.Add(new ColaboradorV2Completo(rowData, headers, service.Param.CultureDate));
          if (ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor != null)
          {
            gestor = ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor;
            if (GestoresV2.FindIndex(p => p.Empresa == gestor.Empresa && p.Estabelecimento == gestor.Estabelecimento && p.Cpf == gestor.Cpf && p.Matricula == gestor.Matricula) == -1)
            {
              GestoresV2.Add(ColaboradoresV2[ColaboradoresV2.Count - 1].Gestor);
            }
          }
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
