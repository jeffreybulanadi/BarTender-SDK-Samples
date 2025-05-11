namespace LabelPrint
{
   partial class LabelPrint
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
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
         this.txtFolderPath = new System.Windows.Forms.TextBox();
         this.btnOpen = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.substringGrid = new System.Windows.Forms.DataGridView();
         this.label2 = new System.Windows.Forms.Label();
         this.cboPrinters = new System.Windows.Forms.ComboBox();
         this.btnPrint = new System.Windows.Forms.Button();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.txtIdenticalCopies = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.txtSerializedCopies = new System.Windows.Forms.TextBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.lstLabelBrowser = new System.Windows.Forms.ListView();
         this.thumbnailCacheWorker = new System.ComponentModel.BackgroundWorker();
         this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
         this.picUpdatingFormat = new System.Windows.Forms.PictureBox();
         this.lblNoSubstrings = new System.Windows.Forms.Label();
         this.lblFormatError = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.substringGrid)).BeginInit();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.picUpdatingFormat)).BeginInit();
         this.SuspendLayout();
         // 
         // txtFolderPath
         // 
         this.txtFolderPath.Location = new System.Drawing.Point(86, 9);
         this.txtFolderPath.Name = "txtFolderPath";
         this.txtFolderPath.ReadOnly = true;
         this.txtFolderPath.Size = new System.Drawing.Size(315, 20);
         this.txtFolderPath.TabIndex = 8;
         // 
         // btnOpen
         // 
         this.btnOpen.Location = new System.Drawing.Point(407, 7);
         this.btnOpen.Name = "btnOpen";
         this.btnOpen.Size = new System.Drawing.Size(75, 23);
         this.btnOpen.TabIndex = 0;
         this.btnOpen.Text = "&Open...";
         this.btnOpen.UseVisualStyleBackColor = true;
         this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 12);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(68, 13);
         this.label1.TabIndex = 7;
         this.label1.Text = "&Label Folder:";
         // 
         // substringGrid
         // 
         this.substringGrid.AllowUserToAddRows = false;
         this.substringGrid.AllowUserToDeleteRows = false;
         this.substringGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
         this.substringGrid.BackgroundColor = System.Drawing.Color.White;
         this.substringGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
         dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
         dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
         this.substringGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
         this.substringGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
         dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
         dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.substringGrid.DefaultCellStyle = dataGridViewCellStyle2;
         this.substringGrid.Location = new System.Drawing.Point(491, 37);
         this.substringGrid.MultiSelect = false;
         this.substringGrid.Name = "substringGrid";
         dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
         dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
         dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
         this.substringGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
         this.substringGrid.Size = new System.Drawing.Size(389, 325);
         this.substringGrid.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(491, 14);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(96, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "&Named Substrings:";
         // 
         // cboPrinters
         // 
         this.cboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboPrinters.FormattingEnabled = true;
         this.cboPrinters.Location = new System.Drawing.Point(98, 18);
         this.cboPrinters.Name = "cboPrinters";
         this.cboPrinters.Size = new System.Drawing.Size(282, 21);
         this.cboPrinters.Sorted = true;
         this.cboPrinters.TabIndex = 1;
         // 
         // btnPrint
         // 
         this.btnPrint.Enabled = false;
         this.btnPrint.Location = new System.Drawing.Point(306, 68);
         this.btnPrint.Name = "btnPrint";
         this.btnPrint.Size = new System.Drawing.Size(75, 23);
         this.btnPrint.TabIndex = 6;
         this.btnPrint.Text = "&Print";
         this.btnPrint.UseVisualStyleBackColor = true;
         this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(6, 21);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(40, 13);
         this.label3.TabIndex = 0;
         this.label3.Text = "P&rinter:";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(6, 48);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(85, 13);
         this.label4.TabIndex = 2;
         this.label4.Text = "&Identical Copies:";
         // 
         // txtIdenticalCopies
         // 
         this.txtIdenticalCopies.Location = new System.Drawing.Point(98, 45);
         this.txtIdenticalCopies.Name = "txtIdenticalCopies";
         this.txtIdenticalCopies.ReadOnly = true;
         this.txtIdenticalCopies.Size = new System.Drawing.Size(112, 20);
         this.txtIdenticalCopies.TabIndex = 3;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(6, 74);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(90, 13);
         this.label5.TabIndex = 4;
         this.label5.Text = "&Serialized Copies:";
         // 
         // txtSerializedCopies
         // 
         this.txtSerializedCopies.Location = new System.Drawing.Point(98, 71);
         this.txtSerializedCopies.Name = "txtSerializedCopies";
         this.txtSerializedCopies.ReadOnly = true;
         this.txtSerializedCopies.Size = new System.Drawing.Size(112, 20);
         this.txtSerializedCopies.TabIndex = 5;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.txtIdenticalCopies);
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.txtSerializedCopies);
         this.groupBox1.Controls.Add(this.label4);
         this.groupBox1.Controls.Add(this.label3);
         this.groupBox1.Controls.Add(this.btnPrint);
         this.groupBox1.Controls.Add(this.cboPrinters);
         this.groupBox1.Location = new System.Drawing.Point(491, 368);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(389, 99);
         this.groupBox1.TabIndex = 6;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Printing";
         // 
         // lstLabelBrowser
         // 
         this.lstLabelBrowser.BackColor = System.Drawing.Color.White;
         this.lstLabelBrowser.HideSelection = false;
         this.lstLabelBrowser.Location = new System.Drawing.Point(7, 37);
         this.lstLabelBrowser.MultiSelect = false;
         this.lstLabelBrowser.Name = "lstLabelBrowser";
         this.lstLabelBrowser.Size = new System.Drawing.Size(475, 430);
         this.lstLabelBrowser.TabIndex = 1;
         this.lstLabelBrowser.UseCompatibleStateImageBehavior = false;
         this.lstLabelBrowser.VirtualMode = true;
         this.lstLabelBrowser.SelectedIndexChanged += new System.EventHandler(this.lstLabelBrowser_SelectedIndexChanged);
         this.lstLabelBrowser.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lstLabelBrowser_RetrieveVirtualItem);
         this.lstLabelBrowser.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.lstLabelBrowser_CacheVirtualItems);
         // 
         // thumbnailCacheWorker
         // 
         this.thumbnailCacheWorker.WorkerReportsProgress = true;
         this.thumbnailCacheWorker.WorkerSupportsCancellation = true;
         this.thumbnailCacheWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.thumbnailCacheWorker_DoWork);
         this.thumbnailCacheWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.thumbnailCacheWorker_ProgressChanged);
         // 
         // folderBrowserDialog
         // 
         this.folderBrowserDialog.ShowNewFolderButton = false;
         // 
         // picUpdatingFormat
         // 
         this.picUpdatingFormat.BackColor = System.Drawing.Color.White;
         this.picUpdatingFormat.Image = global::LabelPrint.Properties.Resources.updating;
         this.picUpdatingFormat.Location = new System.Drawing.Point(675, 198);
         this.picUpdatingFormat.Name = "picUpdatingFormat";
         this.picUpdatingFormat.Size = new System.Drawing.Size(28, 25);
         this.picUpdatingFormat.TabIndex = 34;
         this.picUpdatingFormat.TabStop = false;
         this.picUpdatingFormat.Visible = false;
         // 
         // lblNoSubstrings
         // 
         this.lblNoSubstrings.AutoSize = true;
         this.lblNoSubstrings.BackColor = System.Drawing.Color.White;
         this.lblNoSubstrings.Location = new System.Drawing.Point(565, 175);
         this.lblNoSubstrings.Name = "lblNoSubstrings";
         this.lblNoSubstrings.Size = new System.Drawing.Size(242, 13);
         this.lblNoSubstrings.TabIndex = 4;
         this.lblNoSubstrings.Text = "This label does not contain any named substrings.";
         this.lblNoSubstrings.Visible = false;
         // 
         // lblFormatError
         // 
         this.lblFormatError.AutoSize = true;
         this.lblFormatError.BackColor = System.Drawing.Color.White;
         this.lblFormatError.Location = new System.Drawing.Point(588, 190);
         this.lblFormatError.Name = "lblFormatError";
         this.lblFormatError.Size = new System.Drawing.Size(191, 13);
         this.lblFormatError.TabIndex = 5;
         this.lblFormatError.Text = "There was an error opening this format.";
         this.lblFormatError.Visible = false;
         // 
         // LabelPrint
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(887, 476);
         this.Controls.Add(this.lblFormatError);
         this.Controls.Add(this.lblNoSubstrings);
         this.Controls.Add(this.picUpdatingFormat);
         this.Controls.Add(this.lstLabelBrowser);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.substringGrid);
         this.Controls.Add(this.txtFolderPath);
         this.Controls.Add(this.btnOpen);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "LabelPrint";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Label Print";
         this.Load += new System.EventHandler(this.LabelPrint_Load);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LabelPrint_FormClosed);
         ((System.ComponentModel.ISupportInitialize)(this.substringGrid)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.picUpdatingFormat)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox txtFolderPath;
      private System.Windows.Forms.Button btnOpen;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.DataGridView substringGrid;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox cboPrinters;
      private System.Windows.Forms.Button btnPrint;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox txtIdenticalCopies;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txtSerializedCopies;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.ListView lstLabelBrowser;
      private System.ComponentModel.BackgroundWorker thumbnailCacheWorker;
      private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
      private System.Windows.Forms.PictureBox picUpdatingFormat;
      private System.Windows.Forms.Label lblNoSubstrings;
      private System.Windows.Forms.Label lblFormatError;
   }
}

