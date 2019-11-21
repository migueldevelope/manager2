namespace IntegrationClient
{
  partial class ImportacaoConfigurar
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
      this.CboProc = new System.Windows.Forms.ComboBox();
      this.label12 = new System.Windows.Forms.Label();
      this.CboType = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.CboMode = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.grpBD = new System.Windows.Forms.GroupBox();
      this.txtStr = new System.Windows.Forms.TextBox();
      this.lblStr = new System.Windows.Forms.Label();
      this.CboDatabaseType = new System.Windows.Forms.ComboBox();
      this.btSave = new System.Windows.Forms.Button();
      this.label9 = new System.Windows.Forms.Label();
      this.txtSql = new System.Windows.Forms.TextBox();
      this.lblDefault = new System.Windows.Forms.Label();
      this.txtDefault = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.lblPassword = new System.Windows.Forms.Label();
      this.txtHostName = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblHostName = new System.Windows.Forms.Label();
      this.lblUser = new System.Windows.Forms.Label();
      this.txtUser = new System.Windows.Forms.TextBox();
      this.grpArq = new System.Windows.Forms.GroupBox();
      this.txtSheetName = new System.Windows.Forms.TextBox();
      this.btSaveFile = new System.Windows.Forms.Button();
      this.lblSheetName = new System.Windows.Forms.Label();
      this.btSearchFile = new System.Windows.Forms.Button();
      this.txtFileName = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.grpApi = new System.Windows.Forms.GroupBox();
      this.btSaveApi = new System.Windows.Forms.Button();
      this.txtIdApi = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.CboChave = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.lblDatCul = new System.Windows.Forms.Label();
      this.txtDatCul = new System.Windows.Forms.TextBox();
      this.grpBD.SuspendLayout();
      this.grpArq.SuspendLayout();
      this.grpApi.SuspendLayout();
      this.SuspendLayout();
      // 
      // CboProc
      // 
      this.CboProc.FormattingEnabled = true;
      this.CboProc.Location = new System.Drawing.Point(152, 12);
      this.CboProc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.CboProc.Name = "CboProc";
      this.CboProc.Size = new System.Drawing.Size(265, 24);
      this.CboProc.TabIndex = 1;
      this.CboProc.SelectedIndexChanged += new System.EventHandler(this.CboProc_SelectedIndexChanged);
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(21, 15);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(119, 17);
      this.label12.TabIndex = 0;
      this.label12.Text = "Tipo de Processo";
      // 
      // CboType
      // 
      this.CboType.FormattingEnabled = true;
      this.CboType.Location = new System.Drawing.Point(562, 12);
      this.CboType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.CboType.Name = "CboType";
      this.CboType.Size = new System.Drawing.Size(250, 24);
      this.CboType.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(429, 15);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(127, 17);
      this.label3.TabIndex = 2;
      this.label3.Text = "Tipo de Integração";
      // 
      // CboMode
      // 
      this.CboMode.FormattingEnabled = true;
      this.CboMode.Location = new System.Drawing.Point(152, 40);
      this.CboMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.CboMode.Name = "CboMode";
      this.CboMode.Size = new System.Drawing.Size(265, 24);
      this.CboMode.TabIndex = 4;
      this.CboMode.SelectedIndexChanged += new System.EventHandler(this.CboMode_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 43);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(134, 17);
      this.label2.TabIndex = 35;
      this.label2.Text = "Modo de Integração";
      // 
      // grpBD
      // 
      this.grpBD.Controls.Add(this.txtStr);
      this.grpBD.Controls.Add(this.lblStr);
      this.grpBD.Controls.Add(this.CboDatabaseType);
      this.grpBD.Controls.Add(this.btSave);
      this.grpBD.Controls.Add(this.label9);
      this.grpBD.Controls.Add(this.txtSql);
      this.grpBD.Controls.Add(this.lblDefault);
      this.grpBD.Controls.Add(this.txtDefault);
      this.grpBD.Controls.Add(this.label8);
      this.grpBD.Controls.Add(this.lblPassword);
      this.grpBD.Controls.Add(this.txtHostName);
      this.grpBD.Controls.Add(this.txtPassword);
      this.grpBD.Controls.Add(this.lblHostName);
      this.grpBD.Controls.Add(this.lblUser);
      this.grpBD.Controls.Add(this.txtUser);
      this.grpBD.Location = new System.Drawing.Point(9, 106);
      this.grpBD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.grpBD.Name = "grpBD";
      this.grpBD.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.grpBD.Size = new System.Drawing.Size(803, 405);
      this.grpBD.TabIndex = 42;
      this.grpBD.TabStop = false;
      this.grpBD.Text = "Banco de Dados";
      this.grpBD.Visible = false;
      // 
      // txtStr
      // 
      this.txtStr.Location = new System.Drawing.Point(107, 54);
      this.txtStr.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtStr.Multiline = true;
      this.txtStr.Name = "txtStr";
      this.txtStr.Size = new System.Drawing.Size(685, 54);
      this.txtStr.TabIndex = 60;
      // 
      // lblStr
      // 
      this.lblStr.AutoSize = true;
      this.lblStr.Location = new System.Drawing.Point(35, 59);
      this.lblStr.Name = "lblStr";
      this.lblStr.Size = new System.Drawing.Size(65, 34);
      this.lblStr.TabIndex = 60;
      this.lblStr.Text = "String de\r\nConexão";
      // 
      // CboDatabaseType
      // 
      this.CboDatabaseType.FormattingEnabled = true;
      this.CboDatabaseType.Location = new System.Drawing.Point(107, 27);
      this.CboDatabaseType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.CboDatabaseType.Name = "CboDatabaseType";
      this.CboDatabaseType.Size = new System.Drawing.Size(255, 24);
      this.CboDatabaseType.TabIndex = 59;
      this.CboDatabaseType.SelectedIndexChanged += new System.EventHandler(this.CboDatabaseType_SelectedIndexChanged);
      // 
      // btSave
      // 
      this.btSave.Location = new System.Drawing.Point(722, 373);
      this.btSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btSave.Name = "btSave";
      this.btSave.Size = new System.Drawing.Size(69, 23);
      this.btSave.TabIndex = 58;
      this.btSave.Text = "Salvar";
      this.btSave.UseVisualStyleBackColor = true;
      this.btSave.Click += new System.EventHandler(this.BtSave_Click);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(64, 118);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(36, 17);
      this.label9.TabIndex = 57;
      this.label9.Text = "SQL";
      // 
      // txtSql
      // 
      this.txtSql.Font = new System.Drawing.Font("Tahoma", 8.25F);
      this.txtSql.Location = new System.Drawing.Point(107, 114);
      this.txtSql.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtSql.Multiline = true;
      this.txtSql.Name = "txtSql";
      this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.txtSql.Size = new System.Drawing.Size(685, 252);
      this.txtSql.TabIndex = 61;
      // 
      // lblDefault
      // 
      this.lblDefault.AutoSize = true;
      this.lblDefault.Location = new System.Drawing.Point(368, 89);
      this.lblDefault.Name = "lblDefault";
      this.lblDefault.Size = new System.Drawing.Size(90, 17);
      this.lblDefault.TabIndex = 54;
      this.lblDefault.Text = "Base Padrão";
      // 
      // txtDefault
      // 
      this.txtDefault.Font = new System.Drawing.Font("Tahoma", 8.25F);
      this.txtDefault.Location = new System.Drawing.Point(464, 84);
      this.txtDefault.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtDefault.Name = "txtDefault";
      this.txtDefault.Size = new System.Drawing.Size(327, 24);
      this.txtDefault.TabIndex = 53;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(64, 31);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(36, 17);
      this.label8.TabIndex = 46;
      this.label8.Text = "Tipo";
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new System.Drawing.Point(51, 87);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(49, 17);
      this.lblPassword.TabIndex = 52;
      this.lblPassword.Text = "Senha";
      // 
      // txtHostName
      // 
      this.txtHostName.Location = new System.Drawing.Point(107, 57);
      this.txtHostName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtHostName.Name = "txtHostName";
      this.txtHostName.Size = new System.Drawing.Size(255, 22);
      this.txtHostName.TabIndex = 47;
      // 
      // txtPassword
      // 
      this.txtPassword.Font = new System.Drawing.Font("Tahoma", 8.25F);
      this.txtPassword.Location = new System.Drawing.Point(107, 84);
      this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(255, 24);
      this.txtPassword.TabIndex = 51;
      // 
      // lblHostName
      // 
      this.lblHostName.AutoSize = true;
      this.lblHostName.Location = new System.Drawing.Point(28, 59);
      this.lblHostName.Name = "lblHostName";
      this.lblHostName.Size = new System.Drawing.Size(72, 17);
      this.lblHostName.TabIndex = 48;
      this.lblHostName.Text = "Hostname";
      // 
      // lblUser
      // 
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new System.Drawing.Point(401, 57);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new System.Drawing.Size(57, 17);
      this.lblUser.TabIndex = 50;
      this.lblUser.Text = "Usuário";
      // 
      // txtUser
      // 
      this.txtUser.Location = new System.Drawing.Point(464, 54);
      this.txtUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtUser.Name = "txtUser";
      this.txtUser.Size = new System.Drawing.Size(327, 22);
      this.txtUser.TabIndex = 49;
      // 
      // grpArq
      // 
      this.grpArq.Controls.Add(this.txtSheetName);
      this.grpArq.Controls.Add(this.btSaveFile);
      this.grpArq.Controls.Add(this.lblSheetName);
      this.grpArq.Controls.Add(this.btSearchFile);
      this.grpArq.Controls.Add(this.txtFileName);
      this.grpArq.Controls.Add(this.label10);
      this.grpArq.Location = new System.Drawing.Point(9, 106);
      this.grpArq.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.grpArq.Name = "grpArq";
      this.grpArq.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.grpArq.Size = new System.Drawing.Size(803, 405);
      this.grpArq.TabIndex = 43;
      this.grpArq.TabStop = false;
      this.grpArq.Text = "Arquivo CSV";
      this.grpArq.Visible = false;
      // 
      // txtSheetName
      // 
      this.txtSheetName.Location = new System.Drawing.Point(137, 54);
      this.txtSheetName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtSheetName.Name = "txtSheetName";
      this.txtSheetName.Size = new System.Drawing.Size(255, 22);
      this.txtSheetName.TabIndex = 18;
      // 
      // btSaveFile
      // 
      this.btSaveFile.Location = new System.Drawing.Point(722, 57);
      this.btSaveFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btSaveFile.Name = "btSaveFile";
      this.btSaveFile.Size = new System.Drawing.Size(69, 23);
      this.btSaveFile.TabIndex = 20;
      this.btSaveFile.Text = "Salvar";
      this.btSaveFile.UseVisualStyleBackColor = true;
      this.btSaveFile.Click += new System.EventHandler(this.BtSaveFile_Click);
      // 
      // lblSheetName
      // 
      this.lblSheetName.AutoSize = true;
      this.lblSheetName.Location = new System.Drawing.Point(12, 57);
      this.lblSheetName.Name = "lblSheetName";
      this.lblSheetName.Size = new System.Drawing.Size(119, 17);
      this.lblSheetName.TabIndex = 18;
      this.lblSheetName.Text = "Nome da Planilha";
      // 
      // btSearchFile
      // 
      this.btSearchFile.Location = new System.Drawing.Point(733, 27);
      this.btSearchFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btSearchFile.Name = "btSearchFile";
      this.btSearchFile.Size = new System.Drawing.Size(59, 25);
      this.btSearchFile.TabIndex = 17;
      this.btSearchFile.Text = "...";
      this.btSearchFile.UseVisualStyleBackColor = true;
      // 
      // txtFileName
      // 
      this.txtFileName.Location = new System.Drawing.Point(137, 27);
      this.txtFileName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Size = new System.Drawing.Size(591, 22);
      this.txtFileName.TabIndex = 16;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(75, 31);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(56, 17);
      this.label10.TabIndex = 15;
      this.label10.Text = "Arquivo";
      // 
      // grpApi
      // 
      this.grpApi.Controls.Add(this.btSaveApi);
      this.grpApi.Controls.Add(this.txtIdApi);
      this.grpApi.Controls.Add(this.label4);
      this.grpApi.Location = new System.Drawing.Point(9, 106);
      this.grpApi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.grpApi.Name = "grpApi";
      this.grpApi.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.grpApi.Size = new System.Drawing.Size(803, 405);
      this.grpApi.TabIndex = 44;
      this.grpApi.TabStop = false;
      this.grpApi.Text = "Application Interface";
      this.grpApi.Visible = false;
      // 
      // btSaveApi
      // 
      this.btSaveApi.Location = new System.Drawing.Point(659, 59);
      this.btSaveApi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btSaveApi.Name = "btSaveApi";
      this.btSaveApi.Size = new System.Drawing.Size(69, 23);
      this.btSaveApi.TabIndex = 70;
      this.btSaveApi.Text = "Salvar";
      this.btSaveApi.UseVisualStyleBackColor = true;
      this.btSaveApi.Click += new System.EventHandler(this.BtSaveApi_Click);
      // 
      // txtIdApi
      // 
      this.txtIdApi.Location = new System.Drawing.Point(137, 27);
      this.txtIdApi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtIdApi.Name = "txtIdApi";
      this.txtIdApi.Size = new System.Drawing.Size(591, 22);
      this.txtIdApi.TabIndex = 16;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(8, 30);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(117, 17);
      this.label4.TabIndex = 15;
      this.label4.Text = "ID API Integration";
      // 
      // CboChave
      // 
      this.CboChave.FormattingEnabled = true;
      this.CboChave.Location = new System.Drawing.Point(562, 43);
      this.CboChave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.CboChave.Name = "CboChave";
      this.CboChave.Size = new System.Drawing.Size(250, 24);
      this.CboChave.TabIndex = 5;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(456, 47);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 17);
      this.label1.TabIndex = 45;
      this.label1.Text = "Tipo de Chave";
      // 
      // lblDatCul
      // 
      this.lblDatCul.AutoSize = true;
      this.lblDatCul.Location = new System.Drawing.Point(372, 78);
      this.lblDatCul.Name = "lblDatCul";
      this.lblDatCul.Size = new System.Drawing.Size(184, 17);
      this.lblDatCul.TabIndex = 46;
      this.lblDatCul.Text = "Cultura da Origem de Datas";
      // 
      // txtDatCul
      // 
      this.txtDatCul.Location = new System.Drawing.Point(562, 75);
      this.txtDatCul.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtDatCul.Name = "txtDatCul";
      this.txtDatCul.Size = new System.Drawing.Size(250, 22);
      this.txtDatCul.TabIndex = 6;
      // 
      // ImportacaoConfigurar
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(819, 517);
      this.Controls.Add(this.txtDatCul);
      this.Controls.Add(this.lblDatCul);
      this.Controls.Add(this.CboChave);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.CboProc);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.CboType);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.CboMode);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.grpBD);
      this.Controls.Add(this.grpArq);
      this.Controls.Add(this.grpApi);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ImportacaoConfigurar";
      this.Text = "ImportacaoConfigurar";
      this.Load += new System.EventHandler(this.ImportacaoConfigurar_Load);
      this.grpBD.ResumeLayout(false);
      this.grpBD.PerformLayout();
      this.grpArq.ResumeLayout(false);
      this.grpArq.PerformLayout();
      this.grpApi.ResumeLayout(false);
      this.grpApi.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.ComboBox CboProc;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.ComboBox CboType;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox CboMode;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox grpBD;
    private System.Windows.Forms.ComboBox CboDatabaseType;
    private System.Windows.Forms.Button btSave;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txtSql;
    private System.Windows.Forms.Label lblDefault;
    private System.Windows.Forms.TextBox txtDefault;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtHostName;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblHostName;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.TextBox txtUser;
    private System.Windows.Forms.GroupBox grpArq;
    private System.Windows.Forms.Button btSaveFile;
    private System.Windows.Forms.Label lblSheetName;
    private System.Windows.Forms.Button btSearchFile;
    private System.Windows.Forms.TextBox txtFileName;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox txtSheetName;
    private System.Windows.Forms.TextBox txtStr;
    private System.Windows.Forms.Label lblStr;
    private System.Windows.Forms.GroupBox grpApi;
    private System.Windows.Forms.Button btSaveApi;
    private System.Windows.Forms.TextBox txtIdApi;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox CboChave;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblDatCul;
    private System.Windows.Forms.TextBox txtDatCul;
  }
}