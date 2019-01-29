using System;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using IntegrationService.Tools;
using Manager.Views.Integration;

namespace IntegrationClient
{
  public partial class Login : Form
  {
    public Login()
    {
      InitializeComponent();
    }

    private void Login_Load(object sender, EventArgs e)
    {
      this.Text = string.Format("Login Robo Ana - Versão {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
      if (System.IO.File.Exists(Program.FileConfig))
      {
        // Ler arquivo persistido
        Program.PersonLogin = JsonConvert.DeserializeObject<ViewPersonLogin>(FileClass.ReadFromBinaryFile<string>(Program.FileConfig));
        Thread t = new Thread(new ThreadStart(CallMenu));
        t.Start();
        this.Close();
      }
    }

    private void BtCan_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void BtOk_Click(object sender, EventArgs e)
    {
      try
      {
        // Iniciar Autenticação
        var auth = new IntegrationService.Api.Authentication();
        Program.PersonLogin = auth.Connect(txtUrl.Text, txtEma.Text, txtSen.Text);

        // Se estiver OK preparar o objeto de persistência local
        FileClass.WriteToBinaryFile<string>(Program.FileConfig, JsonConvert.SerializeObject(Program.PersonLogin), false);

        // Prepara chamada outro formulário
        Thread t = new Thread(new ThreadStart(CallMenu));
        t.Start();
        this.Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    public static void CallMenu()
    {
      Application.Run(new Menu());
    }
  }
}
