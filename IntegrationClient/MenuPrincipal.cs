using IntegrationService.Views;
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
using IntegrationService.Core;
using IntegrationService.Tools;
using IntegrationService.Data;
using IntegrationService.Service;

namespace IntegrationClient
{
  public partial class MenuPrincipal : Form
  {
    private ViewPersonLogin Person { get; set; }

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

        if (File.Exists(string.Format("{0}/conn.txt", Person.IdAccount)))
        {
          string conn = FileClass.ReadFromBinaryFile<string>(string.Format("{0}/conn.txt", Person.IdAccount)).Replace("\"",string.Empty);
          chkOra.Checked = Boolean.Parse(conn.Split(';')[0]);
          txtHostName.Text = conn.Split(';')[1];
          txtUser.Text = conn.Split(';')[2];
          txtPassword.Text = conn.Split(';')[3];
          txtDefault.Text = conn.Split(';')[4];
        }
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

    private void BtImpMap_Click(object sender, EventArgs e)
    {
      ImportarMapasAnalisa form = new ImportarMapasAnalisa(){
        Person = Person,
        Conn = string.Format("{0};{1};{2};{3};{4}", chkOra.Checked, txtHostName.Text, txtUser.Text, txtPassword.Text, txtDefault.Text)
      };
      form.ShowDialog();
      form.Close();
    }

    private void BtSyncSkill_Click(object sender, EventArgs e)
    {
      SincronizarCompetencia form = new SincronizarCompetencia(){
        Person = Person,
        Conn = string.Format("{0};{1};{2};{3};{4}", chkOra.Checked, txtHostName.Text, txtUser.Text, txtPassword.Text, txtDefault.Text)
      };
      form.ShowDialog();
      form.Close();
    }

    private void BtSaveCnx_Click(object sender, EventArgs e)
    {
      if (!Directory.Exists(Person.IdAccount))
        Directory.CreateDirectory(Person.IdAccount);

      string person = JsonConvert.SerializeObject(string.Format("{0};{1};{2};{3};{4}",chkOra.Checked,txtHostName.Text,txtUser.Text,txtPassword.Text,txtDefault.Text));
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/conn.txt",Person.IdAccount), person, false);

      MessageBox.Show("Configuração atualizada!");
    }

    private void Button2_Click(object sender, EventArgs e)
    {
      List<Colaborador> lista = new List<Colaborador>();
      Colaborador colaborador = new Colaborador()
      {
        Empresa = "100",
        NomeEmpresa = "Analisa",
        Estabelecimento = "1",
        NomeEstabelecimento = "Analisa",
        Documento ="57763771020",
        Matricula = 1,
        Nome = "Juremir Milani Novo",
        Email = "juremir@jmsoft.com.br",
        DataNascimento = new DateTime(1971,07,01),
        Celular = "54 991089092",
        Telefone = "54 32025412",
        Identidade = "8049471331",
        CarteiraProfissional = "cart_prof",
        Sexo = "Masculino",
        DataAdmissao = new DateTime(1994,12,15),
        Situacao = "Ativo",
        DataRetornoFerias = null,
        MotivoAfastamento =  string.Empty,
        DataDemissao = null,
        Cargo = "2043",
        NomeCargo  = "Analista de Sistemas",
        DataUltimaTrocaCargo = null,
        GrauInstrucao = "15",
        NomeGrauInstrucao = "Superior Incompleto",
        SalarioNominal = 4130,
        DataUltimoReajuste = null,
        DocumentoChefe = string.Empty,
        EmpresaChefe = string.Empty,
        NomeEmpresaChefe = string.Empty,
        MatriculaChefe = 0,
        NomeChefe = string.Empty
      };
      lista.Add(colaborador);
      ImportService import = new ImportService(Person, lista);
      MessageBox.Show(import.Message);
    }
  }
}
