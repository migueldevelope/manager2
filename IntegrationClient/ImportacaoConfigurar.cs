﻿using IntegrationService.Enumns;
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
        cboMode.DataSource = Enum.GetValues(typeof(EnumIntegrationMode));
        cboType.DataSource = Enum.GetValues(typeof(EnumIntegrationType));

        cboProc.SelectedIndex = cboProc.FindStringExact(serviceConfiguration.Param.Process.ToString());
        cboMode.SelectedIndex = cboMode.FindStringExact(serviceConfiguration.Param.Mode.ToString());
        cboType.SelectedIndex = cboType.FindStringExact(serviceConfiguration.Param.Type.ToString());
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
        if (!string.IsNullOrEmpty(serviceConfiguration.Param.ConnectionString))
        {
          cboDatabaseType.SelectedIndex = cboDatabaseType.FindStringExact(serviceConfiguration.Param.ConnectionString.Split(';')[0]);
          txtHostName.Text = serviceConfiguration.Param.ConnectionString.Split(';')[1];
          txtUser.Text = serviceConfiguration.Param.ConnectionString.Split(';')[2];
          txtPassword.Text = serviceConfiguration.Param.ConnectionString.Split(';')[3];
          txtDefault.Text = serviceConfiguration.Param.ConnectionString.Split(';')[4];
        }
        txtSql.Text = serviceConfiguration.Param.SqlCommand;
        grpArq.Visible = false;
        txtFileName.Text = string.Empty;
      }
      if (cboMode.SelectedItem.ToString().StartsWith("FileCsv"))
      {
        grpArq.Visible = true;
        grpBD.Visible = false;
        txtHostName.Text = string.Empty;
        txtUser.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtDefault.Text = string.Empty;
        txtSql.Text = string.Empty;
        txtFileName.Text = serviceConfiguration.Param.FilePathLocal;
        grpArq.Text = "Arquivo Csv";
      }
      if (cboMode.SelectedItem.ToString().StartsWith("FileExcel"))
      {
        grpArq.Visible = true;
        grpBD.Visible = false;
        txtHostName.Text = string.Empty;
        txtUser.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtDefault.Text = string.Empty;
        txtSql.Text = string.Empty;
        txtFileName.Text = serviceConfiguration.Param.FilePathLocal;
        grpArq.Text = "Arquivo Microsoft Excel";
      }
    }

    private void BtSave_Click(object sender, EventArgs e)
    {
      try
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
        serviceConfiguration.SetParameter(new ViewIntegrationParameterMode()
        {
          ConnectionString = string.Format("{0};{1};{2};{3};{4}", cboDatabaseType.SelectedItem, txtHostName.Text, txtUser.Text, txtPassword.Text, txtDefault.Text),
          FilePathLocal = txtFileName.Text,
          Process = (EnumIntegrationProcess)cboProc.SelectedItem,
          Mode = (EnumIntegrationMode)cboMode.SelectedItem,
          Type = (EnumIntegrationType)cboType.SelectedItem,
          SqlCommand = txtSql.Text
        });
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
        serviceConfiguration.SetParameter(new ViewIntegrationParameterMode()
        {
          ConnectionString = string.Empty,
          FilePathLocal = txtFileName.Text,
          Process = (EnumIntegrationProcess)cboProc.SelectedItem,
          Mode = (EnumIntegrationMode)cboMode.SelectedItem,
          Type = (EnumIntegrationType)cboType.SelectedItem,
          SqlCommand = string.Empty
        });
        MessageBox.Show("Parâmetro atualizado!",Text, MessageBoxButtons.OK,MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message,Text, MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }
  }
}
