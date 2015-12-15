namespace LA3DataTransfer
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStaging = new System.Windows.Forms.Button();
            this.btnFSCollectors = new System.Windows.Forms.Button();
            this.btnFSCustomers = new System.Windows.Forms.Button();
            this.btnFSTheLot = new System.Windows.Forms.Button();
            this.btnFSAccount = new System.Windows.Forms.Button();
            this.btnFSPayment = new System.Windows.Forms.Button();
            this.btnFSPayoff = new System.Windows.Forms.Button();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnFilename = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 222);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(566, 22);
            this.statusStrip1.TabIndex = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // btnStaging
            // 
            this.btnStaging.Location = new System.Drawing.Point(12, 38);
            this.btnStaging.Name = "btnStaging";
            this.btnStaging.Size = new System.Drawing.Size(178, 23);
            this.btnStaging.TabIndex = 11;
            this.btnStaging.Text = "Load into Staging";
            this.btnStaging.UseVisualStyleBackColor = true;
            this.btnStaging.Click += new System.EventHandler(this.btnStaging_Click);
            // 
            // btnFSCollectors
            // 
            this.btnFSCollectors.Location = new System.Drawing.Point(12, 67);
            this.btnFSCollectors.Name = "btnFSCollectors";
            this.btnFSCollectors.Size = new System.Drawing.Size(138, 23);
            this.btnFSCollectors.TabIndex = 12;
            this.btnFSCollectors.Text = "Collectors from Staging";
            this.btnFSCollectors.UseVisualStyleBackColor = true;
            this.btnFSCollectors.Click += new System.EventHandler(this.btnFSCollectors_Click);
            // 
            // btnFSCustomers
            // 
            this.btnFSCustomers.Location = new System.Drawing.Point(12, 96);
            this.btnFSCustomers.Name = "btnFSCustomers";
            this.btnFSCustomers.Size = new System.Drawing.Size(138, 23);
            this.btnFSCustomers.TabIndex = 13;
            this.btnFSCustomers.Text = "Customers from Staging";
            this.btnFSCustomers.UseVisualStyleBackColor = true;
            this.btnFSCustomers.Click += new System.EventHandler(this.btnFSCustomers_Click);
            // 
            // btnFSTheLot
            // 
            this.btnFSTheLot.Location = new System.Drawing.Point(156, 67);
            this.btnFSTheLot.Name = "btnFSTheLot";
            this.btnFSTheLot.Size = new System.Drawing.Size(34, 139);
            this.btnFSTheLot.TabIndex = 14;
            this.btnFSTheLot.Text = "The Lot";
            this.btnFSTheLot.UseVisualStyleBackColor = true;
            this.btnFSTheLot.Click += new System.EventHandler(this.btnFSTheLot_Click);
            // 
            // btnFSAccount
            // 
            this.btnFSAccount.Location = new System.Drawing.Point(12, 125);
            this.btnFSAccount.Name = "btnFSAccount";
            this.btnFSAccount.Size = new System.Drawing.Size(138, 23);
            this.btnFSAccount.TabIndex = 15;
            this.btnFSAccount.Text = "Accounts from Staging";
            this.btnFSAccount.UseVisualStyleBackColor = true;
            this.btnFSAccount.Click += new System.EventHandler(this.btnFSAccount_Click);
            // 
            // btnFSPayment
            // 
            this.btnFSPayment.Location = new System.Drawing.Point(12, 154);
            this.btnFSPayment.Name = "btnFSPayment";
            this.btnFSPayment.Size = new System.Drawing.Size(138, 23);
            this.btnFSPayment.TabIndex = 16;
            this.btnFSPayment.Text = "Payments from Staging";
            this.btnFSPayment.UseVisualStyleBackColor = true;
            this.btnFSPayment.Click += new System.EventHandler(this.btnFSPayment_Click);
            // 
            // btnFSPayoff
            // 
            this.btnFSPayoff.Location = new System.Drawing.Point(12, 183);
            this.btnFSPayoff.Name = "btnFSPayoff";
            this.btnFSPayoff.Size = new System.Drawing.Size(138, 23);
            this.btnFSPayoff.TabIndex = 17;
            this.btnFSPayoff.Text = "Payoffs from Staging";
            this.btnFSPayoff.UseVisualStyleBackColor = true;
            this.btnFSPayoff.Click += new System.EventHandler(this.btnFSPayoff_Click);
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(12, 12);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(509, 20);
            this.txtFilename.TabIndex = 18;
            // 
            // btnFilename
            // 
            this.btnFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilename.Location = new System.Drawing.Point(527, 9);
            this.btnFilename.Name = "btnFilename";
            this.btnFilename.Size = new System.Drawing.Size(27, 23);
            this.btnFilename.TabIndex = 19;
            this.btnFilename.Text = "...";
            this.btnFilename.UseVisualStyleBackColor = true;
            this.btnFilename.Click += new System.EventHandler(this.btnFilename_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 244);
            this.Controls.Add(this.btnFilename);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.btnFSPayoff);
            this.Controls.Add(this.btnFSPayment);
            this.Controls.Add(this.btnFSAccount);
            this.Controls.Add(this.btnFSTheLot);
            this.Controls.Add(this.btnFSCustomers);
            this.Controls.Add(this.btnFSCollectors);
            this.Controls.Add(this.btnStaging);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnStaging;
        private System.Windows.Forms.Button btnFSCollectors;
        private System.Windows.Forms.Button btnFSCustomers;
        private System.Windows.Forms.Button btnFSTheLot;
        private System.Windows.Forms.Button btnFSAccount;
        private System.Windows.Forms.Button btnFSPayment;
        private System.Windows.Forms.Button btnFSPayoff;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnFilename;
    }
}

