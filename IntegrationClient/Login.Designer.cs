namespace IntegrationClient
{
  partial class Login
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
      this.lblEma = new System.Windows.Forms.Label();
      this.lblSen = new System.Windows.Forms.Label();
      this.txtSen = new System.Windows.Forms.TextBox();
      this.txtUrl = new System.Windows.Forms.TextBox();
      this.txtEma = new System.Windows.Forms.TextBox();
      this.lblUrl = new System.Windows.Forms.Label();
      this.btOk = new System.Windows.Forms.Button();
      this.btCan = new System.Windows.Forms.Button();
      this.lblVer = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblEma
      // 
      this.lblEma.AutoSize = true;
      this.lblEma.Location = new System.Drawing.Point(134, 81);
      this.lblEma.Name = "lblEma";
      this.lblEma.Size = new System.Drawing.Size(47, 17);
      this.lblEma.TabIndex = 0;
      this.lblEma.Text = "E-mail";
      // 
      // lblSen
      // 
      this.lblSen.AutoSize = true;
      this.lblSen.Location = new System.Drawing.Point(134, 109);
      this.lblSen.Name = "lblSen";
      this.lblSen.Size = new System.Drawing.Size(49, 17);
      this.lblSen.TabIndex = 1;
      this.lblSen.Text = "Senha";
      // 
      // txtSen
      // 
      this.txtSen.Location = new System.Drawing.Point(189, 106);
      this.txtSen.Name = "txtSen";
      this.txtSen.Size = new System.Drawing.Size(254, 22);
      this.txtSen.TabIndex = 2;
      // 
      // txtUrl
      // 
      this.txtUrl.Location = new System.Drawing.Point(189, 50);
      this.txtUrl.Name = "txtUrl";
      this.txtUrl.Size = new System.Drawing.Size(254, 22);
      this.txtUrl.TabIndex = 0;
      // 
      // txtEma
      // 
      this.txtEma.Location = new System.Drawing.Point(189, 78);
      this.txtEma.Name = "txtEma";
      this.txtEma.Size = new System.Drawing.Size(254, 22);
      this.txtEma.TabIndex = 1;
      // 
      // lblUrl
      // 
      this.lblUrl.AutoSize = true;
      this.lblUrl.Location = new System.Drawing.Point(134, 53);
      this.lblUrl.Name = "lblUrl";
      this.lblUrl.Size = new System.Drawing.Size(26, 17);
      this.lblUrl.TabIndex = 4;
      this.lblUrl.Text = "Url";
      // 
      // btOk
      // 
      this.btOk.Location = new System.Drawing.Point(245, 134);
      this.btOk.Name = "btOk";
      this.btOk.Size = new System.Drawing.Size(96, 23);
      this.btOk.TabIndex = 3;
      this.btOk.Text = "Ok";
      this.btOk.UseVisualStyleBackColor = true;
      this.btOk.Click += new System.EventHandler(this.BtOk_Click);
      // 
      // btCan
      // 
      this.btCan.Location = new System.Drawing.Point(347, 134);
      this.btCan.Name = "btCan";
      this.btCan.Size = new System.Drawing.Size(96, 23);
      this.btCan.TabIndex = 4;
      this.btCan.Text = "Cancelar";
      this.btCan.UseVisualStyleBackColor = true;
      this.btCan.Click += new System.EventHandler(this.BtCan_Click);
      // 
      // lblVer
      // 
      this.lblVer.AutoSize = true;
      this.lblVer.Location = new System.Drawing.Point(551, 205);
      this.lblVer.Name = "lblVer";
      this.lblVer.Size = new System.Drawing.Size(49, 17);
      this.lblVer.TabIndex = 5;
      this.lblVer.Text = "Senha";
      this.lblVer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // Login
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(612, 231);
      this.ControlBox = false;
      this.Controls.Add(this.lblVer);
      this.Controls.Add(this.btCan);
      this.Controls.Add(this.btOk);
      this.Controls.Add(this.txtEma);
      this.Controls.Add(this.lblUrl);
      this.Controls.Add(this.txtUrl);
      this.Controls.Add(this.txtSen);
      this.Controls.Add(this.lblSen);
      this.Controls.Add(this.lblEma);
      this.Name = "Login";
      this.Text = "Login";
      this.Load += new System.EventHandler(this.Login_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblEma;
    private System.Windows.Forms.Label lblSen;
    private System.Windows.Forms.TextBox txtSen;
    private System.Windows.Forms.TextBox txtUrl;
    private System.Windows.Forms.TextBox txtEma;
    private System.Windows.Forms.Label lblUrl;
    private System.Windows.Forms.Button btOk;
    private System.Windows.Forms.Button btCan;
    private System.Windows.Forms.Label lblVer;
  }
}

