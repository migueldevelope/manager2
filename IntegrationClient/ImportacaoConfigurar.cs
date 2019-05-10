using IntegrationService.Enumns;
using IntegrationService.Service;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using System;
using System.IO;
using System.Windows.Forms;

namespace IntegrationClient
{
  public partial class ImportacaoConfigurar : Form
  {
    private ConfigurationService serviceConfiguration;

    public ImportacaoConfigurar()
    {
      InitializeComponent();
    }

    private void ImportacaoConfigurar_Load(object sender, EventArgs e)
    {
      try
      {
        Text = "Configuração da Importãção de Colaboradores";

        serviceConfiguration = new ConfigurationService(Program.PersonLogin);

        cboDatabaseType.DataSource = Enum.GetValues(typeof(EnumDatabaseType));
        cboProc.DataSource = Enum.GetValues(typeof(EnumIntegrationProcess));
        cboType.DataSource = Enum.GetValues(typeof(EnumIntegrationType));
        cboMode.DataSource = Enum.GetValues(typeof(EnumIntegrationMode));

        cboProc.SelectedIndex = cboProc.FindStringExact(serviceConfiguration.Param.Process.ToString());
        cboType.SelectedIndex = cboType.FindStringExact(serviceConfiguration.Param.Type.ToString());
        cboMode.SelectedIndex = cboMode.FindStringExact(serviceConfiguration.Param.Mode.ToString());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message,"Erro",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }

    private void CboMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cboMode.SelectedItem.ToString().StartsWith("DataBase"))
      {
        grpBD.Visible = true;
        grpArq.Visible = false;
        grpApi.Visible = false;
        if (serviceConfiguration.Param.ConnectionString != null)
        {
          if (serviceConfiguration.Param.ConnectionString.StartsWith("ODBC"))
          {
            cboDatabaseType.SelectedIndex = cboDatabaseType.FindStringExact(serviceConfiguration.Param.ConnectionString.Split('|')[0]);
            txtStr.Text = serviceConfiguration.Param.ConnectionString.Split('|')[1];
            lblHostName.Visible = false;
            txtHostName.Visible = false;
            lblUser.Visible = false;
            txtUser.Visible = false;
            lblPassword.Visible = false;
            txtPassword.Visible = false;
            lblDefault.Visible = false;
            txtDefault.Visible = false;
            txtHostName.Text = string.Empty;
            txtUser.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtDefault.Text = string.Empty;
            lblDefault.Visible = false;
            txtDefault.Visible = false;
            lblStr.Visible = true;
            txtStr.Visible = true;
          }
          else
          {
            lblHostName.Visible = true;
            txtHostName.Visible = true;
            lblUser.Visible = true;
            txtUser.Visible = true;
            lblPassword.Visible = true;
            txtPassword.Visible = true;
            lblDefault.Visible = true;
            txtDefault.Visible = true;
            lblStr.Visible = false;
            txtStr.Visible = false;
            txtStr.Text = string.Empty;
            if (!string.IsNullOrEmpty(serviceConfiguration.Param.ConnectionString))
            {
              cboDatabaseType.SelectedIndex = cboDatabaseType.FindStringExact(serviceConfiguration.Param.ConnectionString.Split(';')[0]);
              txtHostName.Text = serviceConfiguration.Param.ConnectionString.Split(';')[1];
              txtUser.Text = serviceConfiguration.Param.ConnectionString.Split(';')[2];
              txtPassword.Text = serviceConfiguration.Param.ConnectionString.Split(';')[3];
              txtDefault.Text = serviceConfiguration.Param.ConnectionString.Split(';')[4];
            }
          }
        }
        txtSql.Text = serviceConfiguration.Param.SqlCommand;
        txtIdApi.Text = string.Empty;
        txtFileName.Text = string.Empty;
      }
      if (cboMode.SelectedItem.ToString().StartsWith("FileCsv"))
      {
        grpArq.Visible = true;
        grpBD.Visible = false;
        grpApi.Visible = false;
        txtHostName.Text = string.Empty;
        txtUser.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtDefault.Text = string.Empty;
        txtSql.Text = string.Empty;
        txtFileName.Text = serviceConfiguration.Param.FilePathLocal;
        lblSheetName.Visible = false;
        txtSheetName.Visible = false;
        txtSheetName.Text = string.Empty;
        txtIdApi.Text = string.Empty;
        grpArq.Text = "Arquivo Csv";
      }
      if (cboMode.SelectedItem.ToString().StartsWith("FileExcel"))
      {
        grpArq.Visible = true;
        grpBD.Visible = false;
        grpApi.Visible = false;
        txtHostName.Text = string.Empty;
        txtUser.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtDefault.Text = string.Empty;
        txtSql.Text = string.Empty;
        lblSheetName.Visible = true;
        txtSheetName.Visible = true;
        txtFileName.Text = serviceConfiguration.Param.FilePathLocal;
        txtSheetName.Text = serviceConfiguration.Param.SheetName;
        txtIdApi.Text = string.Empty;
        grpArq.Text = "Arquivo Microsoft Excel";
      }
      if (cboMode.SelectedItem.ToString().StartsWith("Application"))
      {
        grpArq.Visible = false;
        grpBD.Visible = false;
        grpApi.Visible = true;
        txtHostName.Text = string.Empty;
        txtUser.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtDefault.Text = string.Empty;
        txtSql.Text = string.Empty;
        lblSheetName.Visible = true;
        txtSheetName.Visible = true;
        txtFileName.Text = string.Empty;
        txtSheetName.Text = string.Empty;
        txtIdApi.Text = serviceConfiguration.Param.ApiIdentification;
        txtIdApi.Focus();
      }
    }

