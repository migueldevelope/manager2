namespace IntegrationClient
{
  partial class ImportarMapasExcel
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
      this.txtPst = new System.Windows.Forms.TextBox();
      this.lblPst = new System.Windows.Forms.Label();
      this.btnImpV2 = new System.Windows.Forms.Button();
      this.chkEppPlus = new System.Windows.Forms.CheckBox();
      this.chkAtu = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // txtPst
      // 
      this.txtPst.Location = new System.Drawing.Point(114, 6);
      this.txtPst.Name = "txtPst";
      this.txtPst.Size = new System.Drawing.Size(674, 22);
      this.txtPst.TabIndex = 25;
      // 
      // lblPst
      // 
      this.lblPst.AutoSize = true;
      this.lblPst.Location = new System.Drawing.Point(12, 9);
      this.lblPst.Name = "lblPst";
      this.lblPst.Size = new System.Drawing.Size(94, 17);
      this.lblPst.TabIndex = 24;
      this.lblPst.Text = "Pasta Origem";
      // 
      // btnImpV2
      // 
      this.btnImpV2.Location = new System.Drawing.Point(534, 33);
      this.btnImpV2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnImpV2.Name = "btnImpV2";
      this.btnImpV2.Size = new System.Drawing.Size(254, 23);
      this.btnImpV2.TabIndex = 26;
      this.btnImpV2.Text = "Importar Modelo Padrão Fluid";
      this.btnImpV2.UseVisualStyleBackColor = true;
      this.btnImpV2.Click += new System.EventHandler(this.BtnImpV2_Click);
      // 
      // chkEppPlus
      // 
      this.chkEppPlus.AutoSize = true;
      this.chkEppPlus.Location = new System.Drawing.Point(114, 33);
      this.chkEppPlus.Name = "chkEppPlus";
      this.chkEppPlus.Size = new System.Drawing.Size(220, 21);
      this.chkEppPlus.TabIndex = 27;
      this.chkEppPlus.Text = "Utilizar EppPlus (apenas .xlsx)";
      this.chkEppPlus.UseVisualStyleBackColor = true;
      // 
      // chkAtu
      // 
      this.chkAtu.AutoSize = true;
      this.chkAtu.Location = new System.Drawing.Point(114, 60);
      this.chkAtu.Name = "chkAtu";
      this.chkAtu.Size = new System.Drawing.Size(142, 21);
      this.chkAtu.TabIndex = 28;
      this.chkAtu.Text = "Atualizar Cargos?";
      this.chkAtu.UseVisualStyleBackColor = true;
      // 
      // ImportarMapasExcel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.chkAtu);
      this.Controls.Add(this.chkEppPlus);
      this.Controls.Add(this.btnImpV2);
      this.Controls.Add(this.txtPst);
      this.Controls.Add(this.lblPst);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ImportarMapasExcel";
      this.Text = "Importação de Mapas de Competência do Microsoft Excel";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtPst;
    private System.Windows.Forms.Label lblPst;
    private System.Windows.Forms.Button btnImpV2;
    private System.Windows.Forms.CheckBox chkEppPlus;
    private System.Windows.Forms.CheckBox chkAtu;
  }
}