namespace IntegrationClient
{
  partial class ImportacaoImportar
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
      this.SuspendLayout();
      // 
      // btImp
      // 
      this.btImp.Location = new System.Drawing.Point(9, 10);
      this.btImp.Margin = new System.Windows.Forms.Padding(2);
      this.btImp.Name = "btImp";
      this.btImp.Size = new System.Drawing.Size(188, 19);
      this.btImp.TabIndex = 0;
      this.btImp.Text = "Executar Importação Manual";
      this.btImp.UseVisualStyleBackColor = true;
      this.btImp.Click += new System.EventHandler(this.BtImp_Click);
      // 
      // ImportacaoImportar
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(600, 366);
      this.Controls.Add(this.btImp);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ImportacaoImportar";
      this.Text = "ImportacaoImportar";
      this.Load += new System.EventHandler(this.ImportacaoImportar_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btImp;
  }
}