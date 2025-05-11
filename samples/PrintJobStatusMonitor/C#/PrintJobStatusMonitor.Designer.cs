namespace PrintJobStatusMonitor
{
   partial class PrintJobStatusMonitor
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
         this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.txtFilename = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.btnOpen = new System.Windows.Forms.Button();
         this.lstJobStatus = new System.Windows.Forms.ListBox();
         this.btnPrint = new System.Windows.Forms.Button();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.cboPrinters = new System.Windows.Forms.ComboBox();
         this.statusUpdater = new System.ComponentModel.BackgroundWorker();
         this.panel1 = new System.Windows.Forms.Panel();
         this.picThumbnail = new System.Windows.Forms.PictureBox();
         this.label4 = new System.Windows.Forms.Label();
         this.lstMessages = new System.Windows.Forms.ListBox();
         this.label8 = new System.Windows.Forms.Label();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.picThumbnail)).BeginInit();
         this.SuspendLayout();
         // 
         // openFileDialog
         // 
         this.openFileDialog.DefaultExt = "btw";
         this.openFileDialog.Filter = "BarTender Label Formats (*.btw)|*.btw";
         this.openFileDialog.Title = "Open BarTender Label Format";
         // 
         // txtFilename
         // 
         this.txtFilename.Location = new System.Drawing.Point(80, 9);
         this.txtFilename.Name = "txtFilename";
         this.txtFilename.ReadOnly = true;
         this.txtFilename.Size = new System.Drawing.Size(282, 20);
         this.txtFilename.TabIndex = 1;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 12);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(71, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "&Label Format:";
         // 
         // btnOpen
         // 
         this.btnOpen.Location = new System.Drawing.Point(368, 7);
         this.btnOpen.Name = "btnOpen";
         this.btnOpen.Size = new System.Drawing.Size(75, 23);
         this.btnOpen.TabIndex = 2;
         this.btnOpen.Text = "&Open...";
         this.btnOpen.UseVisualStyleBackColor = true;
         this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
         // 
         // lstJobStatus
         // 
         this.lstJobStatus.FormattingEnabled = true;
         this.lstJobStatus.HorizontalScrollbar = true;
         this.lstJobStatus.Location = new System.Drawing.Point(328, 89);
         this.lstJobStatus.Name = "lstJobStatus";
         this.lstJobStatus.Size = new System.Drawing.Size(544, 147);
         this.lstJobStatus.TabIndex = 9;
         this.lstJobStatus.UseTabStops = false;
         // 
         // btnPrint
         // 
         this.btnPrint.Enabled = false;
         this.btnPrint.Location = new System.Drawing.Point(368, 37);
         this.btnPrint.Name = "btnPrint";
         this.btnPrint.Size = new System.Drawing.Size(75, 23);
         this.btnPrint.TabIndex = 5;
         this.btnPrint.Text = "P&rint";
         this.btnPrint.UseVisualStyleBackColor = true;
         this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(328, 70);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(60, 13);
         this.label2.TabIndex = 8;
         this.label2.Text = "&Job Status:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(4, 42);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(40, 13);
         this.label3.TabIndex = 3;
         this.label3.Text = "&Printer:";
         // 
         // cboPrinters
         // 
         this.cboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboPrinters.FormattingEnabled = true;
         this.cboPrinters.Location = new System.Drawing.Point(80, 38);
         this.cboPrinters.Name = "cboPrinters";
         this.cboPrinters.Size = new System.Drawing.Size(282, 21);
         this.cboPrinters.Sorted = true;
         this.cboPrinters.TabIndex = 4;
         // 
         // statusUpdater
         // 
         this.statusUpdater.WorkerReportsProgress = true;
         this.statusUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.statusUpdater_DoWork);
         this.statusUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.statusUpdater_ProgressChanged);
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.Gray;
         this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.panel1.Controls.Add(this.picThumbnail);
         this.panel1.Location = new System.Drawing.Point(7, 89);
         this.panel1.Margin = new System.Windows.Forms.Padding(0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(314, 314);
         this.panel1.TabIndex = 7;
         // 
         // picThumbnail
         // 
         this.picThumbnail.Location = new System.Drawing.Point(0, 0);
         this.picThumbnail.Margin = new System.Windows.Forms.Padding(0);
         this.picThumbnail.Name = "picThumbnail";
         this.picThumbnail.Size = new System.Drawing.Size(309, 309);
         this.picThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.picThumbnail.TabIndex = 0;
         this.picThumbnail.TabStop = false;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(4, 70);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(59, 13);
         this.label4.TabIndex = 6;
         this.label4.Text = "Thumbnail:";
         // 
         // lstMessages
         // 
         this.lstMessages.FormattingEnabled = true;
         this.lstMessages.HorizontalScrollbar = true;
         this.lstMessages.Location = new System.Drawing.Point(328, 256);
         this.lstMessages.Name = "lstMessages";
         this.lstMessages.Size = new System.Drawing.Size(544, 147);
         this.lstMessages.TabIndex = 11;
         this.lstMessages.UseTabStops = false;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(328, 239);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(111, 13);
         this.label8.TabIndex = 10;
         this.label8.Text = "&BarTender Messages:";
         // 
         // PrintJobStatusMonitor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(878, 410);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.lstMessages);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.cboPrinters);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.btnPrint);
         this.Controls.Add(this.lstJobStatus);
         this.Controls.Add(this.btnOpen);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.txtFilename);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "PrintJobStatusMonitor";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Print Job Status Monitor";
         this.Load += new System.EventHandler(this.PrintJobStatusMonitor_Load);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrintJobStatusMonitor_FormClosed);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.picThumbnail)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.OpenFileDialog openFileDialog;
      private System.Windows.Forms.TextBox txtFilename;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button btnOpen;
      private System.Windows.Forms.ListBox lstJobStatus;
      private System.Windows.Forms.Button btnPrint;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.PictureBox picThumbnail;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.ComboBox cboPrinters;
      private System.ComponentModel.BackgroundWorker statusUpdater;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.ListBox lstMessages;
      private System.Windows.Forms.Label label8;
   }
}

