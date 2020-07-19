using IntegrationClient.Enumns;
using IntegrationClient.Service;
using Manager.Views.Enumns;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace IntegrationClient
{
  public partial class ImportacaoImportar : Form
  {
    private ConfigurationService serviceConfiguration;
    public ImportacaoImportar()
    {
      InitializeComponent();
    }
    private void ImportacaoImportar_Load(object sender, EventArgs e)
    {
      prb.Maximum = 100;
      prb.Value = 0;
      lblPrb.Text = string.Empty;
      serviceConfiguration = new ConfigurationService(Program.PersonLogin);
    }
    private void BtnDemAus_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration, DateTime.Now);
        import.RefreshProgressBar += Import_RefreshProgressBar;
        switch (serviceConfiguration.Param.Version)
        {
          case EnumIntegrationVersion.V1:
            MessageBox.Show("Versão de ausência não implementada!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            break;
          case EnumIntegrationVersion.V2:
            import.ExecuteDemissionAbsenceV2(chkLjo.Checked);
            break;
          default:
            break;
        }
        if (import.Status == EnumStatusService.Error)
        {
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void Import_RefreshProgressBar(object sender, EventArgs e)
    {
      try
      {
        prb.Maximum = ((ImportService)sender).ProgressBarMaximun;
        prb.Value = ((ImportService)sender).ProgressBarValue;
        lblPrb.Text = string.Format("{0} - Registros {1} de {2}", ((ImportService)sender).ProgressMessage, prb.Value, prb.Maximum);
        Application.DoEvents();
      }
      catch (Exception)
      {

      }
    }

    private void BtnImp_Click(object sender, EventArgs e)
    {
      try
      {
        var before0 = GC.CollectionCount(0);
        var before1 = GC.CollectionCount(1);
        var before2 = GC.CollectionCount(2);
        var sw = new Stopwatch();
        sw.Start();
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration, DateTime.Now);
        import.RefreshProgressBar += Import_RefreshProgressBar;
        switch (serviceConfiguration.Param.Version)
        {
          case EnumIntegrationVersion.V1:
            MessageBox.Show("Versão de ausência não implementada!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            break;
          case EnumIntegrationVersion.V2:
            if (((Button)sender).Name.Equals("btnImp"))
            {
              import.ExecuteV2(chkLjo.Checked, chkLstAti.Checked, false);
            }
            if (((Button)sender).Name.Equals("btnAmb"))
            {
              import.ExecuteV2(chkLjo.Checked, chkLstAti.Checked, true);
            }
            break;
          default:
            break;
        }
        sw.Stop();
        Console.WriteLine($"Time .: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"Gen(0) .: {GC.CollectionCount(0) - before0}");
        Console.WriteLine($"Gen(1) .: {GC.CollectionCount(1) - before1}");
        Console.WriteLine($"Gen(2) .: {GC.CollectionCount(2) - before2}");
        Console.WriteLine($"Memory .: {Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} mb");

        if (import.Status == EnumStatusService.Error)
        {
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