    private void BtSave_Click(object sender, EventArgs e)
    {
      try
      {
        if (cboDatabaseType.SelectedItem.ToString().StartsWith("ODBC"))
        {
          if (string.IsNullOrEmpty(txtStr.Text))
          {
            txtStr.Focus();
            throw new Exception("String de conexão ODBC deve ser informada!!");
          }
        }
        else
        {
          if (string.IsNullOrEmpty(txtHostName.Text))
          {
            txtHostName.Focus();
            throw new Exception("Informe o nome do servidor!!");
          }
          if (string.IsNullOrEmpty(txtUser.Text))
          {
            txtUser.Focus();
            throw new Exception("Informe o usuário de conexão!!");
          }
          if (string.IsNullOrEmpty(txtPassword.Text))
          {
            txtPassword.Focus();
            throw new Exception("Informe a senha de conexão!!");
          }
          if (string.IsNullOrEmpty(txtDefault.Text) && (EnumDatabaseType)cboDatabaseType.SelectedItem == EnumDatabaseType.SqlServer)
          {
            txtDefault.Focus();
            throw new Exception("Informe o nome do banco de dados padrão!!");
          }
          if (string.IsNullOrEmpty(txtSql.Text))
          {
            txtSql.Focus();
            throw new Exception("Informe o comando para retornar a lista de colaboradores!!");
          }
        }
        serviceConfiguration.Param.ConnectionString = cboDatabaseType.SelectedItem.ToString().Equals("ODBC") ? string.Format("{0}|{1}", cboDatabaseType.SelectedItem, txtStr.Text) : string.Format("{0};{1};{2};{3};{4}", cboDatabaseType.SelectedItem, txtHostName.Text, txtUser.Text, txtPassword.Text, txtDefault.Text);
        serviceConfiguration.Param.FilePathLocal = txtFileName.Text;
        serviceConfiguration.Param.SqlCommand = txtSql.Text;
        serviceConfiguration.Param.SheetName = string.Empty;
        serviceConfiguration.Param.Process = (EnumIntegrationProcess)cboProc.SelectedItem;
        serviceConfiguration.Param.Mode = (EnumIntegrationMode)cboMode.SelectedItem;
        serviceConfiguration.Param.Type = (EnumIntegrationType)cboType.SelectedItem;
        serviceConfiguration.SetParameter(serviceConfiguration.Param);
        MessageBox.Show("Parâmetro atualizado!",Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message,Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }

    private void BtSaveFile_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtFileName.Text))
        {
          txtFileName.Focus();
          throw new Exception("O nome do arquivo deve ser informado.");
        }
        if (!File.Exists(txtFileName.Text))
        {
          txtFileName.Focus();
          throw new Exception("O arquivo informado deve existir.");
        }
        if ((EnumIntegrationMode)cboMode.SelectedItem == EnumIntegrationMode.FileExcelV1 && string.IsNullOrEmpty(txtSheetName.Text))
        {
          txtSheetName.Focus();
          throw new Exception("Informe o nome da planilha de colaboradores.");
        }
        serviceConfiguration.Param.ConnectionString = string.Empty;
        serviceConfiguration.Param.FilePathLocal = txtFileName.Text;
        serviceConfiguration.Param.Process = (EnumIntegrationProcess)cboProc.SelectedItem;
        serviceConfiguration.Param.Mode = (EnumIntegrationMode)cboMode.SelectedItem;
        serviceConfiguration.Param.Type = (EnumIntegrationType)cboType.SelectedItem;
        serviceConfiguration.Param.SqlCommand = string.Empty;
        serviceConfiguration.Param.SheetName = txtSheetName.Text;
        serviceConfiguration.SetParameter(serviceConfiguration.Param);
        MessageBox.Show("Parâmetro atualizado!",Text, MessageBoxButtons.OK,MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message,Text, MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }

