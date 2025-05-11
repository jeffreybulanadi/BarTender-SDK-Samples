namespace PrintPreview
{
   partial class PrintPreview
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
         this.cboPrinters = new System.Windows.Forms.ComboBox();
         this.label4 = new System.Windows.Forms.Label();
         this.btnPreview = new System.Windows.Forms.Button();
         this.btnOpen = new System.Windows.Forms.Button();
         this.label3 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.txtFilename = new System.Windows.Forms.TextBox();
         this.panel1 = new System.Windows.Forms.Panel();
         this.picUpdating = new System.Windows.Forms.PictureBox();
         this.picPreview = new System.Windows.Forms.PictureBox();
         this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
         this.btnFirst = new System.Windows.Forms.Button();
         this.btnPrev = new System.Windows.Forms.Button();
         this.btnLast = new System.Windows.Forms.Button();
         this.btnNext = new System.Windows.Forms.Button();
         this.lblNumPreviews = new System.Windows.Forms.Label();
         this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.picUpdating)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
         this.SuspendLayout();
         // 
         // cboPrinters
         // 
         this.cboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboPrinters.FormattingEnabled = true;
         this.cboPrinters.Location = new System.Drawing.Point(83, 39);
         this.cboPrinters.Name = "cboPrinters";
         this.cboPrinters.Size = new System.Drawing.Size(344, 21);
         this.cboPrinters.Sorted = true;
         this.cboPrinters.TabIndex = 4;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(7, 71);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(72, 13);
         this.label4.TabIndex = 6;
         this.label4.Text = "Print Preview:";
         // 
         // btnPreview
         // 
         this.btnPreview.Enabled = false;
         this.btnPreview.Location = new System.Drawing.Point(434, 39);
         this.btnPreview.Name = "btnPreview";
         this.btnPreview.Size = new System.Drawing.Size(75, 23);
         this.btnPreview.TabIndex = 5;
         this.btnPreview.Text = "P&review";
         this.btnPreview.UseVisualStyleBackColor = true;
         this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
         // 
         // btnOpen
         // 
         this.btnOpen.Location = new System.Drawing.Point(434, 9);
         this.btnOpen.Name = "btnOpen";
         this.btnOpen.Size = new System.Drawing.Size(75, 23);
         this.btnOpen.TabIndex = 2;
         this.btnOpen.Text = "&Open...";
         this.btnOpen.UseVisualStyleBackColor = true;
         this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(7, 43);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(40, 13);
         this.label3.TabIndex = 3;
         this.label3.Text = "&Printer:";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 13);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(71, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "&Label Format:";
         // 
         // txtFilename
         // 
         this.txtFilename.Location = new System.Drawing.Point(83, 10);
         this.txtFilename.Name = "txtFilename";
         this.txtFilename.ReadOnly = true;
         this.txtFilename.Size = new System.Drawing.Size(345, 20);
         this.txtFilename.TabIndex = 1;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.Gray;
         this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.panel1.Controls.Add(this.picUpdating);
         this.panel1.Controls.Add(this.picPreview);
         this.panel1.Location = new System.Drawing.Point(9, 87);
         this.panel1.Margin = new System.Windows.Forms.Padding(0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(500, 500);
         this.panel1.TabIndex = 7;
         // 
         // picUpdating
         // 
         this.picUpdating.BackColor = System.Drawing.Color.White;
         this.picUpdating.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
         this.picUpdating.Image = global::PrintPreview.Properties.Resources.updating;
         this.picUpdating.Location = new System.Drawing.Point(236, 236);
         this.picUpdating.Name = "picUpdating";
         this.picUpdating.Size = new System.Drawing.Size(24, 24);
         this.picUpdating.TabIndex = 12;
         this.picUpdating.TabStop = false;
         this.picUpdating.Visible = false;
         // 
         // picPreview
         // 
         this.picPreview.BackColor = System.Drawing.Color.Gray;
         this.picPreview.Location = new System.Drawing.Point(0, 0);
         this.picPreview.Margin = new System.Windows.Forms.Padding(0);
         this.picPreview.Name = "picPreview";
         this.picPreview.Size = new System.Drawing.Size(495, 495);
         this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.picPreview.TabIndex = 11;
         this.picPreview.TabStop = false;
         // 
         // backgroundWorker
         // 
         this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
         this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
         // 
         // btnFirst
         // 
         this.btnFirst.Location = new System.Drawing.Point(8, 593);
         this.btnFirst.Name = "btnFirst";
         this.btnFirst.Size = new System.Drawing.Size(37, 23);
         this.btnFirst.TabIndex = 8;
         this.btnFirst.Text = "<<";
         this.btnFirst.UseVisualStyleBackColor = true;
         this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
         // 
         // btnPrev
         // 
         this.btnPrev.Location = new System.Drawing.Point(51, 593);
         this.btnPrev.Name = "btnPrev";
         this.btnPrev.Size = new System.Drawing.Size(37, 23);
         this.btnPrev.TabIndex = 9;
         this.btnPrev.Text = "<";
         this.btnPrev.UseVisualStyleBackColor = true;
         this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
         // 
         // btnLast
         // 
         this.btnLast.Location = new System.Drawing.Point(472, 593);
         this.btnLast.Name = "btnLast";
         this.btnLast.Size = new System.Drawing.Size(37, 23);
         this.btnLast.TabIndex = 12;
         this.btnLast.Text = ">>";
         this.btnLast.UseVisualStyleBackColor = true;
         this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
         // 
         // btnNext
         // 
         this.btnNext.Location = new System.Drawing.Point(429, 593);
         this.btnNext.Name = "btnNext";
         this.btnNext.Size = new System.Drawing.Size(37, 23);
         this.btnNext.TabIndex = 11;
         this.btnNext.Text = ">";
         this.btnNext.UseVisualStyleBackColor = true;
         this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
         // 
         // lblNumPreviews
         // 
         this.lblNumPreviews.AutoSize = true;
         this.lblNumPreviews.Location = new System.Drawing.Point(226, 598);
         this.lblNumPreviews.Name = "lblNumPreviews";
         this.lblNumPreviews.Size = new System.Drawing.Size(62, 13);
         this.lblNumPreviews.TabIndex = 10;
         this.lblNumPreviews.Text = "Page 0 of 0";
         // 
         // openFileDialog
         // 
         this.openFileDialog.DefaultExt = "btw";
         this.openFileDialog.Filter = "BarTender Label Formats (*.btw)|*.btw";
         this.openFileDialog.Title = "Open BarTender Label Format";
         // 
         // PrintPreview
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(516, 623);
         this.Controls.Add(this.lblNumPreviews);
         this.Controls.Add(this.btnNext);
         this.Controls.Add(this.btnLast);
         this.Controls.Add(this.btnPrev);
         this.Controls.Add(this.btnFirst);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.cboPrinters);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.btnPreview);
         this.Controls.Add(this.btnOpen);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.txtFilename);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "PrintPreview";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Print Preview";
         this.Load += new System.EventHandler(this.PrintPreview_Load);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrintPreview_FormClosed);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.picUpdating)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ComboBox cboPrinters;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.PictureBox picPreview;
      private System.Windows.Forms.Button btnPreview;
      private System.Windows.Forms.Button btnOpen;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtFilename;
      private System.Windows.Forms.Panel panel1;
      private System.ComponentModel.BackgroundWorker backgroundWorker;
      private System.Windows.Forms.Button btnFirst;
      private System.Windows.Forms.Button btnPrev;
      private System.Windows.Forms.Button btnLast;
      private System.Windows.Forms.Button btnNext;
      private System.Windows.Forms.Label lblNumPreviews;
      private System.Windows.Forms.PictureBox picUpdating;
      private System.Windows.Forms.OpenFileDialog openFileDialog;
   }
}

