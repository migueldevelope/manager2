using System;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using IntegrationService.Tools;
using Manager.Views.Integration;
using IntegrationService.Service;
using IntegrationService.Enumns;

namespace IntegrationClient
{
  public partial class Login : Form
  {
    public Login()
    {
      InitializeComponent();
    }

    private void Login_Load(object sender, EventArgs e)
    {
      Text = string.Format("Login Robo Ana - Versão {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
      if (System.IO.File.Exists(Program.FileConfig))
      {
        Program.PersonLogin = JsonConvert.DeserializeObject<ViewPersonLogin>(FileClass.ReadFromBinaryFile<string>(Program.FileConfig));
        string pathLogs = string.Format("{0}_{1}/integration", Program.PersonLogin.NameAccount, Program.PersonLogin.IdAccount);
        string LogFileName = string.Format("{0}/{1}.log", pathLogs, DateTime.Now.ToString("AUTO_yyyyMMdd_HHmmss"));
        // Ler arquivo persistido
        if (Program.autoImport && Program.autoVersion.Equals("V1"))
        {
          FileClass.SaveLog(LogFileName, "Rotina automática V1 iniciada", EnumTypeLineOpportunityg.Information);
          Environment.ExitCode = 0;
          // Atualizar funcionários
          try
          {
            FileClass.SaveLog(LogFileName, "Admissões e alterações", EnumTypeLineOpportunityg.Information);
            ConfigurationService serviceConfiguration = new ConfigurationService(Program.PersonLogin);
            ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
            import.Execute();
            if (import.Status == EnumStatusService.Error)
            {
              Environment.ExitCode = 1;
            }
          }
          catch
          {
            Environment.ExitCode = 1;
          }
          // Demitir funcionários
          if (Environment.ExitCode == 0)
          {
            try
            {
              FileClass.SaveLog(LogFileName, "Demissões", EnumTypeLineOpportunityg.Information);
              ConfigurationService serviceConfiguration = new ConfigurationService(Program.PersonLogin);
              ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
              import.Execute(DateTime.Now.Date, DateTime.Now.Date);
              if (import.Status == EnumStatusService.Error)
              {
                Environment.ExitCode = 1;
              }
            }
            catch
            {
              Environment.ExitCode = 1;
            }
          }
          FileClass.SaveLog(LogFileName, "Rotina automática V1 encerrada", EnumTypeLineOpportunityg.Information);
          Application.Exit();
        }
        if (Program.autoImport && Program.autoVersion.Equals("V2"))
        {
          FileClass.SaveLog(LogFileName, "Rotina automática V2 iniciada", EnumTypeLineOpportunityg.Information);
          Environment.ExitCode = 0;
          // Atualizar funcionários
          try
          {
            FileClass.SaveLog(LogFileName, "Admissões e alterações", EnumTypeLineOpportunityg.Information);
            ConfigurationService serviceConfiguration = new ConfigurationService(Program.PersonLogin);
            ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
            import.ExecuteV2(false);
            if (import.Status == EnumStatusService.Error)
            {
              Environment.ExitCode = 1;
            }
          }
          catch
          {
            Environment.ExitCode = 1;
          }
          // Demitir funcionários
          if (Environment.ExitCode == 0)
          {
            try
            {
              FileClass.SaveLog(LogFileName, "Demissões por ausencia de ativos", EnumTypeLineOpportunityg.Information);
              ConfigurationService serviceConfiguration = new ConfigurationService(Program.PersonLogin);
              ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
              import.ExecuteDemissionAbsenceV2(false);
              if (import.Status == EnumStatusService.Error)
              {
                Environment.ExitCode = 1;
              }
            }
            catch
            {
              Environment.ExitCode = 1;
            }
          }
          FileClass.SaveLog(LogFileName, "Rotina automática V2 encerrada", EnumTypeLineOpportunityg.Information);
          Application.Exit();
        }
        if (!Program.autoImport)
        {
          Thread t = new Thread(new ThreadStart(CallMenu));
          t.Start();
          Close();
        }
      }
    }

    private void BtCan_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void BtOk_Click(object sender, EventArgs e)
    {
      try
      {
        // Iniciar Autenticação
        var auth = new IntegrationService.Api.Authentication();
        Program.PersonLogin = auth.Connect(txtUrl.Text, txtEma.Text, txtSen.Text);

        // Se estiver OK preparar o objeto de persistência local
        FileClass.WriteToBinaryFile<string>(Program.FileConfig, JsonConvert.SerializeObject(Program.PersonLogin), false);

        // Prepara chamada outro formulário
        Thread t = new Thread(new ThreadStart(CallMenu));
        t.Start();
        Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    public static void CallMenu()
    {
      Application.Run(new Menu());
    }
  }
}
