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
      lblVer.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    private void BtCan_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void BtOk_Click(object sender, EventArgs e)
    {
      var login = new IntegrationService.Api.Authentication;
      login.ValidAuthentication(txtUrl.Text, txtEma.Text, txtSen.Text);
    }
  }
}
