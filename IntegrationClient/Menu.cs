using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegrationClient
{
  public partial class Menu : Form
  {
    public Menu()
    {
      InitializeComponent();
    }

    private void Menu_Load(object sender, EventArgs e)
    {
      Text = string.Format("Robo Ana - Versão {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
      tbarUrl.Text = string.Format(tbarUrl.Text, Program.PersonLogin.Url);
      tbarAccount.Text = string.Format(tbarAccount.Text, Program.PersonLogin.NameAccount);
      tbarPerson.Text = string.Format(tbarPerson.Text, Program.PersonLogin.Name);
    }

    private void PerfilTrocar_Click(object sender, EventArgs e)
    {
      File.Delete(Program.FileConfig);
      // Prepara chamada outro formulário
      Thread t = new Thread(new ThreadStart(CallLogin));
      t.Start();
      Close();
    }
    public static void CallLogin()
    {
      Application.Run(new Login());
    }

    private void PerfilSair_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void ImpColConfig_Click(object sender, EventArgs e)
    {
      ImportacaoConfigurar form = new ImportacaoConfigurar
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }

    private void ImpColImport_Click(object sender, EventArgs e)
    {
      ImportacaoImportar form = new ImportacaoImportar
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }
  }
}
