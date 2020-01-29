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
        serviceConfiguration = new ConfigurationService(Program.PersonLogin);
        Text = "Configuração da Importãção de Colaboradores";
        CboMode.DataSource = Enum.GetValues(typeof(EnumIntegrationMode));
        CboMode.SelectedIndex = CboMode.FindStringExact(serviceConfiguration.Param.Mode.ToString());
        CboVersion.DataSource = Enum.GetValues(typeof(EnumIntegrationVersion));
        CboVersion.SelectedIndex = CboVersion.FindStringExact(serviceConfiguration.Param.Version.ToString());
        CboChave.DataSource = Enum.GetValues(typeof(EnumIntegrationKey));
        CboChave.SelectedIndex = CboChave.FindStringExact(serviceConfiguration.Param.IntegrationKey.ToString());
        txtDatCul.Text = string.IsNullOrEmpty(serviceConfiguration.Param.CultureDate) ? "en-US" : serviceConfiguration.Param.CultureDate;
        CboDatabaseType.DataSource = Enum.GetValues(typeof(EnumDatabaseType));
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message,"Erro",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }

    private void CboMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (CboMode.SelectedItem.ToString().StartsWith("DataBase"))
      {
        grpBD.Visible = true;
        grpArq.Visible = false;
        grpApi.Visible = false;
        if (serviceConfiguration.Param.ConnectionString != null)
        {
          if (serviceConfiguration.Param.ConnectionString.StartsWith("ODBC"))
          {
            CboDatabaseType.SelectedIndex = CboDatabaseType.FindStringExact(serviceConfiguration.Param.ConnectionString.Split('|')[0]);
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
              CboDatabaseType.SelectedIndex = CboDatabaseType.FindStringExact(serviceConfiguration.Param.ConnectionString.Split(';')[0]);
              txtHostName.Text = serviceConfiguration.Param.ConnectionString.Split(';')[1];
              txtUser.Text = serviceConfiguration.Param.ConnectionString.Split(';')[2];
              txtPassword.Text = serviceConfiguration.Param.ConnectionString.Split(';')[3];
              txtDefault.Text = serviceConfiguration.Param.ConnectionString.Split(';')[4];
            }
          }
        }
        txtSql.Text = serviceConfiguration.Param.SqlCommand;
        txtApiId.Text = string.Empty;
        txtFileName.Text = string.Empty;
      }
      if (CboMode.SelectedItem.ToString().StartsWith("FileCsv"))
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
        txtApiId.Text = string.Empty;
        grpArq.Text = "Arquivo Csv";
      }
      if (CboMode.SelectedItem.ToString().StartsWith("FileExcel"))
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
        txtApiId.Text = string.Empty;
        grpArq.Text = "Microsoft Excel";
      }
      if (CboMode.SelectedItem.ToString().StartsWith("ApplicationInterface"))
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
        txtApiId.Text = serviceConfiguration.Param.ApiIdentification;
        txtApiToken.Text = serviceConfiguration.Param.ApiToken;
        txtApiId.Focus();
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
      if (CboDatabaseType.SelectedItem != null)
      {
        if (CboDatabaseType.SelectedItem.ToString().StartsWith("ODBC"))
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

    private void BtSave_Click(object sender, EventArgs e)
    {
      try
      {
        if (CboDatabaseType.SelectedItem.ToString().StartsWith("ODBC"))
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
          if (string.IsNullOrEmpty(txtDefault.Text) && (EnumDatabaseType)CboDatabaseType.SelectedItem == EnumDatabaseType.SqlServer)
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
        SetParameters();
        serviceConfiguration.Param.ConnectionString = CboDatabaseType.SelectedItem.ToString().Equals("ODBC") ? string.Format("{0}|{1}", CboDatabaseType.SelectedItem, txtStr.Text) : string.Format("{0};{1};{2};{3};{4}", CboDatabaseType.SelectedItem, txtHostName.Text, txtUser.Text, txtPassword.Text, txtDefault.Text);
        serviceConfiguration.Param.SqlCommand = txtSql.Text;
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
        if ((EnumIntegrationMode)CboMode.SelectedItem == EnumIntegrationMode.FileExcel && string.IsNullOrEmpty(txtSheetName.Text))
        {
          txtSheetName.Focus();
          throw new Exception("Informe o nome da planilha de colaboradores.");
        }
        SetParameters();
        serviceConfiguration.Param.FilePathLocal = txtFileName.Text;
        serviceConfiguration.Param.SheetName = txtSheetName.Text;
        serviceConfiguration.SetParameter(serviceConfiguration.Param);
        MessageBox.Show("Parâmetro atualizado!",Text, MessageBoxButtons.OK,MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message,Text, MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }

    private void BtSaveApi_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtApiId.Text))
        {
          txtApiId.Focus();
          throw new Exception("Identificação da API customizada deve ser informada.");
        }
        if (!txtApiId.Text.ToUpper().Equals("UNIMEDNERS"))
        {
          txtApiId.Focus();
          throw new Exception("Identificação inválida, entre em contato com o suporte e solicite um ID de aplicação correto.");
        }
        SetParameters();
        serviceConfiguration.Param.ApiIdentification = txtApiId.Text;
        serviceConfiguration.Param.ApiToken = txtApiToken.Text;
        serviceConfiguration.SetParameter(serviceConfiguration.Param);
        MessageBox.Show("Parâmetro atualizado!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void SetParameters()
    {
      // Banco de Dados
      serviceConfiguration.Param.ConnectionString = string.Empty;
      serviceConfiguration.Param.SqlCommand = string.Empty;
      // File
      serviceConfiguration.Param.FilePathLocal = string.Empty;
      serviceConfiguration.Param.SheetName = string.Empty;
      // API
      serviceConfiguration.Param.ApiIdentification = string.Empty;
      // Commun fields
      serviceConfiguration.Param.Version = (EnumIntegrationVersion)CboVersion.SelectedItem;
      serviceConfiguration.Param.Mode = (EnumIntegrationMode)CboMode.SelectedItem;
      serviceConfiguration.Param.IntegrationKey = (EnumIntegrationKey)CboChave.SelectedItem;
      serviceConfiguration.Param.CultureDate = txtDatCul.Text;
    }
  }
}
