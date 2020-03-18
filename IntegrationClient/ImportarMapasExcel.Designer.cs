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
      this.chkAtu = new System.Windows.Forms.CheckBox();
      this.folder = new System.Windows.Forms.FolderBrowserDialog();
      this.chkLjo = new System.Windows.Forms.CheckBox();
      this.chkCom = new System.Windows.Forms.CheckBox();
      this.chkAre = new System.Windows.Forms.CheckBox();
      this.chkSub = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // txtPst
      // 
      this.txtPst.Location = new System.Drawing.Point(114, 6);
      this.txtPst.Name = "txtPst";
      this.txtPst.Size = new System.Drawing.Size(674, 22);
      this.txtPst.TabIndex = 2;
      this.txtPst.Text = "D:\\PUCRS\\MAPAS\\";
      // 
      // lblPst
      // 
      this.lblPst.AutoSize = true;
      this.lblPst.Location = new System.Drawing.Point(12, 9);
      this.lblPst.Name = "lblPst";
      this.lblPst.Size = new System.Drawing.Size(94, 17);
      this.lblPst.TabIndex = 1;
      this.lblPst.Text = "Pasta Origem";
      // 
      // btnImpV2
      // 
      this.btnImpV2.Location = new System.Drawing.Point(534, 33);
      this.btnImpV2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnImpV2.Name = "btnImpV2";
      this.btnImpV2.Size = new System.Drawing.Size(254, 23);
      this.btnImpV2.TabIndex = 8;
      this.btnImpV2.Text = "Importar Modelo Padrão Fluid";
      this.btnImpV2.UseVisualStyleBackColor = true;
      this.btnImpV2.Click += new System.EventHandler(this.BtnImpV2_Click);
      // 
      // chkAtu
      // 
      this.chkAtu.AutoSize = true;
      this.chkAtu.Location = new System.Drawing.Point(114, 116);
      this.chkAtu.Name = "chkAtu";
      this.chkAtu.Size = new System.Drawing.Size(142, 21);
      this.chkAtu.TabIndex = 7;
      this.chkAtu.Text = "Atualizar Cargos?";
      this.chkAtu.UseVisualStyleBackColor = true;
      this.chkAtu.CheckedChanged += new System.EventHandler(this.ChkAtu_CheckedChanged);
      // 
      // chkLjo
      // 
      this.chkLjo.AutoSize = true;
      this.chkLjo.Location = new System.Drawing.Point(114, 143);
      this.chkLjo.Name = "chkLjo";
      this.chkLjo.Size = new System.Drawing.Size(138, 21);
      this.chkLjo.TabIndex = 5;
      this.chkLjo.Text = "Ativar log JSON?";
      this.chkLjo.UseVisualStyleBackColor = true;
      // 
      // chkCom
      // 
      this.chkCom.AutoSize = true;
      this.chkCom.Location = new System.Drawing.Point(114, 89);
      this.chkCom.Name = "chkCom";
      this.chkCom.Size = new System.Drawing.Size(186, 21);
      this.chkCom.TabIndex = 6;
      this.chkCom.Text = "Atualizar Competências?";
      this.chkCom.UseVisualStyleBackColor = true;
      // 
      // chkAre
      // 
      this.chkAre.AutoSize = true;
      this.chkAre.Location = new System.Drawing.Point(114, 62);
      this.chkAre.Name = "chkAre";
      this.chkAre.Size = new System.Drawing.Size(285, 21);
      this.chkAre.TabIndex = 4;
      this.chkAre.Text = "Estrutura de Pastas representa as LOs?";
      this.chkAre.UseVisualStyleBackColor = true;
      // 
      // chkSub
      // 
      this.chkSub.AutoSize = true;
      this.chkSub.Location = new System.Drawing.Point(114, 35);
      this.chkSub.Name = "chkSub";
      this.chkSub.Size = new System.Drawing.Size(191, 21);
      this.chkSub.TabIndex = 3;
      this.chkSub.Text = "Localizar nas subpastas?";
      this.chkSub.UseVisualStyleBackColor = true;
      // 
      // ImportarMapasExcel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.chkSub);
      this.Controls.Add(this.chkAre);
      this.Controls.Add(this.chkCom);
      this.Controls.Add(this.chkLjo);
      this.Controls.Add(this.chkAtu);
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
    private System.Windows.Forms.CheckBox chkAtu;
    private System.Windows.Forms.FolderBrowserDialog folder;
    private System.Windows.Forms.CheckBox chkLjo;
    private System.Windows.Forms.CheckBox chkCom;
        private System.Windows.Forms.CheckBox chkAre;
        private System.Windows.Forms.CheckBox chkSub;
    }
}