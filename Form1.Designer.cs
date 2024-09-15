namespace BotBlum
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonStartBot = new System.Windows.Forms.Button();
            this.DebugTextBox = new System.Windows.Forms.RichTextBox();
            this.DebugTextBoxLabel = new System.Windows.Forms.Label();
            this.bearerTokenTextBox = new System.Windows.Forms.TextBox();
            this.BearerTokenLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.minPointLabel = new System.Windows.Forms.Label();
            this.minPointNumeric = new System.Windows.Forms.NumericUpDown();
            this.maxPointLabel = new System.Windows.Forms.Label();
            this.maxPointNumeric = new System.Windows.Forms.NumericUpDown();
            this.buttonStopBot = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minPointNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPointNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStartBot
            // 
            this.buttonStartBot.Location = new System.Drawing.Point(379, 143);
            this.buttonStartBot.Name = "buttonStartBot";
            this.buttonStartBot.Size = new System.Drawing.Size(79, 33);
            this.buttonStartBot.TabIndex = 0;
            this.buttonStartBot.Text = "Start Bot";
            this.buttonStartBot.UseVisualStyleBackColor = true;
            this.buttonStartBot.Click += new System.EventHandler(this.buttonStartBot_Click);
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.Location = new System.Drawing.Point(3, 22);
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.Size = new System.Drawing.Size(437, 126);
            this.DebugTextBox.TabIndex = 1;
            this.DebugTextBox.Text = "";
            // 
            // DebugTextBoxLabel
            // 
            this.DebugTextBoxLabel.AutoSize = true;
            this.DebugTextBoxLabel.Location = new System.Drawing.Point(3, 0);
            this.DebugTextBoxLabel.Name = "DebugTextBoxLabel";
            this.DebugTextBoxLabel.Size = new System.Drawing.Size(60, 13);
            this.DebugTextBoxLabel.TabIndex = 5;
            this.DebugTextBoxLabel.Text = "Debug Box";
            // 
            // bearerTokenTextBox
            // 
            this.bearerTokenTextBox.Location = new System.Drawing.Point(3, 16);
            this.bearerTokenTextBox.Name = "bearerTokenTextBox";
            this.bearerTokenTextBox.Size = new System.Drawing.Size(434, 20);
            this.bearerTokenTextBox.TabIndex = 2;
            // 
            // BearerTokenLabel
            // 
            this.BearerTokenLabel.AutoSize = true;
            this.BearerTokenLabel.Location = new System.Drawing.Point(3, 0);
            this.BearerTokenLabel.Name = "BearerTokenLabel";
            this.BearerTokenLabel.Size = new System.Drawing.Size(72, 13);
            this.BearerTokenLabel.TabIndex = 3;
            this.BearerTokenLabel.Text = "Bearer Token";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.BearerTokenLabel);
            this.flowLayoutPanel1.Controls.Add(this.bearerTokenTextBox);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(446, 47);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.DebugTextBoxLabel);
            this.flowLayoutPanel2.Controls.Add(this.splitter2);
            this.flowLayoutPanel2.Controls.Add(this.DebugTextBox);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(12, 203);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(446, 152);
            this.flowLayoutPanel2.TabIndex = 6;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 16);
            this.splitter2.Name = "splitter2";
            this.splitter2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.splitter2.Size = new System.Drawing.Size(434, 0);
            this.splitter2.TabIndex = 6;
            this.splitter2.TabStop = false;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label1);
            this.flowLayoutPanel3.Controls.Add(this.splitter1);
            this.flowLayoutPanel3.Controls.Add(this.minPointLabel);
            this.flowLayoutPanel3.Controls.Add(this.minPointNumeric);
            this.flowLayoutPanel3.Controls.Add(this.maxPointLabel);
            this.flowLayoutPanel3.Controls.Add(this.maxPointNumeric);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(12, 78);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(113, 98);
            this.flowLayoutPanel3.TabIndex = 7;
            this.flowLayoutPanel3.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Points Config";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(3, 16);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(100, 3);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // minPointLabel
            // 
            this.minPointLabel.AutoSize = true;
            this.minPointLabel.Location = new System.Drawing.Point(3, 22);
            this.minPointLabel.Name = "minPointLabel";
            this.minPointLabel.Size = new System.Drawing.Size(51, 13);
            this.minPointLabel.TabIndex = 1;
            this.minPointLabel.Text = "Min Point";
            // 
            // minPointNumeric
            // 
            this.minPointNumeric.Location = new System.Drawing.Point(3, 38);
            this.minPointNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.minPointNumeric.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.minPointNumeric.Name = "minPointNumeric";
            this.minPointNumeric.Size = new System.Drawing.Size(84, 20);
            this.minPointNumeric.TabIndex = 0;
            this.minPointNumeric.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
            // 
            // maxPointLabel
            // 
            this.maxPointLabel.AutoSize = true;
            this.maxPointLabel.Location = new System.Drawing.Point(3, 61);
            this.maxPointLabel.Name = "maxPointLabel";
            this.maxPointLabel.Size = new System.Drawing.Size(54, 13);
            this.maxPointLabel.TabIndex = 3;
            this.maxPointLabel.Text = "Max Point";
            // 
            // maxPointNumeric
            // 
            this.maxPointNumeric.Location = new System.Drawing.Point(3, 77);
            this.maxPointNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maxPointNumeric.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.maxPointNumeric.Name = "maxPointNumeric";
            this.maxPointNumeric.Size = new System.Drawing.Size(84, 20);
            this.maxPointNumeric.TabIndex = 2;
            this.maxPointNumeric.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // buttonStopBot
            // 
            this.buttonStopBot.Enabled = false;
            this.buttonStopBot.Location = new System.Drawing.Point(294, 143);
            this.buttonStopBot.Name = "buttonStopBot";
            this.buttonStopBot.Size = new System.Drawing.Size(79, 33);
            this.buttonStopBot.TabIndex = 8;
            this.buttonStopBot.Text = "Stop Bot";
            this.buttonStopBot.UseVisualStyleBackColor = true;
            this.buttonStopBot.Click += new System.EventHandler(this.buttonStopBot_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(470, 367);
            this.Controls.Add(this.buttonStopBot);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.buttonStartBot);
            this.Controls.Add(this.flowLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0.98D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blum Bot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minPointNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPointNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartBot;
        private System.Windows.Forms.RichTextBox DebugTextBox;
        private System.Windows.Forms.Label DebugTextBoxLabel;
        private System.Windows.Forms.TextBox bearerTokenTextBox;
        private System.Windows.Forms.Label BearerTokenLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.NumericUpDown minPointNumeric;
        private System.Windows.Forms.Label minPointLabel;
        private System.Windows.Forms.Label maxPointLabel;
        private System.Windows.Forms.NumericUpDown maxPointNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button buttonStopBot;
        private System.Windows.Forms.Splitter splitter2;
    }
}

