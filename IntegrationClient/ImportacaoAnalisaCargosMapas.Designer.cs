namespace IntegrationClient
{
  partial class ImportacaoAnalisaCargosMapas
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.txtCnxStr = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.grpCnx = new System.Windows.Forms.GroupBox();
      this.lblAjuCnx = new System.Windows.Forms.Label();
      this.btnValidCnx = new System.Windows.Forms.Button();
      this.cboTipBd = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.grpCar = new System.Windows.Forms.GroupBox();
      this.lblIdCompany = new System.Windows.Forms.Label();
      this.cboCompany = new System.Windows.Forms.ComboBox();
      this.lblCompany = new System.Windows.Forms.Label();
      this.txtCmdCar = new System.Windows.Forms.TextBox();
      this.lblCmdCar = new System.Windows.Forms.Label();
      this.txtCmdCom = new System.Windows.Forms.TextBox();
      this.lblCmdCom = new System.Windows.Forms.Label();
      this.lblIdSubProc = new System.Windows.Forms.Label();
      this.prb = new System.Windows.Forms.ProgressBar();
      this.lblPrc = new System.Windows.Forms.Label();
      this.cboSubPro = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnImp = new System.Windows.Forms.Button();
      this.grpCnx.SuspendLayout();
      this.grpCar.SuspendLayout();
      this.SuspendLayout();
      // 
      // txtCnxStr
      // 
      this.txtCnxStr.Location = new System.Drawing.Point(177, 54);
      this.txtCnxStr.Multiline = true;
      this.txtCnxStr.Name = "txtCnxStr";
      this.txtCnxStr.Size = new System.Drawing.Size(712, 71);
      this.txtCnxStr.TabIndex = 4;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(6, 53);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(124, 17);
      this.label8.TabIndex = 3;
      this.label8.Text = "String de Conexão";
      // 
      // grpCnx
      // 
      this.grpCnx.Controls.Add(this.lblAjuCnx);
      this.grpCnx.Controls.Add(this.btnValidCnx);
      this.grpCnx.Controls.Add(this.cboTipBd);
      this.grpCnx.Controls.Add(this.txtCnxStr);
      this.grpCnx.Controls.Add(this.label1);
      this.grpCnx.Controls.Add(this.label8);
      this.grpCnx.Location = new System.Drawing.Point(7, 12);
      this.grpCnx.Name = "grpCnx";
      this.grpCnx.Size = new System.Drawing.Size(895, 164);
      this.grpCnx.TabIndex = 0;
      this.grpCnx.TabStop = false;
      this.grpCnx.Text = "Configuração de Acesso Base de Dados Analisa";
      // 
      // lblAjuCnx
      // 
      this.lblAjuCnx.Location = new System.Drawing.Point(362, 24);
      this.lblAjuCnx.Name = "lblAjuCnx";
      this.lblAjuCnx.Size = new System.Drawing.Size(527, 24);
      this.lblAjuCnx.TabIndex = 6;
      this.lblAjuCnx.Text = "Ajuda para conexão";
      // 
      // btnValidCnx
      // 
      this.btnValidCnx.Location = new System.Drawing.Point(756, 131);
      this.btnValidCnx.Name = "btnValidCnx";
      this.btnValidCnx.Size = new System.Drawing.Size(133, 23);
      this.btnValidCnx.TabIndex = 5;
      this.btnValidCnx.Text = "Validar Conexão";
      this.btnValidCnx.UseVisualStyleBackColor = true;
      this.btnValidCnx.Click += new System.EventHandler(this.BtnValidCnx_Click);
      // 
      // cboTipBd
      // 
      this.cboTipBd.FormattingEnabled = true;
      this.cboTipBd.Items.AddRange(new object[] {
            "Nenhum",
            "Oracle",
            "SqlServer"});
      this.cboTipBd.Location = new System.Drawing.Point(177, 24);
      this.cboTipBd.Name = "cboTipBd";
      this.cboTipBd.Size = new System.Drawing.Size(179, 24);
      this.cboTipBd.TabIndex = 2;
      this.cboTipBd.SelectedIndexChanged += new System.EventHandler(this.CboTipBd_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 27);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(165, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Tipo de Banco de Dados";
      // 
      // grpCar
      // 
      this.grpCar.Controls.Add(this.lblIdCompany);
      this.grpCar.Controls.Add(this.cboCompany);
      this.grpCar.Controls.Add(this.lblCompany);
      this.grpCar.Controls.Add(this.txtCmdCar);
      this.grpCar.Controls.Add(this.lblCmdCar);
      this.grpCar.Controls.Add(this.txtCmdCom);
      this.grpCar.Controls.Add(this.lblCmdCom);
      this.grpCar.Controls.Add(this.lblIdSubProc);
      this.grpCar.Controls.Add(this.prb);
      this.grpCar.Controls.Add(this.lblPrc);
      this.grpCar.Controls.Add(this.cboSubPro);
      this.grpCar.Controls.Add(this.label2);
      this.grpCar.Controls.Add(this.btnImp);
      this.grpCar.Enabled = false;
      this.grpCar.Location = new System.Drawing.Point(7, 182);
      this.grpCar.Name = "grpCar";
      this.grpCar.Size = new System.Drawing.Size(895, 358);
      this.grpCar.TabIndex = 6;
      this.grpCar.TabStop = false;
      this.grpCar.Text = "Cargos e Mapas";
      // 
      // lblIdCompany
      // 
      this.lblIdCompany.AutoSize = true;
      this.lblIdCompany.Location = new System.Drawing.Point(108, 24);
      this.lblIdCompany.Name = "lblIdCompany";
      this.lblIdCompany.Size = new System.Drawing.Size(78, 17);
      this.lblIdCompany.TabIndex = 8;
      this.lblIdCompany.Text = "IdCompany";
      this.lblIdCompany.Visible = false;
      // 
      // cboCompany
      // 
      this.cboCompany.FormattingEnabled = true;
      this.cboCompany.Items.AddRange(new object[] {
            "Nenhum",
            "Oracle",
            "SqlServer"});
      this.cboCompany.Location = new System.Drawing.Point(177, 21);
      this.cboCompany.Name = "cboCompany";
      this.cboCompany.Size = new System.Drawing.Size(712, 24);
      this.cboCompany.TabIndex = 9;
      this.cboCompany.SelectedIndexChanged += new System.EventHandler(this.CboCompany_SelectedIndexChanged);
      // 
      // lblCompany
      // 
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new System.Drawing.Point(6, 24);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new System.Drawing.Size(64, 17);
      this.lblCompany.TabIndex = 7;
      this.lblCompany.Text = "Empresa";
      // 
      // txtCmdCar
      // 
      this.txtCmdCar.Location = new System.Drawing.Point(177, 158);
      this.txtCmdCar.Multiline = true;
      this.txtCmdCar.Name = "txtCmdCar";
      this.txtCmdCar.Size = new System.Drawing.Size(712, 116);
      this.txtCmdCar.TabIndex = 16;
      // 
      // lblCmdCar
      // 
      this.lblCmdCar.AutoSize = true;
      this.lblCmdCar.Location = new System.Drawing.Point(7, 161);
      this.lblCmdCar.Name = "lblCmdCar";
      this.lblCmdCar.Size = new System.Drawing.Size(114, 17);
      this.lblCmdCar.TabIndex = 15;
      this.lblCmdCar.Text = "Comando Mapas";
      // 
      // txtCmdCom
      // 
      this.txtCmdCom.Location = new System.Drawing.Point(177, 81);
      this.txtCmdCom.Multiline = true;
      this.txtCmdCom.Name = "txtCmdCom";
      this.txtCmdCom.Size = new System.Drawing.Size(712, 71);
      this.txtCmdCom.TabIndex = 14;
      // 
      // lblCmdCom
      // 
      this.lblCmdCom.AutoSize = true;
      this.lblCmdCom.Location = new System.Drawing.Point(7, 84);
      this.lblCmdCom.Name = "lblCmdCom";
      this.lblCmdCom.Size = new System.Drawing.Size(161, 17);
      this.lblCmdCom.TabIndex = 13;
      this.lblCmdCom.Text = "Comando Competências";
      // 
      // lblIdSubProc
      // 
      this.lblIdSubProc.AutoSize = true;
      this.lblIdSubProc.Location = new System.Drawing.Point(108, 54);
      this.lblIdSubProc.Name = "lblIdSubProc";
      this.lblIdSubProc.Size = new System.Drawing.Size(95, 17);
      this.lblIdSubProc.TabIndex = 11;
      this.lblIdSubProc.Text = "IdSubProcess";
      this.lblIdSubProc.Visible = false;
      // 
      // prb
      // 
      this.prb.Location = new System.Drawing.Point(177, 297);
      this.prb.Name = "prb";
      this.prb.Size = new System.Drawing.Size(712, 23);
      this.prb.TabIndex = 18;
      // 
      // lblPrc
      // 
      this.lblPrc.AutoSize = true;
      this.lblPrc.Location = new System.Drawing.Point(174, 277);
      this.lblPrc.Name = "lblPrc";
      this.lblPrc.Size = new System.Drawing.Size(134, 17);
      this.lblPrc.TabIndex = 17;
      this.lblPrc.Text = "Ajuda para conexão";
      // 
      // cboSubPro
      // 
      this.cboSubPro.FormattingEnabled = true;
      this.cboSubPro.Items.AddRange(new object[] {
            "Nenhum",
            "Oracle",
            "SqlServer"});
      this.cboSubPro.Location = new System.Drawing.Point(177, 51);
      this.cboSubPro.Name = "cboSubPro";
      this.cboSubPro.Size = new System.Drawing.Size(712, 24);
      this.cboSubPro.TabIndex = 12;
      this.cboSubPro.SelectedIndexChanged += new System.EventHandler(this.CboSubPro_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 54);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(96, 17);
      this.label2.TabIndex = 10;
      this.label2.Text = "Sub Processo";
      // 
      // btnImp
      // 
      this.btnImp.Location = new System.Drawing.Point(756, 326);
      this.btnImp.Name = "btnImp";
      this.btnImp.Size = new System.Drawing.Size(133, 23);
      this.btnImp.TabIndex = 19;
      this.btnImp.Text = "Importar Mapas";
      this.btnImp.UseVisualStyleBackColor = true;
      this.btnImp.Click += new System.EventHandler(this.BtnImp_Click);
      // 
      // ImportacaoAnalisaCargosMapas
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(914, 549);
      this.Controls.Add(this.grpCar);
      this.Controls.Add(this.grpCnx);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ImportacaoAnalisaCargosMapas";
      this.Text = "Importacao de Cargos e Mapas do Analisa";
      this.Load += new System.EventHandler(this.ImportacaoAnalisaCargosMapas_Load);
      this.grpCnx.ResumeLayout(false);
      this.grpCnx.PerformLayout();
      this.grpCar.ResumeLayout(false);
      this.grpCar.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox txtCnxStr;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.GroupBox grpCnx;
    private System.Windows.Forms.Button btnValidCnx;
    private System.Windows.Forms.ComboBox cboTipBd;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox grpCar;
    private System.Windows.Forms.ComboBox cboSubPro;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnImp;
    private System.Windows.Forms.Label lblAjuCnx;
    private System.Windows.Forms.Label lblPrc;
    private System.Windows.Forms.ProgressBar prb;
    private System.Windows.Forms.Label lblIdSubProc;
    private System.Windows.Forms.TextBox txtCmdCom;
    private System.Windows.Forms.Label lblCmdCom;
    private System.Windows.Forms.TextBox txtCmdCar;
    private System.Windows.Forms.Label lblCmdCar;
    private System.Windows.Forms.Label lblIdCompany;
    private System.Windows.Forms.ComboBox cboCompany;
    private System.Windows.Forms.Label lblCompany;
  }
}