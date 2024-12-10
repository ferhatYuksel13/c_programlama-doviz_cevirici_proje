namespace DovizCevirici
{
    partial class Form1
    {
        /// <summary>
        /// Gerekli bileşenler
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Kullanıcı kontrolündeki kullanılan tüm kaynakları temizler.
        /// </summary>
        /// <param name="disposing">Managed kaynakların temizlenmesi gerektiğini belirten bir değer.</param>
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
        /// Tasarım için gerekli olan yöntem. 
        /// Bu yöntem, Windows Form Designer tarafından çağrılır.
        /// Kodunuzu burada değiştirmeyin. 
        /// Bu kodu, Windows Form Designer tarafından eklenip, silinmesini sağlar.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbFromCurrency = new System.Windows.Forms.ComboBox();
            this.cmbToCurrency = new System.Windows.Forms.ComboBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.btnUpdateRates = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblLastUpdated = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbFromCurrency
            // 
            this.cmbFromCurrency.BackColor = System.Drawing.SystemColors.MenuBar;
            this.cmbFromCurrency.FormattingEnabled = true;
            this.cmbFromCurrency.Location = new System.Drawing.Point(50, 50);
            this.cmbFromCurrency.Name = "cmbFromCurrency";
            this.cmbFromCurrency.Size = new System.Drawing.Size(121, 24);
            this.cmbFromCurrency.TabIndex = 0;
            // 
            // cmbToCurrency
            // 
            this.cmbToCurrency.BackColor = System.Drawing.SystemColors.MenuBar;
            this.cmbToCurrency.FormattingEnabled = true;
            this.cmbToCurrency.Location = new System.Drawing.Point(200, 50);
            this.cmbToCurrency.Name = "cmbToCurrency";
            this.cmbToCurrency.Size = new System.Drawing.Size(121, 24);
            this.cmbToCurrency.TabIndex = 1;
            // 
            // txtAmount
            // 
            this.txtAmount.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtAmount.Location = new System.Drawing.Point(50, 100);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(100, 22);
            this.txtAmount.TabIndex = 2;
            // 
            // btnUpdateRates
            // 
            this.btnUpdateRates.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUpdateRates.Location = new System.Drawing.Point(50, 150);
            this.btnUpdateRates.Name = "btnUpdateRates";
            this.btnUpdateRates.Size = new System.Drawing.Size(121, 30);
            this.btnUpdateRates.TabIndex = 3;
            this.btnUpdateRates.Text = " Güncelle";
            this.btnUpdateRates.UseVisualStyleBackColor = false;
            this.btnUpdateRates.Click += new System.EventHandler(this.btnUpdateRates_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnConvert.Location = new System.Drawing.Point(200, 150);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(121, 30);
            this.btnConvert.TabIndex = 4;
            this.btnConvert.Text = "Çevir";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.BackColor = System.Drawing.SystemColors.MenuBar;
            this.lblResult.Location = new System.Drawing.Point(50, 200);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(180, 16);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Döviz Çeviriciye Hoşgeldiniz!";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.AutoSize = true;
            this.lblLastUpdated.BackColor = System.Drawing.SystemColors.MenuBar;
            this.lblLastUpdated.Location = new System.Drawing.Point(50, 230);
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(210, 16);
            this.lblLastUpdated.TabIndex = 6;
            this.lblLastUpdated.Text = "Son güncelleme: Henüz yapılmadı";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(199)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.lblLastUpdated);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnUpdateRates);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.cmbToCurrency);
            this.Controls.Add(this.cmbFromCurrency);
            this.Name = "Form1";
            this.Text = "Döviz Çevirici";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbFromCurrency;
        private System.Windows.Forms.ComboBox cmbToCurrency;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Button btnUpdateRates;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblLastUpdated;
    }
}
