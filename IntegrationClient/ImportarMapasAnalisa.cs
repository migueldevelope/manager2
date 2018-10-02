using IntegrationService.Api;
using IntegrationService.Tools;
using IntegrationService.Views;
using OracleTools;
using SqlServerTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegrationClient
{
  public partial class ImportarMapasAnalisa : Form
  {
    public ViewPersonLogin Person { get; set; }
    public string Conn { get; set; }
    private ViewIntegrationProcessLevelTwo processLevelTwo { get; set; }

    public ImportarMapasAnalisa()
    {
      InitializeComponent();
    }

    private void BtImp_Click(object sender, EventArgs e)
    {
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/Occupation_cmd.txt", this.Person.IdAccount), txtCmd.Text, false);

      string file = string.Format("{0}/SyncOccupation.csv", this.Person.IdAccount);
      if (File.Exists(file))
        File.Delete(file);

      txtLog.Text = "Status;Occupation;Code;IdOccupation or Message";
      FileClass.SaveLog(file, txtLog.Text);
      DataTable occupations;
      if (Boolean.Parse(Conn.Split(';')[0]))
      {
        OracleConnectionTool cnn = new OracleConnectionTool(Conn.Split(';')[1], Conn.Split(';')[2], Conn.Split(';')[3]);
        occupations = cnn.ExecuteQuery(txtCmd.Text);
        cnn.Close();
      }
      else
      {
        SqlConnectionTool cnn = new SqlConnectionTool(Conn.Split(';')[1], Conn.Split(';')[2], Conn.Split(';')[3], Conn.Split(';')[4]);
        occupations = cnn.ExecuteQuery(txtCmd.Text);
        cnn.Close();
      }
      InfraIntegration infraIntegration = new InfraIntegration(Person);

      ViewIntegrationOccupation cargo;
      foreach (DataRow item in occupations.Rows)
      {
        try
        {
          cargo = infraIntegration.GetOccupationByName(item["nome_cargo"].ToString());
          string registro = string.Format("Ok;{0};{1};{2}", item["nome_cargo"].ToString(), item["cargo"].ToString(), cargo.IdOccupation);
          FileClass.SaveLog(file, registro);
          txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
        }
        catch (Exception ex)
        {
          string registro = string.Format("Erro;{0};{1};{2}", item["nome_cargo"].ToString(), item["cargo"].ToString(), ex.Message.Split('\n')[0]);
          txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
        }
      }
    }

    private void ImportarCargoAnalisa_Load(object sender, EventArgs e)
    {
      if (File.Exists(string.Format("{0}/Occupation_cmd.txt", this.Person.IdAccount)))
        txtCmd.Text = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/Occupation_cmd.txt", this.Person.IdAccount));
    }

    private void BtLoc_Click(object sender, EventArgs e)
    {
      try
      {
        txtLog.Text = string.Empty;
        btImp.Enabled = false;
        processLevelTwo = null;
        InfraIntegration process = new InfraIntegration(Person);
        processLevelTwo = process.GetProcessByName(txtSubProc.Text);
        btImp.Enabled = true;
        txtSubProc.Text = processLevelTwo.Name;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(),"Erro",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }
  }
}
