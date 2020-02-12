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
      this.components = new System.ComponentModel.Container();
      this.btnImp = new System.Windows.Forms.Button();
      this.btnDemAus = new System.Windows.Forms.Button();
      this.prb = new System.Windows.Forms.ProgressBar();
      this.lblPrb = new System.Windows.Forms.Label();
      this.chkLjo = new System.Windows.Forms.CheckBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.SuspendLayout();
      // 
      // btnImp
      // 
      this.btnImp.Location = new System.Drawing.Point(12, 10);
      this.btnImp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnImp.Name = "btnImp";
      this.btnImp.Size = new System.Drawing.Size(254, 23);
      this.btnImp.TabIndex = 6;
      this.btnImp.Text = "Executar Importação";
      this.btnImp.UseVisualStyleBackColor = true;
      this.btnImp.Click += new System.EventHandler(this.BtnImp_Click);
      // 
      // btnDemAus
      // 
      this.btnDemAus.Location = new System.Drawing.Point(12, 99);
      this.btnDemAus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnDemAus.Name = "btnDemAus";
      this.btnDemAus.Size = new System.Drawing.Size(254, 23);
      this.btnDemAus.TabIndex = 8;
      this.btnDemAus.Text = "Demissões por Ausência";
      this.toolTip1.SetToolTip(this.btnDemAus, "Demite os colaboradores que não aparecerem na lista de colaboradores ativos.");
      this.btnDemAus.UseVisualStyleBackColor = true;
      this.btnDemAus.Click += new System.EventHandler(this.BtnDemAus_Click);
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
      this.chkLjo.Location = new System.Drawing.Point(12, 42);
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
      this.Controls.Add(this.btnDemAus);
      this.Controls.Add(this.btnImp);
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
    private System.Windows.Forms.Button btnImp;
    private System.Windows.Forms.Button btnDemAus;
    private System.Windows.Forms.ProgressBar prb;
    private System.Windows.Forms.Label lblPrb;
    private System.Windows.Forms.CheckBox chkLjo;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}