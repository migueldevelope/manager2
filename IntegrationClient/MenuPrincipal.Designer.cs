﻿namespace IntegrationClient
{
  partial class MenuPrincipal
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
      this.txtEma = new System.Windows.Forms.TextBox();
      this.lblUrl = new System.Windows.Forms.Label();
      this.txtUrl = new System.Windows.Forms.TextBox();
      this.lblEma = new System.Windows.Forms.Label();
      this.btChange = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.cboMode = new System.Windows.Forms.ComboBox();
      this.cboType = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.grpBD = new System.Windows.Forms.GroupBox();
      this.btSave = new System.Windows.Forms.Button();
      this.label9 = new System.Windows.Forms.Label();
      this.txtSql = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.chkOra = new System.Windows.Forms.CheckBox();
      this.txtDefault = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.txtHostName = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtUser = new System.Windows.Forms.TextBox();
      this.grpArq = new System.Windows.Forms.GroupBox();
      this.btSaveFile = new System.Windows.Forms.Button();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.btSearchFile = new System.Windows.Forms.Button();
      this.txtFileName = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.btImp = new System.Windows.Forms.Button();
      this.grpBD.SuspendLayout();
      this.grpArq.SuspendLayout();
      this.SuspendLayout();
      // 
      // txtEma
      // 
      this.txtEma.Enabled = false;
      this.txtEma.Location = new System.Drawing.Point(466, 33);
      this.txtEma.Name = "txtEma";
      this.txtEma.Size = new System.Drawing.Size(241, 22);
      this.txtEma.TabIndex = 7;
      // 
      // lblUrl
      // 
      this.lblUrl.AutoSize = true;
      this.lblUrl.Location = new System.Drawing.Point(13, 36);
      this.lblUrl.Name = "lblUrl";
      this.lblUrl.Size = new System.Drawing.Size(26, 17);
      this.lblUrl.TabIndex = 8;
      this.lblUrl.Text = "Url";
      // 
      // txtUrl
      // 
      this.txtUrl.Enabled = false;
      this.txtUrl.Location = new System.Drawing.Point(153, 33);
      this.txtUrl.Name = "txtUrl";
      this.txtUrl.Size = new System.Drawing.Size(254, 22);
      this.txtUrl.TabIndex = 5;
      // 
      // lblEma
      // 
      this.lblEma.AutoSize = true;
      this.lblEma.Location = new System.Drawing.Point(413, 36);
      this.lblEma.Name = "lblEma";
      this.lblEma.Size = new System.Drawing.Size(47, 17);
      this.lblEma.TabIndex = 6;
      this.lblEma.Text = "E-mail";
      // 
      // btChange
      // 
      this.btChange.Location = new System.Drawing.Point(713, 33);
      this.btChange.Name = "btChange";
      this.btChange.Size = new System.Drawing.Size(75, 23);
      this.btChange.TabIndex = 9;
      this.btChange.Text = "Trocar";
      this.btChange.UseVisualStyleBackColor = true;
      this.btChange.Click += new System.EventHandler(this.BtChange_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(123, 17);
      this.label1.TabIndex = 13;
      this.label1.Text = "Cliente Conectado";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 64);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(134, 17);
      this.label2.TabIndex = 14;
      this.label2.Text = "Modo de Integração";
      // 
      // cboMode
      // 
      this.cboMode.FormattingEnabled = true;
      this.cboMode.Location = new System.Drawing.Point(153, 61);
      this.cboMode.Name = "cboMode";
      this.cboMode.Size = new System.Drawing.Size(254, 24);
      this.cboMode.TabIndex = 15;
      this.cboMode.SelectedIndexChanged += new System.EventHandler(this.CboMode_SelectedIndexChanged);
      // 
      // cboType
      // 
      this.cboType.FormattingEnabled = true;
      this.cboType.Location = new System.Drawing.Point(153, 91);
      this.cboType.Name = "cboType";
      this.cboType.Size = new System.Drawing.Size(254, 24);
      this.cboType.TabIndex = 17;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 94);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(127, 17);
      this.label3.TabIndex = 16;
      this.label3.Text = "Tipo de Integração";
      // 
      // grpBD
      // 
      this.grpBD.Controls.Add(this.btSave);
      this.grpBD.Controls.Add(this.label9);
      this.grpBD.Controls.Add(this.txtSql);
      this.grpBD.Controls.Add(this.label6);
      this.grpBD.Controls.Add(this.chkOra);
      this.grpBD.Controls.Add(this.txtDefault);
      this.grpBD.Controls.Add(this.label8);
      this.grpBD.Controls.Add(this.label5);
      this.grpBD.Controls.Add(this.txtHostName);
      this.grpBD.Controls.Add(this.txtPassword);
      this.grpBD.Controls.Add(this.label7);
      this.grpBD.Controls.Add(this.label4);
      this.grpBD.Controls.Add(this.txtUser);
      this.grpBD.Location = new System.Drawing.Point(16, 121);
      this.grpBD.Name = "grpBD";
      this.grpBD.Size = new System.Drawing.Size(780, 498);
      this.grpBD.TabIndex = 20;
      this.grpBD.TabStop = false;
      this.grpBD.Text = "Banco de Dados";
      this.grpBD.Visible = false;
      // 
      // btSave
      // 
      this.btSave.Location = new System.Drawing.Point(724, 469);
      this.btSave.Name = "btSave";
      this.btSave.Size = new System.Drawing.Size(56, 23);
      this.btSave.TabIndex = 58;
      this.btSave.Text = "Salvar";
      this.btSave.UseVisualStyleBackColor = true;
      this.btSave.Click += new System.EventHandler(this.BtSave_Click);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(10, 117);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(36, 17);
      this.label9.TabIndex = 57;
      this.label9.Text = "SQL";
      // 
      // txtSql
      // 
      this.txtSql.Font = new System.Drawing.Font("Tahoma", 8.25F);
      this.txtSql.Location = new System.Drawing.Point(106, 114);
      this.txtSql.Multiline = true;
      this.txtSql.Name = "txtSql";
      this.txtSql.Size = new System.Drawing.Size(612, 378);
      this.txtSql.TabIndex = 56;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(368, 87);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(90, 17);
      this.label6.TabIndex = 54;
      this.label6.Text = "Base Padrão";
      // 
      // chkOra
      // 
      this.chkOra.AutoSize = true;
      this.chkOra.Location = new System.Drawing.Point(176, 29);
      this.chkOra.Name = "chkOra";
      this.chkOra.Size = new System.Drawing.Size(93, 21);
      this.chkOra.TabIndex = 55;
      this.chkOra.Text = "É Oracle?";
      this.chkOra.UseVisualStyleBackColor = true;
      // 
      // txtDefault
      // 
      this.txtDefault.Font = new System.Drawing.Font("Tahoma", 8.25F);
      this.txtDefault.Location = new System.Drawing.Point(464, 84);
      this.txtDefault.Name = "txtDefault";
      this.txtDefault.Size = new System.Drawing.Size(254, 24);
      this.txtDefault.TabIndex = 53;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(6, 29);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(164, 17);
      this.label8.TabIndex = 46;
      this.label8.Text = "Conexão Base de Dados";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(10, 87);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(49, 17);
      this.label5.TabIndex = 52;
      this.label5.Text = "Senha";
      // 
      // txtHostName
      // 
      this.txtHostName.Location = new System.Drawing.Point(106, 56);
      this.txtHostName.Name = "txtHostName";
      this.txtHostName.Size = new System.Drawing.Size(254, 22);
      this.txtHostName.TabIndex = 47;
      // 
      // txtPassword
      // 
      this.txtPassword.Font = new System.Drawing.Font("Tahoma", 8.25F);
      this.txtPassword.Location = new System.Drawing.Point(106, 84);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(254, 24);
      this.txtPassword.TabIndex = 51;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(10, 59);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(72, 17);
      this.label7.TabIndex = 48;
      this.label7.Text = "Hostname";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(368, 57);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(57, 17);
      this.label4.TabIndex = 50;
      this.label4.Text = "Usuário";
      // 
      // txtUser
      // 
      this.txtUser.Location = new System.Drawing.Point(464, 54);
      this.txtUser.Name = "txtUser";
      this.txtUser.Size = new System.Drawing.Size(254, 22);
      this.txtUser.TabIndex = 49;
      // 
      // grpArq
      // 
      this.grpArq.Controls.Add(this.btSaveFile);
      this.grpArq.Controls.Add(this.textBox2);
      this.grpArq.Controls.Add(this.label11);
      this.grpArq.Controls.Add(this.btSearchFile);
      this.grpArq.Controls.Add(this.txtFileName);
      this.grpArq.Controls.Add(this.label10);
      this.grpArq.Location = new System.Drawing.Point(16, 121);
      this.grpArq.Name = "grpArq";
      this.grpArq.Size = new System.Drawing.Size(780, 498);
      this.grpArq.TabIndex = 21;
      this.grpArq.TabStop = false;
      this.grpArq.Text = "Arquivo CSV";
      this.grpArq.Visible = false;
      // 
      // btSaveFile
      // 
      this.btSaveFile.Location = new System.Drawing.Point(652, 469);
      this.btSaveFile.Name = "btSaveFile";
      this.btSaveFile.Size = new System.Drawing.Size(75, 23);
      this.btSaveFile.TabIndex = 20;
      this.btSaveFile.Text = "Salvar";
      this.btSaveFile.UseVisualStyleBackColor = true;
      // 
      // textBox2
      // 
      this.textBox2.Enabled = false;
      this.textBox2.Location = new System.Drawing.Point(137, 57);
      this.textBox2.Multiline = true;
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(590, 406);
      this.textBox2.TabIndex = 19;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(6, 57);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(63, 17);
      this.label11.TabIndex = 18;
      this.label11.Text = "Situação";
      // 
      // btSearchFile
      // 
      this.btSearchFile.Location = new System.Drawing.Point(733, 26);
      this.btSearchFile.Name = "btSearchFile";
      this.btSearchFile.Size = new System.Drawing.Size(38, 23);
      this.btSearchFile.TabIndex = 17;
      this.btSearchFile.Text = "...";
      this.btSearchFile.UseVisualStyleBackColor = true;
      // 
      // txtFileName
      // 
      this.txtFileName.Enabled = false;
      this.txtFileName.Location = new System.Drawing.Point(137, 27);
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Size = new System.Drawing.Size(590, 22);
      this.txtFileName.TabIndex = 16;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(10, 29);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(56, 17);
      this.label10.TabIndex = 15;
      this.label10.Text = "Arquivo";
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // btImp
      // 
      this.btImp.Location = new System.Drawing.Point(466, 64);
      this.btImp.Name = "btImp";
      this.btImp.Size = new System.Drawing.Size(241, 23);
      this.btImp.TabIndex = 31;
      this.btImp.Text = "Importar Colaborador";
      this.btImp.UseVisualStyleBackColor = true;
      this.btImp.Click += new System.EventHandler(this.BtImp_Click);
      // 
      // MenuPrincipal
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(804, 631);
      this.Controls.Add(this.btImp);
      this.Controls.Add(this.grpBD);
      this.Controls.Add(this.cboType);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.cboMode);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btChange);
      this.Controls.Add(this.txtEma);
      this.Controls.Add(this.lblUrl);
      this.Controls.Add(this.txtUrl);
      this.Controls.Add(this.lblEma);
      this.Controls.Add(this.grpArq);
      this.Name = "MenuPrincipal";
      this.Text = "MenuPrincipal";
      this.Load += new System.EventHandler(this.MenuPrincipal_Load);
      this.grpBD.ResumeLayout(false);
      this.grpBD.PerformLayout();
      this.grpArq.ResumeLayout(false);
      this.grpArq.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtEma;
    private System.Windows.Forms.Label lblUrl;
    private System.Windows.Forms.TextBox txtUrl;
    private System.Windows.Forms.Label lblEma;
    private System.Windows.Forms.Button btChange;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cboMode;
    private System.Windows.Forms.ComboBox cboType;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.GroupBox grpBD;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txtSql;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox chkOra;
    private System.Windows.Forms.TextBox txtDefault;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtHostName;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtUser;
    private System.Windows.Forms.GroupBox grpArq;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Button btSearchFile;
    private System.Windows.Forms.TextBox txtFileName;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.Button btSaveFile;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button btSave;
    private System.Windows.Forms.Button btImp;
  }
}