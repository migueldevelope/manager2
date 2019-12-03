﻿using IntegrationService.Enumns;
using IntegrationService.Service;
using System;
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
    private void BtImp_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
        import.Execute();
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
        MessageBox.Show(ex.Message, Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }
    private void BtDem_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
        import.Execute(DateTime.Parse(txtDatIni.Text).Date, DateTime.Parse(txtDatFin.Text).Date);
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
    private void BtnImpV2_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
        import.RefreshProgressBar += Import_RefreshProgressBar;
        import.ExecuteV2(chkLjo.Checked);
        if (import.Status == EnumStatusService.Error)
        {
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        prb.Maximum = 100;
        prb.Value = 0;
        lblPrb.Text = string.Empty;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void Import_RefreshProgressBar(object sender, EventArgs e)
    {
      prb.Maximum = ((ImportService)sender).ProgressBarMaximun;
      prb.Value = ((ImportService)sender).ProgressBarValue;
      lblPrb.Text = string.Format("{0} - Registros {1} de {2}",((ImportService)sender).ProgressMessage, prb.Value, prb.Maximum);
      Application.DoEvents();
    }
    private void BtnDemIndV2_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
        //import.ExecuteDemissionLastImportV2();
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

    private void BtnDemAusV2_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
        import.ExecuteDemissionAbsenceV2(chkLjo.Checked);
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
