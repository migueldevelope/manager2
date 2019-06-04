using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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

    class Account
    {
      public int ID { get; set; }
      public double Balance { get; set; }
    }
    
    private void testeDeLeituraEmExcelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var bankAccounts = new List<Account>
      {
          new Account
          {
              ID = 345,
              Balance = 541.27
          },
          new Account
          {
              ID = 123,
              Balance = -127.44
          }
      };
      var package = new ExcelPackage(new FileInfo(@"C:\Jms\ExcelFile.xlsx"));
      ExcelWorksheet sheet = package.Workbook.Worksheets["teste"];
      sheet.Cells[1,1].Value = "ID";
      sheet.Cells[1,2].Value = "Balance";
      var linha = 2;
      foreach (var ac in bankAccounts)
      {
        sheet.Cells[linha, 1].Value = ac.ID;
        sheet.Cells[linha, 2].Value = ac.Balance;
        if (ac.Balance < 0)
        {
          sheet.Cells[linha, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
          sheet.Cells[linha, 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
          sheet.Cells[linha, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
          sheet.Cells[linha, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
        }
        //ApplicationExcel.ActiveCell.Offset[1, 0].Select();
        linha++;
      }
      package.Save();
    }

    private void ImpAnaCar_Click(object sender, EventArgs e)
    {
      ImportacaoAnalisaCargosMapas form = new ImportacaoAnalisaCargosMapas
      {
        MdiParent = this,
        WindowState = FormWindowState.Normal
      };
      form.Show();
    }
  }
}
