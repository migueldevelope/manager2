using IntegrationService.Api;
using IntegrationService.Enumns;
using IntegrationService.Tools;
using Manager.Views.BusinessList;
using Manager.Views.Integration;
using Newtonsoft.Json;
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
    private ViewListProcessLevelTwo ProcessLevelTwo { get; set; }

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

      txtLog.Text = string.Format("Status;Occupation;Code;IdOccupation or Message{0}",Environment.NewLine);
      FileClass.SaveLog(file, txtLog.Text, EnumTypeLineOpportunityg.Register);
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

      prb.Maximum = occupations.Rows.Count;
      prb.Minimum = 0;
      prb.Value = 0;
      this.Refresh();
      string salvaGrupoCargo = string.Empty;
      string salvaCargo = string.Empty;
      string salvaCargoCodigo = string.Empty;
      //ViewIntegrationGroup grupoCargo = null;
      ViewIntegrationProfileOccupation cargo = null;
      Boolean novoCargo = false;
      foreach (DataRow item in occupations.Rows)
      {
        try
        {
          if (!salvaGrupoCargo.Equals(item["nome_grupo_cargo"].ToString().Trim().ToUpper()))
          {
            if (novoCargo)
            {
              // gravar cargo
//              cargo = infraIntegration.AddOccupation(cargo);
              //string registro = string.Format("Ok;{0};{1};{2}", salvaCargo, salvaCargoCodigo, string.Format("Novo cargo, {0}", cargo.IdOccupation));
              //FileClass.SaveLog(file, registro, EnumTypeLineOpportunityg.Register);
              //txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
              novoCargo = false;
            }
            lblGrpCar.Text = item["nome_grupo_cargo"].ToString();
            this.Refresh();
//            grupoCargo = infraIntegration.GetGroupByName(ProcessLevelTwo.IdCompany, item["nome_grupo_cargo"].ToString().Trim());
//            salvaGrupoCargo = grupoCargo.Name.ToUpper();
          }
          if (!salvaCargo.Equals(item["nome_cargo"].ToString().Trim().ToUpper()))
          {
            try
            {
              if (novoCargo)
              {
                // gravar cargo
//                cargo = infraIntegration.AddOccupation(cargo);
//                string registroNovo = string.Format("Ok;{0};{1};{2}", salvaCargo, salvaCargoCodigo, string.Format("Novo cargo, {0}", cargo.IdOccupation));
                //FileClass.SaveLog(file, registroNovo, EnumTypeLineOpportunityg.Register);
                //txtLog.Text = string.Concat(txtLog.Text, registroNovo, Environment.NewLine);
                novoCargo = false;
              }
//              cargo = infraIntegration.GetOccupationByName(ProcessLevelTwo.IdCompany, item["nome_cargo"].ToString().Trim());
//              string registro = string.Format("Ok;{0};{1};{2}", item["nome_cargo"].ToString(), item["cargo"].ToString(), cargo.IdOccupation);
//              FileClass.SaveLog(file, registro, EnumTypeLineOpportunityg.Register);
              //txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
            }
            catch (Exception)
            {
              novoCargo = true;
              cargo = null;
            }
            salvaCargo = item["nome_cargo"].ToString().Trim().ToUpper();
            salvaCargoCodigo = item["cargo"].ToString();
          }
          if (novoCargo)
          {
            //Montar cargo
            if (cargo == null)
            {
              cargo = new ViewIntegrationProfileOccupation()
              {
                Name = item["nome_cargo"].ToString().Trim(),
                //NameGroup = grupoCargo.Name,
                //IdCompany = ProcessLevelTwo.IdCompany,
                //NameCompany = ProcessLevelTwo.NameCompany,
                //IdArea = ProcessLevelTwo.IdArea,
                //NameArea = ProcessLevelTwo.NameArea,
                //IdProcessLevelOne = ProcessLevelTwo.IdProcessLevelOne,
                //NameProcessLevelOne = ProcessLevelTwo.NameProcessLevelOne,
                //IdProcessLevelTwo = ProcessLevelTwo.Id,
                //NameProcessLevelTwo = ProcessLevelTwo.Name,
                Skills = new List<string>(),
                //Schooling = grupoCargo.Schooling,
                SchoolingComplement = new List<string>(),
                Activities = new List<string>(),
                SpecificRequirements = null
              };
              //for (int i = 0; i < grupoCargo.Schooling.Count; i++)
                //cargo.SchoolingComplement.Add(string.Empty);
            }
            switch (Int16.Parse(item["tipo"].ToString()))
            {
              case 0: // responsabilidades
                cargo.Activities.Add(item["conteudo"].ToString());
                break;
              case 1: // comportamental
                cargo.Skills.Add(item["nome_compor"].ToString());
                break;
              case 2: // tecnica
                cargo.Skills.Add(item["nome_tecnica"].ToString());
                break;
              case 3: // escolaridade
                for (int i = 0; i < cargo.Schooling.Count; i++)
                {
                  if (cargo.Schooling[i].Trim().ToUpper().Equals(item["nome_escolaridade"].ToString().Trim().ToUpper()))
                    cargo.SchoolingComplement[i] = item["escolacompl"].ToString();
                }
                break;
              default:
                break;
            }
          }
          prb.Value++;
          this.Refresh();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
          return;
        }
      }
      if (novoCargo)
      {
        // gravar cargo
//        cargo = infraIntegration.AddOccupation(cargo);
        //string registro = string.Format("Ok;{0};{1};{2}", salvaCargo, salvaCargoCodigo, string.Format("Novo cargo, {0}",cargo.IdOccupation));
        //FileClass.SaveLog(file, registro, EnumTypeLineOpportunityg.Register);
        //txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
        novoCargo = false;
      }
      MessageBox.Show("Importação de cargos realizada!");
    }

    private void ImportarCargoAnalisa_Load(object sender, EventArgs e)
    {
      if (File.Exists(string.Format("{0}/Occupation_cmd.txt", this.Person.IdAccount)))
        txtCmd.Text = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/Occupation_cmd.txt", this.Person.IdAccount));
      if (File.Exists(string.Format("{0}/SubProcess_cmd.txt", this.Person.IdAccount))){
        ProcessLevelTwo = JsonConvert.DeserializeObject<ViewListProcessLevelTwo>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/SubProcess_cmd.txt", this.Person.IdAccount)));
        btImp.Enabled = true;
        txtSubProc.Text = ProcessLevelTwo.Name;
      }
    }

    private void BtLoc_Click(object sender, EventArgs e)
    {
      try
      {
        txtLog.Text = string.Empty;
        btImp.Enabled = false;
        ProcessLevelTwo = null;
        InfraIntegration process = new InfraIntegration(Person);
//        ProcessLevelTwo = process.GetProcessByName(txtSubProc.Text);
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/SubProcess_cmd.txt", this.Person.IdAccount), JsonConvert.SerializeObject(ProcessLevelTwo), false);
        btImp.Enabled = true;
        txtSubProc.Text = ProcessLevelTwo.Name;
      }
      catch (Exception ex)
      {
        if (File.Exists(string.Format("{0}/SubProcess_cmd.txt", this.Person.IdAccount)))
          File.Delete(string.Format("{0}/SubProcess_cmd.txt", this.Person.IdAccount));
        MessageBox.Show(ex.ToString(),"Erro",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }
  }
}
