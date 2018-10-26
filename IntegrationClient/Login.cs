using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using IntegrationService.Tools;

namespace IntegrationClient
{
  public partial class Login : Form
  {
    private string fileConfig = "config.txt";
    public Login()
    {
      InitializeComponent();
    }

    private void Login_Load(object sender, EventArgs e)
    {
      lblVer.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      if (System.IO.File.Exists(fileConfig ))
      {
        Thread t = new Thread(new ThreadStart(CallMenuPrincipal));
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
        auth.Connect(txtUrl.Text, txtEma.Text, txtSen.Text);

        // Se estiver OK preparar o objeto de persistência local
        string person = JsonConvert.SerializeObject(auth.Person);
        FileClass.WriteToBinaryFile<string>(fileConfig, person, false);

        // Prepara chamada outro formulário
        Thread t = new Thread(new ThreadStart(CallMenuPrincipal));
        t.Start();
        this.Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    public static void CallMenuPrincipal()
    {
      Application.Run(new MenuPrincipal());
    }
  }
}
