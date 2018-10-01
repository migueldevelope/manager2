namespace IntegrationClient
{
  partial class SincronizarCompetencia
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
      this.lblCmd = new System.Windows.Forms.Label();
      this.txtCmd = new System.Windows.Forms.TextBox();
      this.txtLog = new System.Windows.Forms.TextBox();
      this.btVal = new System.Windows.Forms.Button();
      this.btAtu = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblCmd
      // 
      this.lblCmd.AutoSize = true;
      this.lblCmd.Location = new System.Drawing.Point(12, 9);
      this.lblCmd.Name = "lblCmd";
      this.lblCmd.Size = new System.Drawing.Size(232, 17);
      this.lblCmd.TabIndex = 0;
      this.lblCmd.Text = "Comando de Leitura base ANALISA";
      // 
      // txtCmd
      // 
      this.txtCmd.Location = new System.Drawing.Point(12, 29);
      this.txtCmd.Multiline = true;
      this.txtCmd.Name = "txtCmd";
      this.txtCmd.Size = new System.Drawing.Size(942, 129);
      this.txtCmd.TabIndex = 1;
      // 
      // txtLog
      // 
      this.txtLog.Location = new System.Drawing.Point(12, 198);
      this.txtLog.Multiline = true;
      this.txtLog.Name = "txtLog";
      this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.txtLog.Size = new System.Drawing.Size(942, 259);
      this.txtLog.TabIndex = 2;
      // 
      // btVal
      // 
      this.btVal.Location = new System.Drawing.Point(879, 164);
      this.btVal.Name = "btVal";
      this.btVal.Size = new System.Drawing.Size(75, 23);
      this.btVal.TabIndex = 3;
      this.btVal.Text = "Validar";
      this.btVal.UseVisualStyleBackColor = true;
      this.btVal.Click += new System.EventHandler(this.BtVal_Click);
      // 
      // btAtu
      // 
      this.btAtu.Location = new System.Drawing.Point(879, 463);
      this.btAtu.Name = "btAtu";
      this.btAtu.Size = new System.Drawing.Size(75, 23);
      this.btAtu.TabIndex = 4;
      this.btAtu.Text = "Atualizar";
      this.btAtu.UseVisualStyleBackColor = true;
      this.btAtu.Click += new System.EventHandler(this.BtAtu_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 178);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(145, 17);
      this.label1.TabIndex = 5;
      this.label1.Text = "Log da Sincronização";
      // 
      // SincronizarCompetencia
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(966, 494);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btAtu);
      this.Controls.Add(this.btVal);
      this.Controls.Add(this.txtLog);
      this.Controls.Add(this.txtCmd);
      this.Controls.Add(this.lblCmd);
      this.Name = "SincronizarCompetencia";
      this.Text = "SincronizarCompetencias";
      this.Load += new System.EventHandler(this.SincronizarCompetencia_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblCmd;
    private System.Windows.Forms.TextBox txtCmd;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.Button btVal;
    private System.Windows.Forms.Button btAtu;
    private System.Windows.Forms.Label label1;
  }
}