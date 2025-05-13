namespace PrintToFileManager
{
   partial class PrintToFileManager
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
         this.btnOpenBTW = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.txtLabelFilename = new System.Windows.Forms.TextBox();
         this.btnPrintToFile = new System.Windows.Forms.Button();
         this.btnPrintPrinterFile = new System.Windows.Forms.Button();
         this.txtPrinterFileFilename = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.btnOpenPrinterFile = new System.Windows.Forms.Button();
         this.btnGenerateLicense = new System.Windows.Forms.Button();
         this.txtPrintLicense = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.openBTWDialog = new System.Windows.Forms.OpenFileDialog();
         this.openPrinterFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.label6 = new System.Windows.Forms.Label();
         this.cboStep1Printer = new System.Windows.Forms.ComboBox();
         this.savePrinterFileDialog = new System.Windows.Forms.SaveFileDialog();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.cboStep2Printer = new System.Windows.Forms.ComboBox();
         this.label3 = new System.Windows.Forms.Label();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.cboStep3Printer = new System.Windows.Forms.ComboBox();
         this.label4 = new System.Windows.Forms.Label();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnOpenBTW
         // 
         this.btnOpenBTW.Location = new System.Drawing.Point(406, 23);
         this.btnOpenBTW.Name = "btnOpenBTW";
         this.btnOpenBTW.Size = new System.Drawing.Size(84, 23);
         this.btnOpenBTW.TabIndex = 2;
         this.btnOpenBTW.Text = "&Open...";
         this.btnOpenBTW.UseVisualStyleBackColor = true;
         this.btnOpenBTW.Click += new System.EventHandler(this.btnOpenBTW_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 28);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(71, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Label For&mat:";
         // 
         // txtLabelFilename
         // 
         this.txtLabelFilename.Location = new System.Drawing.Point(87, 25);
         this.txtLabelFilename.Name = "txtLabelFilename";
         this.txtLabelFilename.ReadOnly = true;
         this.txtLabelFilename.Size = new System.Drawing.Size(312, 20);
         this.txtLabelFilename.TabIndex = 1;
         // 
         // btnPrintToFile
         // 
         this.btnPrintToFile.Enabled = false;
         this.btnPrintToFile.Location = new System.Drawing.Point(406, 54);
         this.btnPrintToFile.Name = "btnPrintToFile";
         this.btnPrintToFile.Size = new System.Drawing.Size(84, 23);
         this.btnPrintToFile.TabIndex = 5;
         this.btnPrintToFile.Text = "Print &To File...";
         this.btnPrintToFile.UseVisualStyleBackColor = true;
         this.btnPrintToFile.Click += new System.EventHandler(this.btnPrintToFile_Click);
         // 
         // btnPrintPrinterFile
         // 
         this.btnPrintPrinterFile.Enabled = false;
         this.btnPrintPrinterFile.Location = new System.Drawing.Point(406, 54);
         this.btnPrintPrinterFile.Name = "btnPrintPrinterFile";
         this.btnPrintPrinterFile.Size = new System.Drawing.Size(84, 23);
         this.btnPrintPrinterFile.TabIndex = 5;
         this.btnPrintPrinterFile.Text = "Pri&nt";
         this.btnPrintPrinterFile.UseVisualStyleBackColor = true;
         this.btnPrintPrinterFile.Click += new System.EventHandler(this.btnPrintPrinterFile_Click);
         // 
         // txtPrinterFileFilename
         // 
         this.txtPrinterFileFilename.Location = new System.Drawing.Point(87, 25);
         this.txtPrinterFileFilename.Name = "txtPrinterFileFilename";
         this.txtPrinterFileFilename.ReadOnly = true;
         this.txtPrinterFileFilename.Size = new System.Drawing.Size(311, 20);
         this.txtPrinterFileFilename.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(4, 28);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(59, 13);
         this.label2.TabIndex = 0;
         this.label2.Text = "Printer &File:";
         // 
         // btnOpenPrinterFile
         // 
         this.btnOpenPrinterFile.Location = new System.Drawing.Point(406, 23);
         this.btnOpenPrinterFile.Name = "btnOpenPrinterFile";
         this.btnOpenPrinterFile.Size = new System.Drawing.Size(84, 23);
         this.btnOpenPrinterFile.TabIndex = 2;
         this.btnOpenPrinterFile.Text = "Op&en...";
         this.btnOpenPrinterFile.UseVisualStyleBackColor = true;
         this.btnOpenPrinterFile.Click += new System.EventHandler(this.btnOpenPrinterFile_Click);
         // 
         // btnGenerateLicense
         // 
         this.btnGenerateLicense.Location = new System.Drawing.Point(406, 18);
         this.btnGenerateLicense.Name = "btnGenerateLicense";
         this.btnGenerateLicense.Size = new System.Drawing.Size(84, 23);
         this.btnGenerateLicense.TabIndex = 2;
         this.btnGenerateLicense.Text = "&Generate";
         this.btnGenerateLicense.UseVisualStyleBackColor = true;
         this.btnGenerateLicense.Click += new System.EventHandler(this.btnGenerateLicense_Click);
         // 
         // txtPrintLicense
         // 
         this.txtPrintLicense.Location = new System.Drawing.Point(87, 50);
         this.txtPrintLicense.Multiline = true;
         this.txtPrintLicense.Name = "txtPrintLicense";
         this.txtPrintLicense.ReadOnly = true;
         this.txtPrintLicense.Size = new System.Drawing.Size(312, 127);
         this.txtPrintLicense.TabIndex = 4;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(4, 53);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(71, 13);
         this.label5.TabIndex = 3;
         this.label5.Text = "Print &License:";
         // 
         // openBTWDialog
         // 
         this.openBTWDialog.DefaultExt = "btw";
         this.openBTWDialog.Filter = "BarTender Label Formats (*.btw)|*.btw";
         this.openBTWDialog.Title = "Open BarTender Label Format";
         // 
         // openPrinterFileDialog
         // 
         this.openPrinterFileDialog.DefaultExt = "btw";
         this.openPrinterFileDialog.Filter = "Printer Files (*.prn)|*.prn|All files (*.*)|*.*";
         this.openPrinterFileDialog.Title = "Open Printer File";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(4, 22);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(40, 13);
         this.label6.TabIndex = 0;
         this.label6.Text = "&Printer:";
         // 
         // cboStep1Printer
         // 
         this.cboStep1Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboStep1Printer.FormattingEnabled = true;
         this.cboStep1Printer.Location = new System.Drawing.Point(87, 19);
         this.cboStep1Printer.Name = "cboStep1Printer";
         this.cboStep1Printer.Size = new System.Drawing.Size(311, 21);
         this.cboStep1Printer.Sorted = true;
         this.cboStep1Printer.TabIndex = 1;
         this.cboStep1Printer.SelectedIndexChanged += new System.EventHandler(this.cboStep1Printer_SelectedIndexChanged);
         // 
         // savePrinterFileDialog
         // 
         this.savePrinterFileDialog.DefaultExt = "prn";
         this.savePrinterFileDialog.FileName = "printerFile";
         this.savePrinterFileDialog.Filter = "Printer Files (*.prn)|*.prn|All files|*.*";
         this.savePrinterFileDialog.SupportMultiDottedExtensions = true;
         this.savePrinterFileDialog.Title = "Save Printer File";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.cboStep1Printer);
         this.groupBox1.Controls.Add(this.btnGenerateLicense);
         this.groupBox1.Controls.Add(this.label6);
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.txtPrintLicense);
         this.groupBox1.Location = new System.Drawing.Point(7, 7);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(499, 187);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Step 1: Obtain a printing license on the client computer.";
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.cboStep2Printer);
         this.groupBox2.Controls.Add(this.btnPrintToFile);
         this.groupBox2.Controls.Add(this.label3);
         this.groupBox2.Controls.Add(this.txtLabelFilename);
         this.groupBox2.Controls.Add(this.btnOpenBTW);
         this.groupBox2.Controls.Add(this.label1);
         this.groupBox2.Location = new System.Drawing.Point(7, 200);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(499, 85);
         this.groupBox2.TabIndex = 1;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Step 2: Print to file on the server computer using the print license and a compat" +
             "ible printer driver.";
         // 
         // cboStep2Printer
         // 
         this.cboStep2Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboStep2Printer.FormattingEnabled = true;
         this.cboStep2Printer.Location = new System.Drawing.Point(87, 55);
         this.cboStep2Printer.Name = "cboStep2Printer";
         this.cboStep2Printer.Size = new System.Drawing.Size(311, 21);
         this.cboStep2Printer.Sorted = true;
         this.cboStep2Printer.TabIndex = 4;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(4, 58);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(40, 13);
         this.label3.TabIndex = 3;
         this.label3.Text = "P&rinter:";
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.cboStep3Printer);
         this.groupBox3.Controls.Add(this.label4);
         this.groupBox3.Controls.Add(this.btnOpenPrinterFile);
         this.groupBox3.Controls.Add(this.btnPrintPrinterFile);
         this.groupBox3.Controls.Add(this.label2);
         this.groupBox3.Controls.Add(this.txtPrinterFileFilename);
         this.groupBox3.Location = new System.Drawing.Point(7, 292);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(499, 85);
         this.groupBox3.TabIndex = 2;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Step 3: Print the printer file on the client computer.";
         // 
         // cboStep3Printer
         // 
         this.cboStep3Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboStep3Printer.FormattingEnabled = true;
         this.cboStep3Printer.Location = new System.Drawing.Point(87, 55);
         this.cboStep3Printer.Name = "cboStep3Printer";
         this.cboStep3Printer.Size = new System.Drawing.Size(311, 21);
         this.cboStep3Printer.Sorted = true;
         this.cboStep3Printer.TabIndex = 4;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(4, 58);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(40, 13);
         this.label4.TabIndex = 3;
         this.label4.Text = "Pr&inter:";
         // 
         // PrintToFileManager
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(514, 385);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "PrintToFileManager";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Print To File Manager";
         this.Load += new System.EventHandler(this.PrintToFileManager_Load);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrintToFileManager_FormClosed);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.groupBox3.ResumeLayout(false);
         this.groupBox3.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnOpenBTW;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtLabelFilename;
      private System.Windows.Forms.TextBox txtPrinterFileFilename;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button btnOpenPrinterFile;
      private System.Windows.Forms.OpenFileDialog openBTWDialog;
      private System.Windows.Forms.OpenFileDialog openPrinterFileDialog;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txtPrintLicense;
      private System.Windows.Forms.Button btnPrintToFile;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.ComboBox cboStep1Printer;
      private System.Windows.Forms.SaveFileDialog savePrinterFileDialog;
      private System.Windows.Forms.Button btnGenerateLicense;
      private System.Windows.Forms.Button btnPrintPrinterFile;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.ComboBox cboStep2Printer;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.ComboBox cboStep3Printer;
      private System.Windows.Forms.Label label4;
   }
}

