using IntegrationClient.Api;
using IntegrationClient.Enumns;
using IntegrationClient.Tools;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace IntegrationClient.Service
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
    private readonly ConfigurationService service;
    private readonly PersonIntegration personIntegration;
    private string LogFileName;
    private readonly Version VersionProgram;
    private bool hasLogFile;

    #region Construtores
    public ImportService(ViewPersonLogin person, ConfigurationService serviceConfiguration, DateTime initialTime)
    {
      try
      {
        Person = person;
        service = serviceConfiguration;
        Message = string.Empty;
        Status = EnumStatusService.Ok;
        pathLogs = string.Format("{0}{1}_{2}/integration", AppDomain.CurrentDomain.BaseDirectory, Person.NameAccount, Person.IdAccount);
        if (!Directory.Exists(pathLogs))
        {
          _ = Directory.CreateDirectory(pathLogs);
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
    public void ExecuteV2(bool jsonLog, bool listEnabled, bool full)
    {
      try
      {
        FileClass.SaveLog(LogFileName, string.Format("Iniciando o processo de integração."), EnumTypeLineOpportunityg.Information);
        LoadEmploee();
        if (ColaboradoresV2.Count == 0)
        {
          throw new Exception("Lista de colaboradores vazia.");
        }
        CleanEmploee();
        if (listEnabled)
        {
          // Gerar planilha com lista de colaboradores ativos
          ListEmploeeEnabled();
        }
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
        if (full)
        {
          ExecuteDemissionAbsenceV2(jsonLog);
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
        // Unimed Nordeste Rs
        if (Person.IdAccount.Equals("5b91299a17858f95ffdb79f6"))
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
        // AEL
        if (Person.IdAccount.Equals("5d6ebda43501a40001d97db7"))
        {
          // Limpeza de Jovens Aprendizes
          List<string> cleanOccupations = new List<string>
          {
            "Aprendiz do Senai"
          };
          int work = -1;
          foreach (string occupation in cleanOccupations)
          {
            work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Equals(occupation.ToUpper()));
            while (work != -1)
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_clean.log"), string.Format("{0};{1};{2};{3};{4};{5};{6}", string.Format("{0} removido!", occupation),
                ColaboradoresV2[work].Colaborador.Cpf,
                ColaboradoresV2[work].Colaborador.Empresa, ColaboradoresV2[work].Colaborador.NomeEmpresa,
                ColaboradoresV2[work].Colaborador.Estabelecimento, ColaboradoresV2[work].Colaborador.NomeEstabelecimento,
                ColaboradoresV2[work].Nome), EnumTypeLineOpportunityg.Register);
              ColaboradoresV2.RemoveAt(work);
              work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Equals(occupation.ToUpper()));
            }
          }
        }
        // FALLGATTER
        if (Person.IdAccount.Equals("5db199964432430001d07675"))
        {
          // Limpeza de Jovens Aprendizes
          List<string> cleanOccupations = new List<string>
          {
            "Aprendiz Senai",
            "Estagiario",
            "Estagiário",
            "Menor Aprendiz"
          };
          int work = -1;
          foreach (string occupation in cleanOccupations)
          {
            work = ColaboradoresV2.FindIndex(p => p.NomeCargo.ToUpper().Equals(occupation.ToUpper()));
            while (work != -1)
            {
              FileClass.SaveLog(LogFileName.Replace(".log", "_clean.log"), string.Format("{0};{1};{2};{3};{4};{5};{6}", string.Format("{0} removido!", occupation),
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
    private void ListEmploeeEnabled()
    {
      try
      {
        ProgressBarMaximun = ColaboradoresV2.Count;
        ProgressBarValue = 0;
        ProgressMessage = "Opcional, Lista de Colaboradores ativos ...";
        OnRefreshProgressBar(EventArgs.Empty);
        // Ler planilha de dados
        Excel.Application excelApp = new Excel.Application
        {
          DisplayAlerts = false,
          Visible = true
        };
        Excel.Workbook excelPst = excelApp.Workbooks.Add();
        Excel.Worksheet excelPln = excelPst.Worksheets[1];
        excelPln.Range["A1"].Value = "cpf";
        excelPln.Range["B1"].Value = "Empresa";
        excelPln.Range["C1"].Value = "Nome_Empresa";
        excelPln.Range["D1"].Value = "Estabelecimento";
        excelPln.Range["E1"].Value = "Nome_Estabelecimento";
        excelPln.Range["F1"].Value = "Matricula";
        excelPln.Range["G1"].Value = "Nome";
        excelPln.Range["H1"].Value = "Situação";
        excelPln.Range["I1"].Value = "Cargo";
        excelPln.Range["J1"].Value = "Escolaridade";
        int line = 1;
        int current = 0;
        foreach (ColaboradorV2Completo item in ColaboradoresV2)
        {
          line++;
          excelPln.Range[string.Format("A{0}", line)].Value = item.Colaborador.Cpf;
          excelPln.Range[string.Format("B{0}", line)].Value = item.Colaborador.Empresa;
          excelPln.Range[string.Format("C{0}", line)].Value = item.Colaborador.NomeEmpresa;
          excelPln.Range[string.Format("D{0}", line)].Value = item.Colaborador.Estabelecimento;
          excelPln.Range[string.Format("E{0}", line)].Value = item.Colaborador.NomeEstabelecimento;
          excelPln.Range[string.Format("F{0}", line)].Value = item.Colaborador.Matricula;
          excelPln.Range[string.Format("G{0}", line)].Value = item.Nome;
          excelPln.Range[string.Format("H{0}", line)].Value = item.Situacao;
          excelPln.Range[string.Format("J{0}", line)].Value = item.NomeCargo;
          excelPln.Range[string.Format("I{0}", line)].Value = item.NomeGrauInstrucao;
          current++;
          ProgressBarValue = current;
          OnRefreshProgressBar(EventArgs.Empty);
        }
        excelPst.SaveAs(LogFileName.Replace(".log", "_employee_active.xlsx"));
        excelPst.Close(false);
        excelApp.Workbooks.Close();
        excelApp.Quit();
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
            switch (service.Param.IntegrationKey)
            {
              case EnumIntegrationKey.CompanyEstablishment:
                view = ColaboradoresV2.Find(p => p.Colaborador.Chave1() == string.Format("{0};{1};{2};{3}", colaborador.Cpf, item.Split(';')[0], item.Split(';')[1], colaborador.Matricula));
                break;
              case EnumIntegrationKey.Company:
                view = ColaboradoresV2.Find(p => p.Colaborador.Chave2() == string.Format("{0};{1};{2}", colaborador.Cpf, item.Split(';')[0], colaborador.Matricula));
                break;
              case EnumIntegrationKey.Document:
                view = ColaboradoresV2.Find(p => p.Colaborador.Cpf == colaborador.Cpf);
                break;
              default:
                throw new Exception("Tipo de integração de chaves inválida");
            }
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
          ProgressMessage = string.Format("Carregando colaboradores 1/3 - {0}...",offset);
          OnRefreshProgressBar(EventArgs.Empty);
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
          offset += limit;
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
        _ = excelPln.Range["A1"].Select();
        _ = excelApp.Selection.End[Excel.XlDirection.xlDown].Select();
        int finalRow = excelApp.ActiveCell.Row;
        ProgressBarMaximun = finalRow + 1;
        ProgressBarValue = 0;
        ProgressMessage = "Carregando colaboradores 1/3...";
        OnRefreshProgressBar(EventArgs.Empty);

        #region Reformatação da planilha AEL
        if (Person.IdAccount.Equals("5d6ebda43501a40001d97db7")) 
        {
          _ = excelPln.Range["A1"].Select();
          _ = excelApp.Selection.End[Excel.XlDirection.xlDown].Select();
          // Buscar nome da empresa do gestor
          _ = excelPln.Range["AN2"].Select();
          excelPln.Range["AN2"].FormulaR1C1 = @"=IF(ISERROR(VLOOKUP(RC[-2],C[-39]:C[-38],2,FALSE)),"""",VLOOKUP(RC[-2],C[-39]:C[-38],2,FALSE))";
          _ = excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AN2:AN{0}", finalRow)]);
          _ = excelPln.Range[string.Format("AN2:AN{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          // Buscar cpf do gestor
          _ = excelPln.Range["AP2"].Select();
          excelPln.Range["AP2"].FormulaR1C1 = @"=IF(ISERROR(VLOOKUP(RC[-3],C[-39]:C[-19],21,FALSE)),"""",VLOOKUP(RC[-3],C[-39]:C[-19],21,FALSE))";
          _ = excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AP2:AP{0}", finalRow)]);
          _ = excelPln.Range[string.Format("AP2:AP{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          // Buscar estabelecimento do gestor
          _ = excelPln.Range["AQ2"].Select();
          excelPln.Range["AQ2"].FormulaR1C1 = @"=IF(ISERROR(VLOOKUP(RC[-4],C[-40]:C[-9],32,FALSE)),"""",VLOOKUP(RC[-4],C[-40]:C[-9],32,FALSE))";
          _ = excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AQ2:AQ{0}", finalRow)]);
          _ = excelPln.Range[string.Format("AQ2:AQ{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          // Buscar nome do estabelecimento do gestor
          _ = excelPln.Range["AR2"].Select();
          excelPln.Range["AR2"].FormulaR1C1 = @"=IF(ISERROR(VLOOKUP(RC[-5],C[-41]:C[-9],33,FALSE)),"""",VLOOKUP(RC[-5],C[-41]:C[-9],33,FALSE))"; 
          _ = excelApp.ActiveCell.AutoFill(excelPln.Range[string.Format("AR2:AR{0}", finalRow)]);
          _ = excelPln.Range[string.Format("AR2:AR{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);
          _ = excelPln.Columns[37].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[36].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[32].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[31].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[30].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[29].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[28].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[27].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[25].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[24].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[22].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[21].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[20].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[19].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[18].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[10].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[9].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns[5].EntireColumn.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
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

        #region Reformatação da planilha FALLGATTER
        if (Person.IdAccount.Equals("5db199964432430001d07675"))
        {
          // Retirar os e-mails que não são fallgatter
          for (int row = 2; row < finalRow; row++)
          {
            if (!string.IsNullOrEmpty(excelPln.Range[string.Format("H{0}",row)].Value))
            {
              string mailWork = excelPln.Range[string.Format("H{0}", row)].Value;
              if ( !mailWork.Trim().ToLower().EndsWith("@fallgatter.com.br") )
              {
                excelPln.Range[string.Format("H{0}", row)].Value = string.Empty;
              }
            }
          }
          // Ajuste da coluna de situação
          // Retirar os e-mails que não são fallgatter
          for (int row = 2; row < finalRow; row++)
          {
            if (!string.IsNullOrEmpty(excelPln.Range[string.Format("L{0}", row)].Value))
            {
              string mailWork = excelPln.Range[string.Format("L{0}", row)].Value;
              if (mailWork.Trim().ToLower().Equals("atestado") || mailWork.Trim().ToLower().Equals("auxilio") 
                || mailWork.Trim().ToLower().Equals("seguro") || mailWork.Trim().ToLower().Equals("suspensão"))
              {
                excelPln.Range[string.Format("L{0}", row)].Value = "Afastado";
              }
            }
          }
          _ = excelPln.Range["A1"].EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
          _ = excelPln.Range["A2"].Select();
          excelPln.Range["A2"].FormulaR1C1 = @"=CONCATENATE(RC[4],"";"",RC[6])";
          _ = excelApp.Selection.AutoFill(excelPln.Range[string.Format("A2:A{0}", finalRow)]);
          _ = excelPln.Range["U2"].Select();
          excelPln.Range["U2"].FormulaR1C1 = @"=CONCATENATE(RC[-2],"";"",RC[-1])";
          _ = excelApp.Selection.AutoFill(excelPln.Range[string.Format("U2:U{0}", finalRow)]);

          _ = excelPln.Range["V1"].Select();
          excelPln.Range["V1"].Value = "empresa_gestor";
          _ = excelPln.Range["V2"].Select();
          excelPln.Range["V2"].FormulaR1C1 = "=VLOOKUP(RC[-1],C1:C6,3,FALSE)";
          _ = excelApp.Selection.AutoFill(excelPln.Range[string.Format("V2:V{0}", finalRow)]);
          _ = excelPln.Range[string.Format("V2:V{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);

          _ = excelPln.Range["W1"].Select();
          excelPln.Range["W1"].Value = "nome_empresa_gestor";
          _ = excelPln.Range["W2"].Select();
          excelPln.Range["W2"].FormulaR1C1 = "=VLOOKUP(RC[-2],C1:C6,4,FALSE)";
          _ = excelApp.Selection.AutoFill(excelPln.Range[string.Format("W2:W{0}", finalRow)]);
          _ = excelPln.Range[string.Format("W2:W{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);

          _ = excelPln.Range["X1"].Select();
          excelPln.Range["X1"].Value = "nome_estabelecimento_gestor";
          _ = excelPln.Range["X2"].Select();
          excelPln.Range["X2"].FormulaR1C1 = "=VLOOKUP(RC[-3],C1:C6,6,FALSE)";
          _ = excelApp.Selection.AutoFill(excelPln.Range[string.Format("X2:X{0}", finalRow)]);
          _ = excelPln.Range[string.Format("X2:X{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);

          _ = excelPln.Range["Y1"].Select();
          excelPln.Range["Y1"].Value = "cpf_gestor";
          _ = excelPln.Range["Y2"].Select();
          excelPln.Range["Y2"].FormulaR1C1 = "=VLOOKUP(RC[-4],C1:C6,2,FALSE)";
          _ = excelApp.Selection.AutoFill(excelPln.Range[string.Format("Y2:Y{0}", finalRow)]);
          _ = excelPln.Range[string.Format("Y2:Y{0}", finalRow)].Select();
          _ = excelApp.Selection.Copy();
          _ = excelApp.Selection.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.Constants.xlNone, false, false);

          _ = excelPln.Columns["U:U"].Select();
          _ = excelApp.Selection.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
          _ = excelPln.Columns["A:A"].Select();
          _ = excelApp.Selection.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);

          _ = excelPln.Range["A1"].Select();
          _ = excelPln.Cells.Replace("#N/D", string.Empty, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows);
        }
        #endregion

        _ = excelPln.Range["A1"].Select();
        _ = excelApp.Selection.End[Excel.XlDirection.xlToRight].Select();
        int collumnCount = excelApp.ActiveCell.Column + 1;
        _ = excelPln.Range["A1"].Select();
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
            if ((workData is int) && ((int)workData == -2146826246))
            {
              workData = null;
            } ;
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