    private void CboDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
    {
      lblHostName.Visible = true;
      txtHostName.Visible = true;
      lblUser.Visible = true;
      txtUser.Visible = true;
      lblPassword.Visible = true;
      txtPassword.Visible = true;
      lblDefault.Visible = true;
      txtDefault.Visible = true;
      lblStr.Visible = false;
      txtStr.Visible = false;
      txtStr.Text = string.Empty;
      if (cboDatabaseType.SelectedItem != null)
      {
        if (cboDatabaseType.SelectedItem.ToString().StartsWith("ODBC"))
        {
          lblHostName.Visible = false;
          txtHostName.Visible = false;
          lblUser.Visible = false;
          txtUser.Visible = false;
          lblPassword.Visible = false;
          txtPassword.Visible = false;
          lblDefault.Visible = false;
          txtDefault.Visible = false;
          txtHostName.Text = string.Empty;
          txtUser.Text = string.Empty;
          txtPassword.Text = string.Empty;
          txtDefault.Text = string.Empty;
          lblDefault.Visible = false;
          txtDefault.Visible = false;
          lblStr.Visible = true;
          txtStr.Visible = true;
        }
      }
    }

    private void CboProc_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cboProc.SelectedItem.ToString().StartsWith("Application"))
      {
        cboType.Visible = false;
        cboMode.Visible = false;
        grpBD.Visible = false;
        grpArq.Visible = false;
        label3.Visible = false;
        label2.Visible = false;
        grpApi.Visible = true;
      }
      else
      {
        cboType.Visible = true;
        cboMode.Visible = true;
        label3.Visible = true;
        label2.Visible = true;
        grpApi.Visible = false;
      }
    }

    private void BtSaveApi_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtIdApi.Text))
        {
          txtIdApi.Focus();
          throw new Exception("Identificação da API customizada deve ser informada.");
        }
        if (!txtIdApi.Text.ToUpper().Equals("UNIMEDNERS"))
        {
          txtIdApi.Focus();
          throw new Exception("Identificação inválida, entre em contato com o suporte e solicite um ID de aplicação correto.");
        }
        serviceConfiguration.Param.ConnectionString = string.Empty;
        serviceConfiguration.Param.FilePathLocal = string.Empty;
        serviceConfiguration.Param.Process = (EnumIntegrationProcess)cboProc.SelectedItem;
        serviceConfiguration.Param.Mode = (EnumIntegrationMode)cboMode.SelectedItem;
        serviceConfiguration.Param.Type = (EnumIntegrationType)cboType.SelectedItem;
        serviceConfiguration.Param.SqlCommand = string.Empty;
        serviceConfiguration.Param.SheetName = string.Empty;
        serviceConfiguration.Param.ApiIdentification = txtIdApi.Text;
        serviceConfiguration.SetParameter(serviceConfiguration.Param);
        MessageBox.Show("Parâmetro atualizado!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
