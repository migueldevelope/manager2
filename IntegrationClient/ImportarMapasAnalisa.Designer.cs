namespace IntegrationClient
{
  partial class ImportarMapasAnalisa
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
      this.txtCmd = new System.Windows.Forms.TextBox();
      this.lblCmd = new System.Windows.Forms.Label();
      this.btLoc = new System.Windows.Forms.Button();
      this.txtSubProc = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.prb = new System.Windows.Forms.ProgressBar();
      this.label1 = new System.Windows.Forms.Label();
      this.txtLog = new System.Windows.Forms.TextBox();
      this.lblGrpCar = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btImp
      // 
      this.btImp.Enabled = false;
      this.btImp.Location = new System.Drawing.Point(916, 192);
      this.btImp.Name = "btImp";
      this.btImp.Size = new System.Drawing.Size(75, 23);
      this.btImp.TabIndex = 6;
      this.btImp.Text = "Importar";
      this.btImp.UseVisualStyleBackColor = true;
      this.btImp.Click += new System.EventHandler(this.BtImp_Click);
      // 
      // txtCmd
      // 
      this.txtCmd.Location = new System.Drawing.Point(12, 57);
      this.txtCmd.Multiline = true;
      this.txtCmd.Name = "txtCmd";
      this.txtCmd.Size = new System.Drawing.Size(979, 129);
      this.txtCmd.TabIndex = 5;
      // 
      // lblCmd
      // 
      this.lblCmd.AutoSize = true;
      this.lblCmd.Location = new System.Drawing.Point(12, 37);
      this.lblCmd.Name = "lblCmd";
      this.lblCmd.Size = new System.Drawing.Size(232, 17);
      this.lblCmd.TabIndex = 4;
      this.lblCmd.Text = "Comando de Leitura base ANALISA";
      // 
      // btLoc
      // 
      this.btLoc.Location = new System.Drawing.Point(916, 11);
      this.btLoc.Name = "btLoc";
      this.btLoc.Size = new System.Drawing.Size(75, 23);
      this.btLoc.TabIndex = 24;
      this.btLoc.Text = "Localizar";
      this.btLoc.UseVisualStyleBackColor = true;
      this.btLoc.Click += new System.EventHandler(this.BtLoc_Click);
      // 
      // txtSubProc
      // 
      this.txtSubProc.Location = new System.Drawing.Point(114, 12);
      this.txtSubProc.Name = "txtSubProc";
      this.txtSubProc.Size = new System.Drawing.Size(796, 22);
      this.txtSubProc.TabIndex = 23;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(12, 14);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(96, 17);
      this.label8.TabIndex = 22;
      this.label8.Text = "Sub.Processo";
      // 
      // prb
      // 
      this.prb.Location = new System.Drawing.Point(114, 192);
      this.prb.Name = "prb";
      this.prb.Size = new System.Drawing.Size(796, 23);
      this.prb.TabIndex = 26;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 222);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(145, 17);
      this.label1.TabIndex = 28;
      this.label1.Text = "Log da Sincronização";
      // 
      // txtLog
      // 
      this.txtLog.Location = new System.Drawing.Point(12, 242);
      this.txtLog.Multiline = true;
      this.txtLog.Name = "txtLog";
      this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.txtLog.Size = new System.Drawing.Size(979, 331);
      this.txtLog.TabIndex = 27;
      // 
      // lblGrpCar
      // 
      this.lblGrpCar.AutoSize = true;
      this.lblGrpCar.BackColor = System.Drawing.SystemColors.Highlight;
      this.lblGrpCar.Location = new System.Drawing.Point(208, 222);
      this.lblGrpCar.Name = "lblGrpCar";
      this.lblGrpCar.Size = new System.Drawing.Size(0, 17);
      this.lblGrpCar.TabIndex = 29;
      // 
      // ImportarMapasAnalisa
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1003, 585);
      this.Controls.Add(this.lblGrpCar);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txtLog);
      this.Controls.Add(this.prb);
      this.Controls.Add(this.btLoc);
      this.Controls.Add(this.txtSubProc);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.btImp);
      this.Controls.Add(this.txtCmd);
      this.Controls.Add(this.lblCmd);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ImportarMapasAnalisa";
      this.Text = "ImportarCargoAnalisa";
      this.Load += new System.EventHandler(this.ImportarCargoAnalisa_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btImp;
    private System.Windows.Forms.TextBox txtCmd;
    private System.Windows.Forms.Label lblCmd;
    private System.Windows.Forms.Button btLoc;
    private System.Windows.Forms.TextBox txtSubProc;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.ProgressBar prb;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.Label lblGrpCar;
  }
}