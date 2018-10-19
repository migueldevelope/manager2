namespace IntegrationClient
{
  partial class Form1
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
      this.button2 = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btSaveCnx = new System.Windows.Forms.Button();
      this.btSyncSkill = new System.Windows.Forms.Button();
      this.btImpMap = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(291, 222);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(254, 23);
      this.button2.TabIndex = 30;
      this.button2.Text = "Importar Colaborador";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btSaveCnx);
      this.panel1.Location = new System.Drawing.Point(12, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(368, 204);
      this.panel1.TabIndex = 29;
      // 
      // btSaveCnx
      // 
      this.btSaveCnx.Location = new System.Drawing.Point(279, 167);
      this.btSaveCnx.Name = "btSaveCnx";
      this.btSaveCnx.Size = new System.Drawing.Size(75, 23);
      this.btSaveCnx.TabIndex = 34;
      this.btSaveCnx.Text = "Salvar";
      this.btSaveCnx.UseVisualStyleBackColor = true;
      // 
      // btSyncSkill
      // 
      this.btSyncSkill.Location = new System.Drawing.Point(12, 222);
      this.btSyncSkill.Name = "btSyncSkill";
      this.btSyncSkill.Size = new System.Drawing.Size(254, 23);
      this.btSyncSkill.TabIndex = 28;
      this.btSyncSkill.Text = "Sincronizar Competências";
      this.btSyncSkill.UseVisualStyleBackColor = true;
      // 
      // btImpMap
      // 
      this.btImpMap.Location = new System.Drawing.Point(12, 251);
      this.btImpMap.Name = "btImpMap";
      this.btImpMap.Size = new System.Drawing.Size(254, 23);
      this.btImpMap.TabIndex = 27;
      this.btImpMap.Text = "Importar Mapas";
      this.btImpMap.UseVisualStyleBackColor = true;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(291, 251);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 26;
      this.button1.Text = "Trocar";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.btSyncSkill);
      this.Controls.Add(this.btImpMap);
      this.Controls.Add(this.button1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btSaveCnx;
    private System.Windows.Forms.Button btSyncSkill;
    private System.Windows.Forms.Button btImpMap;
    private System.Windows.Forms.Button button1;
  }
}