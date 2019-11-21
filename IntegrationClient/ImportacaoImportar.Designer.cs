namespace IntegrationClient
{
  partial class ImportacaoImportar
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
      this.btImp = new System.Windows.Forms.Button();
      this.btDem = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.txtDatIni = new System.Windows.Forms.TextBox();
      this.txtDatFin = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnImpV2 = new System.Windows.Forms.Button();
      this.btnDemUltV2 = new System.Windows.Forms.Button();
      this.btnDemV2 = new System.Windows.Forms.Button();
      this.prb = new System.Windows.Forms.ProgressBar();
      this.lblPrb = new System.Windows.Forms.Label();
      this.chkLjo = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // btImp
      // 
      this.btImp.Location = new System.Drawing.Point(12, 11);
      this.btImp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btImp.Name = "btImp";
      this.btImp.Size = new System.Drawing.Size(251, 23);
      this.btImp.TabIndex = 0;
      this.btImp.Text = "Executar Importação V1";
      this.btImp.UseVisualStyleBackColor = true;
      this.btImp.Click += new System.EventHandler(this.BtImp_Click);
      // 
      // btDem
      // 
      this.btDem.Location = new System.Drawing.Point(9, 100);
      this.btDem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btDem.Name = "btDem";
      this.btDem.Size = new System.Drawing.Size(254, 23);
      this.btDem.TabIndex = 1;
      this.btDem.Text = "Demissões V1";
      this.btDem.UseVisualStyleBackColor = true;
      this.btDem.Click += new System.EventHandler(this.BtDem_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(10, 48);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(77, 17);
      this.label1.TabIndex = 2;
      this.label1.Text = "Data Inicial";
      // 
      // txtDatIni
      // 
      this.txtDatIni.Location = new System.Drawing.Point(93, 45);
      this.txtDatIni.Name = "txtDatIni";
      this.txtDatIni.Size = new System.Drawing.Size(170, 22);
      this.txtDatIni.TabIndex = 3;
      // 
      // txtDatFin
      // 
      this.txtDatFin.Location = new System.Drawing.Point(93, 73);
      this.txtDatFin.Name = "txtDatFin";
      this.txtDatFin.Size = new System.Drawing.Size(170, 22);
      this.txtDatFin.TabIndex = 5;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(15, 76);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(72, 17);
      this.label2.TabIndex = 4;
      this.label2.Text = "Data Final";
      // 
      // btnImpV2
      // 
      this.btnImpV2.Location = new System.Drawing.Point(326, 11);
      this.btnImpV2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnImpV2.Name = "btnImpV2";
      this.btnImpV2.Size = new System.Drawing.Size(254, 23);
      this.btnImpV2.TabIndex = 6;
      this.btnImpV2.Text = "Executar Importação V2";
      this.btnImpV2.UseVisualStyleBackColor = true;
      this.btnImpV2.Click += new System.EventHandler(this.BtnImpV2_Click);
      // 
      // btnDemUltV2
      // 
      this.btnDemUltV2.Location = new System.Drawing.Point(326, 127);
      this.btnDemUltV2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnDemUltV2.Name = "btnDemUltV2";
      this.btnDemUltV2.Size = new System.Drawing.Size(254, 23);
      this.btnDemUltV2.TabIndex = 7;
      this.btnDemUltV2.Text = "Demissões Última Importação V2";
      this.btnDemUltV2.UseVisualStyleBackColor = true;
      this.btnDemUltV2.Click += new System.EventHandler(this.BtnDemUltV2_Click);
      // 
      // btnDemV2
      // 
      this.btnDemV2.Enabled = false;
      this.btnDemV2.Location = new System.Drawing.Point(326, 100);
      this.btnDemV2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnDemV2.Name = "btnDemV2";
      this.btnDemV2.Size = new System.Drawing.Size(254, 23);
      this.btnDemV2.TabIndex = 8;
      this.btnDemV2.Text = "Demissões V2";
      this.btnDemV2.UseVisualStyleBackColor = true;
      // 
      // prb
      // 
      this.prb.Location = new System.Drawing.Point(9, 300);
      this.prb.Name = "prb";
      this.prb.Size = new System.Drawing.Size(571, 23);
      this.prb.TabIndex = 9;
      // 
      // lblPrb
      // 
      this.lblPrb.AutoSize = true;
      this.lblPrb.Location = new System.Drawing.Point(10, 280);
      this.lblPrb.Name = "lblPrb";
      this.lblPrb.Size = new System.Drawing.Size(77, 17);
      this.lblPrb.TabIndex = 10;
      this.lblPrb.Text = "Data Inicial";
      // 
      // chkLjo
      // 
      this.chkLjo.AutoSize = true;
      this.chkLjo.Location = new System.Drawing.Point(326, 43);
      this.chkLjo.Name = "chkLjo";
      this.chkLjo.Size = new System.Drawing.Size(88, 21);
      this.chkLjo.TabIndex = 11;
      this.chkLjo.Text = "Log Json";
      this.chkLjo.UseVisualStyleBackColor = true;
      // 
      // ImportacaoImportar
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(591, 335);
      this.Controls.Add(this.chkLjo);
      this.Controls.Add(this.lblPrb);
      this.Controls.Add(this.prb);
      this.Controls.Add(this.btnDemV2);
      this.Controls.Add(this.btnDemUltV2);
      this.Controls.Add(this.btnImpV2);
      this.Controls.Add(this.txtDatFin);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.txtDatIni);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btDem);
      this.Controls.Add(this.btImp);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ImportacaoImportar";
      this.Text = "ImportacaoImportar";
      this.Load += new System.EventHandler(this.ImportacaoImportar_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btImp;
    private System.Windows.Forms.Button btDem;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtDatIni;
    private System.Windows.Forms.TextBox txtDatFin;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnImpV2;
    private System.Windows.Forms.Button btnDemUltV2;
    private System.Windows.Forms.Button btnDemV2;
    private System.Windows.Forms.ProgressBar prb;
    private System.Windows.Forms.Label lblPrb;
    private System.Windows.Forms.CheckBox chkLjo;
  }
}