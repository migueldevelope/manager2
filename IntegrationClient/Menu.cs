using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace IntegrationClient
{
  public partial class Menu : Form
  {

    #region Constructor - Load
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
    #endregion

    #region Menu Perfil
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
    #endregion

    #region Menu Import
    private void ImpColCfg_Click(object sender, EventArgs e)
    {
      ImportacaoConfigurar form = new ImportacaoConfigurar
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }
    private void ImpColImp_Click(object sender, EventArgs e)
    {
      ImportacaoImportar form = new ImportacaoImportar
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }
    #endregion

    #region Menu Mapas
    private void ImpCarAna_Click(object sender, EventArgs e)
    {
      ImportacaoAnalisaCargosMapas form = new ImportacaoAnalisaCargosMapas
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }
    private void ImpCarExl_Click(object sender, EventArgs e)
    {
      ImportarMapasExcel form = new ImportarMapasExcel
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }
    #endregion

  }
}
