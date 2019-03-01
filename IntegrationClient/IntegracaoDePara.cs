using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntegrationService.Api;
using Manager.Views.Integration;

namespace IntegrationClient
{
  public partial class IntegracaoDePara : Form
  {
    private int tipoDePara;
    public IntegracaoDePara()
    {
      InitializeComponent();
    }

    private void IntegracaoDePara_Load(object sender, EventArgs e)
    {
      lblMsg.Text = string.Empty;
      tipoDePara = 0;
      MontaGrid();
    }

    private void MontaGrid()
    {
      try
      {
        MatchIntegration matchIntegration = new MatchIntegration(Program.PersonLogin);
        List<ViewIntegrationCompany> listView = new List<ViewIntegrationCompany>();
        switch (tipoDePara)
        {
          case 0: // empresa
            if (chkT.Checked)
              listView = matchIntegration.GetIntegrationCompanies(1000, 1, string.Empty, true);
            else
              listView = matchIntegration.GetIntegrationCompanies();
            break;
          default:
            break;
        }
        dgvP.DataSource = listView;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void MontaGridOrigem()
    {
      try
      {
        MatchIntegration matchIntegration = new MatchIntegration(Program.PersonLogin);
        List<ViewIntegrationCompany> listView = new List<ViewIntegrationCompany>();
        switch (tipoDePara)
        {
          case 0: // empresa
            if (chkT.Checked)
              listView = matchIntegration.GetIntegrationCompanies(1000, 1, string.Empty, true);
            else
              listView = matchIntegration.GetIntegrationCompanies();
            break;
          default:
            break;
        }
        dgvP.DataSource = listView;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void ChkT_CheckedChanged(object sender, EventArgs e)
    {
      MontaGrid();
    }

    private void RbEmp_CheckedChanged(object sender, EventArgs e)
    {
      txtFil.Text = string.Empty;
      tipoDePara = 0;
      MontaGrid();
    }

    private void RbEstab_CheckedChanged(object sender, EventArgs e)
    {
      txtFil.Text = string.Empty;
      tipoDePara = 1;
      MontaGrid();
    }

    private void RbCargo_CheckedChanged(object sender, EventArgs e)
    {
      txtFil.Text = string.Empty;
      tipoDePara = 2;
      MontaGrid();
    }

    private void RbEsc_CheckedChanged(object sender, EventArgs e)
    {
      txtFil.Text = string.Empty;
      tipoDePara = 3;
      MontaGrid();
    }

    private void BtnFil_Click(object sender, EventArgs e)
    {
      MontaGridOrigem();
    }
  }
}
