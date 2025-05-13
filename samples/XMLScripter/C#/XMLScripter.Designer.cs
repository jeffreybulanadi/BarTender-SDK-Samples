namespace XMLScripter
{
   partial class XMLScripter
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMLScripter));
         this.txtXMLScript = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.btnRunXMLScript = new System.Windows.Forms.Button();
         this.webXMLResponse = new System.Windows.Forms.WebBrowser();
         this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
         this.panel1 = new System.Windows.Forms.Panel();
         this.picUpdating = new System.Windows.Forms.PictureBox();
         this.btnLoadXMLScript = new System.Windows.Forms.Button();
         this.btnSaveXMLScript = new System.Windows.Forms.Button();
         this.btnCopyResponseToClipboard = new System.Windows.Forms.Button();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.picUpdating)).BeginInit();
         this.SuspendLayout();
         // 
         // txtXMLScript
         // 
         this.txtXMLScript.AcceptsReturn = true;
         this.txtXMLScript.AcceptsTab = true;
         this.txtXMLScript.BackColor = System.Drawing.SystemColors.Window;
         this.txtXMLScript.Location = new System.Drawing.Point(7, 34);
         this.txtXMLScript.Multiline = true;
         this.txtXMLScript.Name = "txtXMLScript";
         this.txtXMLScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.txtXMLScript.Size = new System.Drawing.Size(551, 279);
         this.txtXMLScript.TabIndex = 1;
         this.txtXMLScript.TabStop = false;
         this.txtXMLScript.Text = resources.GetString("txtXMLScript.Text");
         this.txtXMLScript.WordWrap = false;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 18);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(62, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "&XML Script:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(7, 333);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(83, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "X&ML Response:";
         // 
         // btnRunXMLScript
         // 
         this.btnRunXMLScript.Location = new System.Drawing.Point(444, 319);
         this.btnRunXMLScript.Name = "btnRunXMLScript";
         this.btnRunXMLScript.Size = new System.Drawing.Size(114, 23);
         this.btnRunXMLScript.TabIndex = 4;
         this.btnRunXMLScript.Text = "&Run XML Script";
         this.btnRunXMLScript.UseVisualStyleBackColor = true;
         this.btnRunXMLScript.Click += new System.EventHandler(this.btnRunXMLScript_Click);
         // 
         // webXMLResponse
         // 
         this.webXMLResponse.AllowWebBrowserDrop = false;
         this.webXMLResponse.IsWebBrowserContextMenuEnabled = false;
         this.webXMLResponse.Location = new System.Drawing.Point(-2, 0);
         this.webXMLResponse.MinimumSize = new System.Drawing.Size(20, 20);
         this.webXMLResponse.Name = "webXMLResponse";
         this.webXMLResponse.Size = new System.Drawing.Size(549, 276);
         this.webXMLResponse.TabIndex = 0;
         this.webXMLResponse.TabStop = false;
         this.webXMLResponse.WebBrowserShortcutsEnabled = false;
         // 
         // backgroundWorker
         // 
         this.backgroundWorker.WorkerReportsProgress = true;
         this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
         this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
         // 
         // panel1
         // 
         this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.panel1.Controls.Add(this.picUpdating);
         this.panel1.Controls.Add(this.webXMLResponse);
         this.panel1.Location = new System.Drawing.Point(7, 349);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(551, 279);
         this.panel1.TabIndex = 6;
         // 
         // picUpdating
         // 
         this.picUpdating.BackColor = System.Drawing.Color.White;
         this.picUpdating.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
         this.picUpdating.Image = global::XMLScripter.Properties.Resources.updating;
         this.picUpdating.Location = new System.Drawing.Point(260, 134);
         this.picUpdating.Name = "picUpdating";
         this.picUpdating.Size = new System.Drawing.Size(24, 24);
         this.picUpdating.TabIndex = 6;
         this.picUpdating.TabStop = false;
         this.picUpdating.Visible = false;
         // 
         // btnLoadXMLScript
         // 
         this.btnLoadXMLScript.Location = new System.Drawing.Point(401, 5);
         this.btnLoadXMLScript.Name = "btnLoadXMLScript";
         this.btnLoadXMLScript.Size = new System.Drawing.Size(75, 23);
         this.btnLoadXMLScript.TabIndex = 2;
         this.btnLoadXMLScript.Text = "&Load...";
         this.btnLoadXMLScript.UseVisualStyleBackColor = true;
         this.btnLoadXMLScript.Click += new System.EventHandler(this.btnLoadXMLScript_Click);
         // 
         // btnSaveXMLScript
         // 
         this.btnSaveXMLScript.Location = new System.Drawing.Point(483, 5);
         this.btnSaveXMLScript.Name = "btnSaveXMLScript";
         this.btnSaveXMLScript.Size = new System.Drawing.Size(75, 23);
         this.btnSaveXMLScript.TabIndex = 3;
         this.btnSaveXMLScript.Text = "&Save...";
         this.btnSaveXMLScript.UseVisualStyleBackColor = true;
         this.btnSaveXMLScript.Click += new System.EventHandler(this.btnSaveXMLScript_Click);
         // 
         // btnCopyResponseToClipboard
         // 
         this.btnCopyResponseToClipboard.Enabled = false;
         this.btnCopyResponseToClipboard.Location = new System.Drawing.Point(378, 634);
         this.btnCopyResponseToClipboard.Name = "btnCopyResponseToClipboard";
         this.btnCopyResponseToClipboard.Size = new System.Drawing.Size(180, 23);
         this.btnCopyResponseToClipboard.TabIndex = 7;
         this.btnCopyResponseToClipboard.Text = "&Copy XML Response To Clipboard";
         this.btnCopyResponseToClipboard.UseVisualStyleBackColor = true;
         this.btnCopyResponseToClipboard.Click += new System.EventHandler(this.btnCopyResponseToClipboard_Click);
         // 
         // XMLScripter
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(564, 662);
         this.Controls.Add(this.btnCopyResponseToClipboard);
         this.Controls.Add(this.btnSaveXMLScript);
         this.Controls.Add(this.btnLoadXMLScript);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.btnRunXMLScript);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.txtXMLScript);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "XMLScripter";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "XML Scripter";
         this.Load += new System.EventHandler(this.XMLScripter_Load);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.XMLScripter_FormClosed);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.picUpdating)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox txtXMLScript;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Button btnRunXMLScript;
      private System.Windows.Forms.WebBrowser webXMLResponse;
      private System.ComponentModel.BackgroundWorker backgroundWorker;
      private System.Windows.Forms.PictureBox picUpdating;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button btnLoadXMLScript;
      private System.Windows.Forms.Button btnSaveXMLScript;
      private System.Windows.Forms.Button btnCopyResponseToClipboard;
   }
}

