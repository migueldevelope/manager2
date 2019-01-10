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
using Newtonsoft.Json;
using System.Threading;
using IntegrationService.Tools;
using IntegrationService.Data;
using IntegrationService.Service;
using IntegrationService.Enumns;
using System.Reflection;
using Manager.Views.Integration;
using Manager.Views.Enumns;

namespace IntegrationClient
{
  public partial class MenuPrincipal : Form
  {
    private ViewPersonLogin Person { get; set; }
    private ConfigurationService serviceConfiguration ;

    public MenuPrincipal()
    {
      InitializeComponent();
    }

    private void BtChange_Click(object sender, EventArgs e)
    {
      File.Delete("config.txt");
      // Prepara chamada outro formulário
      Thread t = new Thread(new ThreadStart(CallLogin));
      t.Start();
      this.Close();
    }

    private void MenuPrincipal_Load(object sender, EventArgs e)
    {
      try
      {
        string auth = FileClass.ReadFromBinaryFile<string>("config.txt");
        Person = JsonConvert.DeserializeObject<ViewPersonLogin>(auth);
        txtUrl.Text = Person.Url;
        txtEma.Text = Person.Mail;

        serviceConfiguration = new ConfigurationService(Person);

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
        MessageBox.Show(ex.ToString());
      }

    }
    public static void CallLogin()
    {
      Application.Run(new Login());
    }

    private void BtSyncSkill_Click(object sender, EventArgs e)
    {
      SincronizarCompetencia form = new SincronizarCompetencia(){
        Person = Person,
        Conn = string.Format("{0};{1};{2};{3};{4}", cboDatabaseType.SelectedItem, txtHostName.Text, txtUser.Text, txtPassword.Text, txtDefault.Text)
      };
      form.ShowDialog();
      form.Close();
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
          MessageBox.Show("Informe o nome do servidor!!");
          txtHostName.Focus();
          return;
        }
        if (string.IsNullOrEmpty(txtUser.Text))
        {
          MessageBox.Show("Informe o usuário de conexão!!");
          txtUser.Focus();
          return;
        }
        if (string.IsNullOrEmpty(txtPassword.Text))
        {
          MessageBox.Show("Informe a senha de conexão!!");
          txtPassword.Focus();
          return;
        }
        if (string.IsNullOrEmpty(txtDefault.Text) && (EnumDatabaseType)cboDatabaseType.SelectedItem == EnumDatabaseType.SqlServer)
        {
          MessageBox.Show("Informe o nome do banco de dados padrão!!");
          txtDefault.Focus();
          return;
        }
        if (string.IsNullOrEmpty(txtSql.Text))
        {
          MessageBox.Show("Informe o comando para retornar a lista de colaboradores!!");
          txtSql.Focus();
          return;
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
        MessageBox.Show("Parâmetro atualizado!");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void BtImp_Click(object sender, EventArgs e)
    {
      try
      {
        ImportService import = new ImportService(Person, serviceConfiguration);
        import.Execute();
        if (import.Status == EnumStatusService.Error)
          MessageBox.Show(import.Message,"Importação",MessageBoxButtons.OK,MessageBoxIcon.Error);
        else
          MessageBox.Show(import.Message,"Importação",MessageBoxButtons.OK,MessageBoxIcon.Information);
      }
      catch (Exception ex) 
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void Button1_Click(object sender, EventArgs e)
    {
      List<ConnectionString> conns = new List<ConnectionString>();
      ConnectionString conn = new ConnectionString("jmserver10", "rhjmsoft", "bti9010", "rhjmsoft_todeschini")
      {
        Sql = "SELECT * FROM JMGERFUN"
      };
      conns.Add(conn);
      conn = new ConnectionString("orcl12", "unimed_ners", "bti9010")
      {
        Sql = "SELECT * FROM JMGERFUN"
      };
      conns.Add(conn);
      foreach (ConnectionString aux in conns)
      {
        DataTable dados = new DataTable();
        GetPersonSystem gps = new GetPersonSystem(aux);
        dados = gps.GetPerson();
        MessageBox.Show(dados.Rows.Count.ToString());
      }
    }

    [STAThread]
    private void BtSearchFile_Click(object sender, EventArgs e)
    {
      try
      {
        txtFileName.Text = @"E:\Analisa2018\Clientes\Layout_Manual_Básico_v1.csv";
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void BtSaveFile_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(txtFileName.Text))
        {
          MessageBox.Show("O nome do arquivo deve ser informado.");
          txtFileName.Focus();
          return;
        }
        if (!File.Exists(txtFileName.Text))
        {
          MessageBox.Show("O arquivo informado deve existir.");
          txtFileName.Focus();
          return;
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
        MessageBox.Show("Parâmetro atualizado!");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void BtCar_Click(object sender, EventArgs e)
    {
      ImportarMapasAnalisa form = new ImportarMapasAnalisa()
      {
        Person = Person,
        Conn = string.Format("{0};{1};{2};{3};{4}", serviceConfiguration.Param.ConnectionString.Split(';')[0].Equals("Oracle"), serviceConfiguration.Param.ConnectionString.Split(';')[1], serviceConfiguration.Param.ConnectionString.Split(';')[2], serviceConfiguration.Param.ConnectionString.Split(';')[3], serviceConfiguration.Param.ConnectionString.Split(';')[4])
      };
      form.ShowDialog();
      form.Close();
    }
  }
}
