﻿namespace IntegrationClient
{
  partial class Menu
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
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.impCol = new System.Windows.Forms.ToolStripMenuItem();
      this.impColCfg = new System.Windows.Forms.ToolStripMenuItem();
      this.impColPrc = new System.Windows.Forms.ToolStripMenuItem();
      this.map = new System.Windows.Forms.ToolStripMenuItem();
      this.mapImpAna = new System.Windows.Forms.ToolStripMenuItem();
      this.mapImpExl = new System.Windows.Forms.ToolStripMenuItem();
      this.mapCarDiv0 = new System.Windows.Forms.ToolStripSeparator();
      this.mapExp = new System.Windows.Forms.ToolStripMenuItem();
      this.perfil = new System.Windows.Forms.ToolStripMenuItem();
      this.perfilTrocar = new System.Windows.Forms.ToolStripMenuItem();
      this.perfilSair = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tbarUrl = new System.Windows.Forms.ToolStripLabel();
      this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tbarAccount = new System.Windows.Forms.ToolStripLabel();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.tbarPerson = new System.Windows.Forms.ToolStripLabel();
      this.menuStrip1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.impCol,
            this.map,
            this.perfil});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(800, 28);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // impCol
      // 
      this.impCol.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.impColCfg,
            this.impColPrc});
      this.impCol.Name = "impCol";
      this.impCol.Size = new System.Drawing.Size(221, 24);
      this.impCol.Text = "&Importação de Colaboradores";
      // 
      // impColCfg
      // 
      this.impColCfg.Name = "impColCfg";
      this.impColCfg.Size = new System.Drawing.Size(207, 26);
      this.impColCfg.Text = "&Configuração";
      this.impColCfg.Click += new System.EventHandler(this.ImpColCfg_Click);
      // 
      // impColPrc
      // 
      this.impColPrc.Name = "impColPrc";
      this.impColPrc.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
      this.impColPrc.Size = new System.Drawing.Size(207, 26);
      this.impColPrc.Text = "&Importação";
      this.impColPrc.Click += new System.EventHandler(this.ImpColImp_Click);
      // 
      // map
      // 
      this.map.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapImpAna,
            this.mapImpExl,
            this.mapCarDiv0,
            this.mapExp});
      this.map.Name = "map";
      this.map.Size = new System.Drawing.Size(65, 24);
      this.map.Text = "&Mapas";
      // 
      // mapImpAna
      // 
      this.mapImpAna.Name = "mapImpAna";
      this.mapImpAna.Size = new System.Drawing.Size(235, 26);
      this.mapImpAna.Text = "Importação do &Analisa";
      this.mapImpAna.Click += new System.EventHandler(this.MapImpAna_Click);
      // 
      // mapImpExl
      // 
      this.mapImpExl.Name = "mapImpExl";
      this.mapImpExl.Size = new System.Drawing.Size(235, 26);
      this.mapImpExl.Text = "Importação do &Excel";
      this.mapImpExl.Click += new System.EventHandler(this.MapImpExl_Click);
      // 
      // mapCarDiv0
      // 
      this.mapCarDiv0.Name = "mapCarDiv0";
      this.mapCarDiv0.Size = new System.Drawing.Size(232, 6);
      // 
      // mapExp
      // 
      this.mapExp.Name = "mapExp";
      this.mapExp.Size = new System.Drawing.Size(235, 26);
      this.mapExp.Text = "&Exportação da Conta";
      this.mapExp.Click += new System.EventHandler(this.MapExp_Click);
      // 
      // perfil
      // 
      this.perfil.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perfilTrocar,
            this.perfilSair});
      this.perfil.Name = "perfil";
      this.perfil.Size = new System.Drawing.Size(54, 24);
      this.perfil.Text = "&Perfil";
      // 
      // perfilTrocar
      // 
      this.perfilTrocar.Name = "perfilTrocar";
      this.perfilTrocar.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
      this.perfilTrocar.Size = new System.Drawing.Size(182, 26);
      this.perfilTrocar.Text = "&Trocar";
      this.perfilTrocar.Click += new System.EventHandler(this.PerfilTrocar_Click);
      // 
      // perfilSair
      // 
      this.perfilSair.Name = "perfilSair";
      this.perfilSair.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
      this.perfilSair.Size = new System.Drawing.Size(182, 26);
      this.perfilSair.Text = "Sai&r";
      this.perfilSair.Click += new System.EventHandler(this.PerfilSair_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbarUrl,
            this.toolStripLabel2,
            this.toolStripSeparator1,
            this.tbarAccount,
            this.toolStripSeparator2,
            this.tbarPerson});
      this.toolStrip1.Location = new System.Drawing.Point(0, 28);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(800, 25);
      this.toolStrip1.TabIndex = 2;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tbarUrl
      // 
      this.tbarUrl.Name = "tbarUrl";
      this.tbarUrl.Size = new System.Drawing.Size(134, 22);
      this.tbarUrl.Text = "Url do Servidor: {0}";
      // 
      // toolStripLabel2
      // 
      this.toolStripLabel2.Name = "toolStripLabel2";
      this.toolStripLabel2.Size = new System.Drawing.Size(0, 22);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // tbarAccount
      // 
      this.tbarAccount.Name = "tbarAccount";
      this.tbarAccount.Size = new System.Drawing.Size(73, 22);
      this.tbarAccount.Text = "Conta: {0}";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // tbarPerson
      // 
      this.tbarPerson.Name = "tbarPerson";
      this.tbarPerson.Size = new System.Drawing.Size(84, 22);
      this.tbarPerson.Text = "Usuário: {0}";
      // 
      // Menu
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.IsMdiContainer = true;
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "Menu";
      this.Text = "Menu";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.Menu_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem impCol;
    private System.Windows.Forms.ToolStripMenuItem impColCfg;
    private System.Windows.Forms.ToolStripMenuItem perfil;
    private System.Windows.Forms.ToolStripMenuItem perfilTrocar;
    private System.Windows.Forms.ToolStripMenuItem perfilSair;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripLabel tbarUrl;
    private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripLabel tbarPerson;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripLabel tbarAccount;
    private System.Windows.Forms.ToolStripMenuItem impColPrc;
    private System.Windows.Forms.ToolStripMenuItem map;
    private System.Windows.Forms.ToolStripMenuItem mapImpAna;
    private System.Windows.Forms.ToolStripMenuItem mapImpExl;
    private System.Windows.Forms.ToolStripSeparator mapCarDiv0;
    private System.Windows.Forms.ToolStripMenuItem mapExp;
  }
}