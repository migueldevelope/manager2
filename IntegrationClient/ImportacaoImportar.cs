using IntegrationService.Enumns;
using IntegrationService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      Text = "Importação de Colaboradores";
      serviceConfiguration = new ConfigurationService(Program.PersonLogin);
    }

    private void BtImp_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Program.PersonLogin, serviceConfiguration);
        import.Execute();
        if (import.Status == EnumStatusService.Error)
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        else
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        else
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        import.ExecuteV2();
        if (import.Status == EnumStatusService.Error)
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        else
          MessageBox.Show(import.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
