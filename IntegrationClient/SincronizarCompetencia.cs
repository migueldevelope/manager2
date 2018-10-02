using IntegrationService.Api;
using IntegrationService.Tools;
using IntegrationService.Views;
using OracleTools;
using SqlServerTools;
using System;
using System.Data;
using System.IO;
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
        if (File.Exists(file))
          File.Delete(file);

        txtLog.Text = "Status;Skill;Code;Type;IdSkill or Message";
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
        foreach (DataRow item in skills.Rows)
        {
          try
          {
            trabalho = skillIntegration.GetSkillByName(item["nome_competencia"].ToString());
            string registro = string.Format("Ok;{0};{1};{2};{3}", item["nome_competencia"].ToString(), item["codigo"].ToString(), item["tipo"].ToString(), trabalho.IdSkill);
            FileClass.SaveLog(file, registro);
            txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
          }
          catch (Exception ex)
          {
            if (change)
            {
              ViewIntegrationSkill newSkill = new ViewIntegrationSkill()
              {
                Name = item["nome_competencia"].ToString().ToUpper(),
                Concept = item["conceito"].ToString(),
                TypeSkill = Int16.Parse(item["tipo"].ToString())
              };
              newSkill = skillIntegration.AddSkill(newSkill);
              string registro = string.Format("Ok;{0};{1};{2};{3}", item["nome_competencia"].ToString(), item["codigo"].ToString(), item["tipo"].ToString(), newSkill.IdSkill);
              FileClass.SaveLog(file, registro);
              txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
            }
            else
            {
              string registro = string.Format("Erro;{0};{1};{2};{3}", item["nome_competencia"].ToString(), item["codigo"].ToString(), item["tipo"].ToString(), ex.Message.Split('\n')[0]);
              txtLog.Text = string.Concat(txtLog.Text, registro, Environment.NewLine);
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
