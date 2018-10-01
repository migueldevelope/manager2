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
  public partial class SincronizarCompetencia : Form
  {
    public ViewPersonLogin Person { get; set; }
    public string Conn { get; set; }

    public SincronizarCompetencia()
    {
      InitializeComponent();
    }

    private void BtVal_Click(object sender, EventArgs e)
    {
      Processar(false);
    }

    private void SincronizarCompetencia_Load(object sender, EventArgs e)
    {
      if (File.Exists(string.Format("{0}/Skill_cmd.txt", this.Person.IdAccount)))
        txtCmd.Text = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/Skill_cmd.txt", this.Person.IdAccount));
    }

    private void Processar(bool change)
    {
      try
      {
        FileClass.WriteToBinaryFile<string>(string.Format("{0}/Skill_cmd.txt", this.Person.IdAccount), txtCmd.Text, false);

        string file = string.Format("{0}/SyncSkill.csv", this.Person.IdAccount);
        if (!File.Exists(file))
          File.Delete(file);

        txtLog.Text = string.Format("Status;Skill;Code;Type;IdSkill or Message{0}", Environment.NewLine);
        FileClass.SaveLog(file, txtLog.Text);
        DataTable skills;
        if (Boolean.Parse(Conn.Split(';')[0]))
        {
          OracleConnectionTool cnn = new OracleConnectionTool(Conn.Split(';')[1], Conn.Split(';')[2], Conn.Split(';')[3]);
          skills = cnn.ExecuteQuery(txtCmd.Text);
          cnn.Close();
        }
        else
        {
          SqlConnectionTool cnn = new SqlConnectionTool(Conn.Split(';')[1], Conn.Split(';')[2], Conn.Split(';')[3], Conn.Split(';')[4]);
          skills = cnn.ExecuteQuery(txtCmd.Text);
          cnn.Close();
        }
        InfraIntegration skillIntegration = new InfraIntegration(Person);
        ViewIntegrationSkill trabalho;
        for (int i = 0; i < skills.Rows.Count; i++)
        {
          try
          {
            trabalho = skillIntegration.GetSkillByName(skills.Rows[i]["nome_competencia"].ToString());
            string registro = string.Format("Ok;{0};{1};{2};{3}{4}", skills.Rows[i]["nome_competencia"].ToString(), skills.Rows[i]["codigo"].ToString(), skills.Rows[i]["tipo"].ToString(), trabalho.IdSkill, Environment.NewLine);
            FileClass.SaveLog(file, registro);
            txtLog.Text = string.Concat(txtLog.Text, registro);
          }
          catch (Exception ex)
          {
            if (change)
            {
              ViewIntegrationSkill newSkill = new ViewIntegrationSkill()
              {
                Name = skills.Rows[i]["nome_competencia"].ToString().ToUpper(),
                Concept= skills.Rows[i]["conceito"].ToString(),
                TypeSkill= Int16.Parse(skills.Rows[i]["tipo"].ToString())
              };
              newSkill = skillIntegration.AddSkill(newSkill);
              string registro = string.Format("Ok;{0};{1};{2};{3}{4}", skills.Rows[i]["nome_competencia"].ToString(), skills.Rows[i]["codigo"].ToString(), skills.Rows[i]["tipo"].ToString(), newSkill.IdSkill, Environment.NewLine);
              FileClass.SaveLog(file, registro);
              txtLog.Text = string.Concat(txtLog.Text, registro);
            }
            else
            {
              string registro = string.Format("Erro;{0};{1};{2};{3}{4}", skills.Rows[i]["nome_competencia"].ToString(), skills.Rows[i]["codigo"].ToString(), skills.Rows[i]["tipo"].ToString(), ex.Message.Split('\n')[0], Environment.NewLine);
              txtLog.Text = string.Concat(txtLog.Text, registro);
            }
          }
        }
      }
      catch (Exception ex)
      {
        txtLog.Text = ex.ToString();
      }
    }

    private void BtAtu_Click(object sender, EventArgs e)
    {
      Processar(true);
    }
  }
}
