namespace IntegrationClient
{
  partial class ExportarMapas
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
      this.btnExp = new System.Windows.Forms.Button();
      this.txtPst = new System.Windows.Forms.TextBox();
      this.lblPst = new System.Windows.Forms.Label();
      this.lblPrb = new System.Windows.Forms.Label();
      this.prb = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // btnExp
      // 
      this.btnExp.Location = new System.Drawing.Point(534, 39);
      this.btnExp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnExp.Name = "btnExp";
      this.btnExp.Size = new System.Drawing.Size(254, 23);
      this.btnExp.TabIndex = 10;
      this.btnExp.Text = "Exportar Mapas";
      this.btnExp.UseVisualStyleBackColor = true;
      this.btnExp.Click += new System.EventHandler(this.BtnExp_Click);
      // 
      // txtPst
      // 
      this.txtPst.Location = new System.Drawing.Point(114, 12);
      this.txtPst.Name = "txtPst";
      this.txtPst.Size = new System.Drawing.Size(674, 22);
      this.txtPst.TabIndex = 9;
      // 
      // lblPst
      // 
      this.lblPst.AutoSize = true;
      this.lblPst.Location = new System.Drawing.Point(12, 15);
      this.lblPst.Name = "lblPst";
      this.lblPst.Size = new System.Drawing.Size(96, 17);
      this.lblPst.TabIndex = 8;
      this.lblPst.Text = "Pasta Destino";
      // 
      // lblPrb
      // 
      this.lblPrb.AutoSize = true;
      this.lblPrb.Location = new System.Drawing.Point(16, 100);
      this.lblPrb.Name = "lblPrb";
      this.lblPrb.Size = new System.Drawing.Size(77, 17);
      this.lblPrb.TabIndex = 12;
      this.lblPrb.Text = "Data Inicial";
      // 
      // prb
      // 
      this.prb.Location = new System.Drawing.Point(15, 120);
      this.prb.Name = "prb";
      this.prb.Size = new System.Drawing.Size(773, 23);
      this.prb.TabIndex = 11;
      // 
      // ExportarMapas
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 153);
      this.Controls.Add(this.lblPrb);
      this.Controls.Add(this.prb);
      this.Controls.Add(this.btnExp);
      this.Controls.Add(this.txtPst);
      this.Controls.Add(this.lblPst);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExportarMapas";
      this.Text = "Exportação de Mapas de Competências";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnExp;
    private System.Windows.Forms.TextBox txtPst;
    private System.Windows.Forms.Label lblPst;
    private System.Windows.Forms.Label lblPrb;
    private System.Windows.Forms.ProgressBar prb;
  }
}