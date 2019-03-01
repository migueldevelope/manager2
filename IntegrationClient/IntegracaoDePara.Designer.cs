namespace IntegrationClient
{
  partial class IntegracaoDePara
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.rbEsc = new System.Windows.Forms.RadioButton();
      this.rbCargo = new System.Windows.Forms.RadioButton();
      this.rbEstab = new System.Windows.Forms.RadioButton();
      this.rbEmp = new System.Windows.Forms.RadioButton();
      this.lblMsg = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btMatch = new System.Windows.Forms.Button();
      this.btnFil = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.txtFil = new System.Windows.Forms.TextBox();
      this.chkT = new System.Windows.Forms.CheckBox();
      this.dgvD = new System.Windows.Forms.DataGridView();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgvP = new System.Windows.Forms.DataGridView();
      this.IdIntegration = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DePara = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvD)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvP)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.rbEsc);
      this.groupBox1.Controls.Add(this.rbCargo);
      this.groupBox1.Controls.Add(this.rbEstab);
      this.groupBox1.Controls.Add(this.rbEmp);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(776, 60);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Tipo da Integração";
      // 
      // rbEsc
      // 
      this.rbEsc.AutoSize = true;
      this.rbEsc.Location = new System.Drawing.Point(310, 22);
      this.rbEsc.Name = "rbEsc";
      this.rbEsc.Size = new System.Drawing.Size(111, 21);
      this.rbEsc.TabIndex = 3;
      this.rbEsc.Text = "Escolaridade";
      this.rbEsc.UseVisualStyleBackColor = true;
      this.rbEsc.CheckedChanged += new System.EventHandler(this.RbEsc_CheckedChanged);
      // 
      // rbCargo
      // 
      this.rbCargo.AutoSize = true;
      this.rbCargo.Location = new System.Drawing.Point(237, 22);
      this.rbCargo.Name = "rbCargo";
      this.rbCargo.Size = new System.Drawing.Size(67, 21);
      this.rbCargo.TabIndex = 2;
      this.rbCargo.Text = "Cargo";
      this.rbCargo.UseVisualStyleBackColor = true;
      this.rbCargo.CheckedChanged += new System.EventHandler(this.RbCargo_CheckedChanged);
      // 
      // rbEstab
      // 
      this.rbEstab.AutoSize = true;
      this.rbEstab.Location = new System.Drawing.Point(98, 22);
      this.rbEstab.Name = "rbEstab";
      this.rbEstab.Size = new System.Drawing.Size(133, 21);
      this.rbEstab.TabIndex = 1;
      this.rbEstab.Text = "Estabelecimento";
      this.rbEstab.UseVisualStyleBackColor = true;
      this.rbEstab.CheckedChanged += new System.EventHandler(this.RbEstab_CheckedChanged);
      // 
      // rbEmp
      // 
      this.rbEmp.AutoSize = true;
      this.rbEmp.Checked = true;
      this.rbEmp.Location = new System.Drawing.Point(7, 22);
      this.rbEmp.Name = "rbEmp";
      this.rbEmp.Size = new System.Drawing.Size(85, 21);
      this.rbEmp.TabIndex = 0;
      this.rbEmp.TabStop = true;
      this.rbEmp.Text = "Empresa";
      this.rbEmp.UseVisualStyleBackColor = true;
      this.rbEmp.CheckedChanged += new System.EventHandler(this.RbEmp_CheckedChanged);
      // 
      // lblMsg
      // 
      this.lblMsg.AutoSize = true;
      this.lblMsg.Location = new System.Drawing.Point(12, 79);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new System.Drawing.Size(46, 17);
      this.lblMsg.TabIndex = 1;
      this.lblMsg.Text = "label1";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btMatch);
      this.groupBox2.Controls.Add(this.btnFil);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.txtFil);
      this.groupBox2.Controls.Add(this.chkT);
      this.groupBox2.Controls.Add(this.dgvD);
      this.groupBox2.Controls.Add(this.dgvP);
      this.groupBox2.Location = new System.Drawing.Point(12, 99);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(776, 601);
      this.groupBox2.TabIndex = 2;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "DE -> PARA";
      // 
      // btMatch
      // 
      this.btMatch.Location = new System.Drawing.Point(695, 570);
      this.btMatch.Name = "btMatch";
      this.btMatch.Size = new System.Drawing.Size(75, 23);
      this.btMatch.TabIndex = 6;
      this.btMatch.Text = "Atribuir";
      this.btMatch.UseVisualStyleBackColor = true;
      // 
      // btnFil
      // 
      this.btnFil.Location = new System.Drawing.Point(696, 262);
      this.btnFil.Name = "btnFil";
      this.btnFil.Size = new System.Drawing.Size(75, 23);
      this.btnFil.TabIndex = 5;
      this.btnFil.Text = "Filtrar";
      this.btnFil.UseVisualStyleBackColor = true;
      this.btnFil.Click += new System.EventHandler(this.BtnFil_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 265);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 17);
      this.label1.TabIndex = 4;
      this.label1.Text = "Filtrar";
      // 
      // txtFil
      // 
      this.txtFil.Location = new System.Drawing.Point(56, 262);
      this.txtFil.Name = "txtFil";
      this.txtFil.Size = new System.Drawing.Size(634, 22);
      this.txtFil.TabIndex = 3;
      // 
      // chkT
      // 
      this.chkT.AutoSize = true;
      this.chkT.Location = new System.Drawing.Point(7, 22);
      this.chkT.Name = "chkT";
      this.chkT.Size = new System.Drawing.Size(78, 21);
      this.chkT.TabIndex = 2;
      this.chkT.Text = "Todos?";
      this.chkT.UseVisualStyleBackColor = true;
      this.chkT.CheckedChanged += new System.EventHandler(this.ChkT_CheckedChanged);
      // 
      // dgvD
      // 
      this.dgvD.AllowUserToAddRows = false;
      this.dgvD.AllowUserToDeleteRows = false;
      this.dgvD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3});
      this.dgvD.Location = new System.Drawing.Point(7, 291);
      this.dgvD.Name = "dgvD";
      this.dgvD.RowTemplate.Height = 24;
      this.dgvD.Size = new System.Drawing.Size(764, 273);
      this.dgvD.TabIndex = 1;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.DataPropertyName = "_id";
      this.dataGridViewTextBoxColumn1.Frozen = true;
      this.dataGridViewTextBoxColumn1.HeaderText = "Id";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Visible = false;
      // 
      // dataGridViewTextBoxColumn3
      // 
      this.dataGridViewTextBoxColumn3.DataPropertyName = "name";
      this.dataGridViewTextBoxColumn3.HeaderText = "Nome";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      // 
      // dgvP
      // 
      this.dgvP.AllowUserToAddRows = false;
      this.dgvP.AllowUserToDeleteRows = false;
      this.dgvP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvP.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdIntegration,
            this.Id,
            this.Nome,
            this.DePara});
      this.dgvP.Location = new System.Drawing.Point(7, 48);
      this.dgvP.MultiSelect = false;
      this.dgvP.Name = "dgvP";
      this.dgvP.RowTemplate.Height = 24;
      this.dgvP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvP.Size = new System.Drawing.Size(764, 211);
      this.dgvP.TabIndex = 0;
      // 
      // IdIntegration
      // 
      this.IdIntegration.DataPropertyName = "IdIntegration";
      this.IdIntegration.Frozen = true;
      this.IdIntegration.HeaderText = "IdIntegration";
      this.IdIntegration.Name = "IdIntegration";
      this.IdIntegration.ReadOnly = true;
      this.IdIntegration.Visible = false;
      // 
      // Id
      // 
      this.Id.DataPropertyName = "IdCompany";
      this.Id.HeaderText = "IdCompany";
      this.Id.Name = "Id";
      this.Id.Visible = false;
      // 
      // Nome
      // 
      this.Nome.DataPropertyName = "NameIntegration";
      this.Nome.HeaderText = "Empresa Folha";
      this.Nome.Name = "Nome";
      this.Nome.ReadOnly = true;
      this.Nome.Width = 300;
      // 
      // DePara
      // 
      this.DePara.DataPropertyName = "NameCompany";
      this.DePara.HeaderText = "Empresa";
      this.DePara.Name = "DePara";
      this.DePara.Width = 300;
      // 
      // IntegracaoDePara
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 704);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.lblMsg);
      this.Controls.Add(this.groupBox1);
      this.Name = "IntegracaoDePara";
      this.Text = "IntegracaoDePara";
      this.Load += new System.EventHandler(this.IntegracaoDePara_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvD)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvP)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton rbEstab;
    private System.Windows.Forms.RadioButton rbEmp;
    private System.Windows.Forms.RadioButton rbEsc;
    private System.Windows.Forms.RadioButton rbCargo;
    private System.Windows.Forms.Label lblMsg;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.DataGridView dgvP;
    private System.Windows.Forms.DataGridView dgvD;
    private System.Windows.Forms.Button btMatch;
    private System.Windows.Forms.Button btnFil;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtFil;
    private System.Windows.Forms.CheckBox chkT;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private System.Windows.Forms.DataGridViewTextBoxColumn IdIntegration;
    private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    private System.Windows.Forms.DataGridViewTextBoxColumn Nome;
    private System.Windows.Forms.DataGridViewTextBoxColumn DePara;
  }
}