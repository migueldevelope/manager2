using IntegrationClient.FileModel;
using IntegrationService.Api;
using IntegrationService.Enumns;
using IntegrationService.Tools;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using OracleTools;
using SqlServerTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace IntegrationClient
{
  public partial class ImportacaoAnalisaCargosMapas : Form
  {
    public ImportacaoAnalisaCargosMapas()
    {
      InitializeComponent();
    }

    private void ImportacaoAnalisaCargosMapas_Load(object sender, EventArgs e)
    {
      if (File.Exists(string.Format("{0}/Connection.txt", Program.PersonLogin.IdAccount)))
      {
        Connection conn = FileClass.ReadFromBinaryFile<Connection>(string.Format("{0}/Connection.txt", Program.PersonLogin.IdAccount));
        CboTipBd.Text = conn.BancoDados;
        txtCnxStr.Text = conn.ConnectionString;
        BtnValidCnx_Click(sender, e);
      }
      else
      {
        CboTipBd.Text = "Nenhum";
      }
    }

    private void BtnValidCnx_Click(object sender, EventArgs e)
    {
      try
      {
        DataTable occupations;
        txtCnxStr.Text = txtCnxStr.Text.Replace(" ", string.Empty);
        if (CboTipBd.Text.Equals("Oracle"))
        {
          OracleConnectionTool cnn = new OracleConnectionTool(txtCnxStr.Text.Split(';')[0], txtCnxStr.Text.Split(';')[1], txtCnxStr.Text.Split(';')[2]);
          occupations = cnn.ExecuteQuery("SELECT * FROM JMSYSPAR WHERE ROWNUM<2");
          cnn.Close();
        }
        else
        {
          SqlConnectionTool cnn = new SqlConnectionTool(txtCnxStr.Text.Split(';')[0], txtCnxStr.Text.Split(';')[1], txtCnxStr.Text.Split(';')[2], txtCnxStr.Text.Split(';')[3]);
          occupations = cnn.ExecuteQuery("SELECT TOP 1 * FROM JMSYSPAR");
          cnn.Close();
        }
        if (File.Exists(string.Format("{0}/Connection.txt", Program.PersonLogin.IdAccount)))
          File.Delete(string.Format("{0}/Connection.txt", Program.PersonLogin.IdAccount));

        Connection conn = new Connection
        {
          BancoDados = CboTipBd.Text,
          ConnectionString = txtCnxStr.Text
        };
        FileClass.WriteToBinaryFile(string.Format("{0}/Connection.txt", Program.PersonLogin.IdAccount), conn);

        CarregarGrupoCargo();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    private void CboTipBd_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (CboTipBd.Text)
      {
        case "Oracle":
          lblAjuCnx.Text = "Hostname ; User ; Password";
          break;
        case "SqlServer":
          lblAjuCnx.Text = "Hostname ; User ; Password ; Database Name";
          break;
        default:
          lblAjuCnx.Text = "Selecione o tipo de banco de dados";
          break;
      }
    }

    private void CarregarGrupoCargo()
    {
      grpCar.Enabled = true;
      grpCnx.Enabled = false;

      // Company
      string companyId = string.Empty;
      if (File.Exists(string.Format("{0}/Company.txt", Program.PersonLogin.IdAccount)))
        companyId = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/Company.txt", Program.PersonLogin.IdAccount));

      InfraIntegration infraService = new InfraIntegration(Program.PersonLogin);
      List<ViewListCompany> companys = infraService.GetCompanyList();
      CboCompany.Items.Clear();
      foreach (var item in companys)
      {
        CboCompany.Items.Add(string.Format("{0} - {1}", item.Name, item._id));
        if (companyId.Equals(item._id))
          CboCompany.SelectedIndex = CboCompany.Items.Count - 1;
      }

      string subPrcId = string.Empty;
      if (File.Exists(string.Format("{0}/SubProcesso.txt", Program.PersonLogin.IdAccount)))
        subPrcId = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/SubProcesso.txt", Program.PersonLogin.IdAccount));

      List<ViewListProcessLevelTwo> subProcessos = infraService.GetProcessLevelTwoList();
      CboSubPro.Items.Clear();
      foreach (var item in subProcessos)
      {
        CboSubPro.Items.Add(string.Format("{0} - {1} - {2} - {3}", item.Name, item.ProcessLevelOne.Name, item.ProcessLevelOne.Area.Name, item._id));
        if (subPrcId.Equals(item._id))
          CboSubPro.SelectedIndex = CboSubPro.Items.Count - 1;
      }

      txtCmdCom.Text = "select * from (select 'Soft' tipo, descricao nome, conceito, compor codigo from jmgpecom where empresa = 100 union all select 'Hard' tipo, descricao nome, conceito, tecnica codigo from jmgpetec where empresa = 100) tab order by tab.tipo DESC, tab.nome";
      if (File.Exists(string.Format("{0}/CommandCom.txt", Program.PersonLogin.IdAccount)))
        txtCmdCom.Text = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/CommandCom.txt", Program.PersonLogin.IdAccount));

      txtCmdCar.Text = string.Empty;
      if (File.Exists(string.Format("{0}/CommandCar.txt", Program.PersonLogin.IdAccount)))
        txtCmdCar.Text = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/CommandCar.txt", Program.PersonLogin.IdAccount));
    }

    private void CboSubPro_SelectedIndexChanged(object sender, EventArgs e)
    {
      lblIdSubProc.Text = CboSubPro.Text.Substring(CboSubPro.Text.LastIndexOf(" ") + 1, 24);
      if (File.Exists(string.Format("{0}/SubProcesso.txt", Program.PersonLogin.IdAccount)))
        File.Delete(string.Format("{0}/SubProcesso.txt", Program.PersonLogin.IdAccount));
      FileClass.WriteToBinaryFile(string.Format("{0}/SubProcesso.txt", Program.PersonLogin.IdAccount), lblIdSubProc.Text);
    }
    private void BtnImp_Click(object sender, EventArgs e)
    {
      try
      {
        if (CboSubPro.Items.Count == 0)
        {
          MessageBox.Show("Não tem nenhum sub-processo criado nesta conta!", "Erro de Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
        if (File.Exists(string.Format("{0}/CommandCom.txt", Program.PersonLogin.IdAccount)))
          File.Delete(string.Format("{0}/CommandCom.txt", Program.PersonLogin.IdAccount));
        FileClass.WriteToBinaryFile(string.Format("{0}/CommandCom.txt", Program.PersonLogin.IdAccount), txtCmdCom.Text);

        if (File.Exists(string.Format("{0}/CommandCar.txt", Program.PersonLogin.IdAccount)))
          File.Delete(string.Format("{0}/CommandCar.txt", Program.PersonLogin.IdAccount));
        FileClass.WriteToBinaryFile(string.Format("{0}/CommandCar.txt", Program.PersonLogin.IdAccount), txtCmdCar.Text);

        if (File.Exists(string.Format("{0}/Skill.log", Program.PersonLogin.IdAccount)))
          File.Delete(string.Format("{0}/Skill.log", Program.PersonLogin.IdAccount));

        if (File.Exists(string.Format("{0}/Cargo.log", Program.PersonLogin.IdAccount)))
          File.Delete(string.Format("{0}/Cargo.log", Program.PersonLogin.IdAccount));

        DataTable skills;
        if (CboTipBd.Text.Equals("Oracle"))
        {
          OracleConnectionTool cnn = new OracleConnectionTool(txtCnxStr.Text.Split(';')[0], txtCnxStr.Text.Split(';')[1], txtCnxStr.Text.Split(';')[2]);
          skills = cnn.ExecuteQuery(txtCmdCom.Text);
          cnn.Close();
        }
        else
        {
          SqlConnectionTool cnn = new SqlConnectionTool(txtCnxStr.Text.Split(';')[0], txtCnxStr.Text.Split(';')[1], txtCnxStr.Text.Split(';')[2], txtCnxStr.Text.Split(';')[3]);
          skills = cnn.ExecuteQuery(txtCmdCom.Text);
          cnn.Close();
        }
        InfraIntegration infraService = new InfraIntegration(Program.PersonLogin);
        ViewCrudSkill viewSkill = null;
        prb.Maximum = skills.Rows.Count;
        prb.Value = 0;
        lblPrc.Text = "Integração de Competências Comportamentais/Técnicas";
        Refresh();
        string registro = string.Empty;
        foreach (DataRow item in skills.Rows)
        {
          viewSkill = new ViewCrudSkill
          {
            Name = item["nome"].ToString().ToUpper(),
            Concept = item["conceito"].ToString()
          };
          if (item["tipo"].ToString().Equals("Soft"))
            viewSkill.TypeSkill = EnumTypeSkill.Soft;
          else
            viewSkill.TypeSkill = EnumTypeSkill.Hard;
          registro = string.Format("Erro;{0};{1};{2}", item["nome"].ToString().ToUpper(), item["codigo"].ToString(), item["tipo"].ToString());
          try
          {
            viewSkill = infraService.IntegrationSkill(viewSkill);
            registro = string.Format("Ok;{0};{1};{2};{3}", item["nome"].ToString().ToUpper(), item["codigo"].ToString(), item["tipo"].ToString(), viewSkill._id);
          }
          catch (Exception ex)
          {
            registro = string.Concat(registro, ";", ex.Message);
          }
          FileClass.SaveLog(string.Format("{0}/Skill.log", Program.PersonLogin.IdAccount), registro, EnumTypeLineOpportunityg.Register);
          prb.Value = prb.Value + 1;
          Refresh();
        }
        lblPrc.Text = "Preparando mapa de cargos";
        prb.Value = 0;
        Refresh();
        DataTable occupations;
        if (CboTipBd.Text.Equals("Oracle"))
        {
          OracleConnectionTool cnn = new OracleConnectionTool(txtCnxStr.Text.Split(';')[0], txtCnxStr.Text.Split(';')[1], txtCnxStr.Text.Split(';')[2]);
          occupations = cnn.ExecuteQuery(txtCmdCar.Text);
          cnn.Close();
        }
        else
        {
          SqlConnectionTool cnn = new SqlConnectionTool(txtCnxStr.Text.Split(';')[0], txtCnxStr.Text.Split(';')[1], txtCnxStr.Text.Split(';')[2], txtCnxStr.Text.Split(';')[3]);
          occupations = cnn.ExecuteQuery(txtCmdCar.Text);
          cnn.Close();
        }
        lblPrc.Text = "Importando mapa de cargos";
        prb.Maximum = occupations.Rows.Count;
        Refresh();
        ViewIntegrationProfileOccupation viewProfile = new ViewIntegrationProfileOccupation();
        string nomeCargo = string.Empty;
        string cargo = string.Empty;
        foreach (DataRow item in occupations.Rows)
        {
          if (!nomeCargo.Equals(item["nome_cargo"].ToString().ToUpper()))
          {
            if (!string.IsNullOrEmpty(nomeCargo))
            {
              // Call post
              try
              {
                viewProfile = infraService.IntegrationProfile(viewProfile);
                registro = string.Format("Ok;{0};{1}", nomeCargo, cargo, viewProfile._id);
              }
              catch (Exception ex)
              {
                registro = string.Format("Erro;{0};{1}", nomeCargo, cargo, ex.Message);
              }
              FileClass.SaveLog(string.Format("{0}/Cargo.log", Program.PersonLogin.IdAccount), registro, EnumTypeLineOpportunityg.Register);
            }
            nomeCargo = item["nome_cargo"].ToString().ToUpper();
            cargo = item["cargo"].ToString();
            viewProfile = new ViewIntegrationProfileOccupation
            {
              Name = item["nome_cargo"].ToString(),
              NameGroup = item["nome_grupo_cargo"].ToString(),
              Activities = new List<string>(),
              IdProcessLevelTwo = lblIdSubProc.Text,
              Schooling = new List<string>(),
              SchoolingComplement = new List<string>(),
              Skills = new List<string>(),
              SpecificRequirements = string.Empty,
              IdCompany = lblIdCompany.Text
            };
          }
          switch (short.Parse(item["tipo"].ToString()))
          {
            case 0: // responsabilidades
              viewProfile.Activities.Add(item["conteudo"].ToString());
              break;
            case 1: // comportamental
              viewProfile.Skills.Add(item["nome_compor"].ToString().ToUpper());
              break;
            case 2: // tecnica
              viewProfile.Skills.Add(item["nome_tecnica"].ToString().ToUpper());
              break;
            case 3: // escolaridade
              viewProfile.Schooling.Add(item["nome_escolaridade"].ToString().Trim().ToUpper());
              viewProfile.SchoolingComplement.Add(item["escolacompl"].ToString().Trim());
              break;
            default:
              break;
          }
          prb.Value++;
          Refresh();
        }
        if (!string.IsNullOrEmpty(nomeCargo))
        {
          // Call post
          try
          {
            viewProfile = infraService.IntegrationProfile(viewProfile);
            registro = string.Format("Ok;{0};{1}", nomeCargo, cargo, viewProfile._id);
          }
          catch (Exception ex)
          {
            registro = string.Format("Erro;{0};{1}", nomeCargo, cargo, ex.Message);
          }
          FileClass.SaveLog(string.Format("{0}/Cargo.log", Program.PersonLogin.IdAccount), registro, EnumTypeLineOpportunityg.Register);
        }
        prb.Value = 0;
        Refresh();
        MessageBox.Show("Fim de Importação de Cargos.", "Importação de Cargos e Mapas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Erro de Integração", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void CboCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      lblIdCompany.Text = CboCompany.Text.Substring(CboCompany.Text.LastIndexOf(" ") + 1, 24);
      if (File.Exists(string.Format("{0}/Company.txt", Program.PersonLogin.IdAccount)))
        File.Delete(string.Format("{0}/Company.txt", Program.PersonLogin.IdAccount));
      FileClass.WriteToBinaryFile(string.Format("{0}/Company.txt", Program.PersonLogin.IdAccount), lblIdCompany.Text);
    }
  }
}
