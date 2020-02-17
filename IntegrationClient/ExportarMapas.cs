using IntegrationService.Api;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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
  public partial class ExportarMapas : Form
  {
    public ExportarMapas()
    {
      InitializeComponent();
    }

    private void BtnExp_Click(object sender, EventArgs e)
    {
      try
      {
        IntegrationApi IntegrationApi  = new IntegrationApi(Program.PersonLogin);
        List<ViewListOccupationResume> occupations = IntegrationApi.OccupationExportList();
        lblPrb.Text = string.Format("Exportando 0 de {0} mapas", occupations.Count);
        prb.Maximum = occupations.Count;
        prb.Minimum = 0;
        Refresh();
        foreach (ViewListOccupationResume occupation in occupations)
        {
          prb.Minimum++;
          lblPrb.Text = string.Format("Exportando {0} de {1} mapas", prb.Minimum, occupations.Count);
          Refresh();
          ViewMapOccupation map = IntegrationApi.OccupationExportProfile(occupation._id);
        }
      }
      catch (Exception)
      {

        throw;
      }
    }
  }
}
