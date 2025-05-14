namespace TaskMaster
{
   partial class TaskMaster
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
         this.components = new System.ComponentModel.Container();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.lblEnginesRunning = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.btnStop = new System.Windows.Forms.Button();
         this.btnStart = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.txtNumEngines = new System.Windows.Forms.TextBox();
         this.lstAvailableTasks = new System.Windows.Forms.ListBox();
         this.label2 = new System.Windows.Forms.Label();
         this.lstRunTasks = new System.Windows.Forms.ListBox();
         this.label3 = new System.Windows.Forms.Label();
         this.btnAdd = new System.Windows.Forms.Button();
         this.btnRemove = new System.Windows.Forms.Button();
         this.lstOutput = new System.Windows.Forms.ListBox();
         this.label4 = new System.Windows.Forms.Label();
         this.btnRun = new System.Windows.Forms.Button();
         this.label6 = new System.Windows.Forms.Label();
         this.lblTasksInQueue = new System.Windows.Forms.Label();
         this.timerTaskUpdater = new System.Windows.Forms.Timer(this.components);
         this.label7 = new System.Windows.Forms.Label();
         this.lblEnginesBusy = new System.Windows.Forms.Label();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.lblTaskInfo = new System.Windows.Forms.Label();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.lblEnginesRunning);
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.btnStop);
         this.groupBox1.Controls.Add(this.btnStart);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.txtNumEngines);
         this.groupBox1.Location = new System.Drawing.Point(7, 9);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(422, 48);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Print Engines";
         // 
         // lblEnginesRunning
         // 
         this.lblEnginesRunning.Location = new System.Drawing.Point(355, 23);
         this.lblEnginesRunning.Name = "lblEnginesRunning";
         this.lblEnginesRunning.Size = new System.Drawing.Size(23, 13);
         this.lblEnginesRunning.TabIndex = 5;
         this.lblEnginesRunning.Text = "0";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(299, 22);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(50, 13);
         this.label5.TabIndex = 4;
         this.label5.Text = "Running:";
         // 
         // btnStop
         // 
         this.btnStop.Enabled = false;
         this.btnStop.Location = new System.Drawing.Point(218, 17);
         this.btnStop.Name = "btnStop";
         this.btnStop.Size = new System.Drawing.Size(75, 23);
         this.btnStop.TabIndex = 3;
         this.btnStop.Text = "St&op";
         this.btnStop.UseVisualStyleBackColor = true;
         this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
         // 
         // btnStart
         // 
         this.btnStart.Location = new System.Drawing.Point(137, 17);
         this.btnStart.Name = "btnStart";
         this.btnStart.Size = new System.Drawing.Size(75, 23);
         this.btnStart.TabIndex = 2;
         this.btnStart.Text = "&Start";
         this.btnStart.UseVisualStyleBackColor = true;
         this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 23);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(74, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "&Engine Count:";
         // 
         // txtNumEngines
         // 
         this.txtNumEngines.Location = new System.Drawing.Point(84, 19);
         this.txtNumEngines.Name = "txtNumEngines";
         this.txtNumEngines.Size = new System.Drawing.Size(47, 20);
         this.txtNumEngines.TabIndex = 1;
         this.txtNumEngines.Text = "3";
         // 
         // lstAvailableTasks
         // 
         this.lstAvailableTasks.FormattingEnabled = true;
         this.lstAvailableTasks.Location = new System.Drawing.Point(7, 82);
         this.lstAvailableTasks.Name = "lstAvailableTasks";
         this.lstAvailableTasks.Size = new System.Drawing.Size(168, 225);
         this.lstAvailableTasks.TabIndex = 2;
         this.lstAvailableTasks.SelectedIndexChanged += new System.EventHandler(this.lstAvailableTasks_SelectedIndexChanged);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(4, 66);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(85, 13);
         this.label2.TabIndex = 1;
         this.label2.Text = "&Available Tasks:";
         // 
         // lstRunTasks
         // 
         this.lstRunTasks.FormattingEnabled = true;
         this.lstRunTasks.Location = new System.Drawing.Point(261, 82);
         this.lstRunTasks.Name = "lstRunTasks";
         this.lstRunTasks.Size = new System.Drawing.Size(168, 225);
         this.lstRunTasks.TabIndex = 6;
         this.lstRunTasks.SelectedIndexChanged += new System.EventHandler(this.lstRunTasks_SelectedIndexChanged);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(435, 9);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(42, 13);
         this.label3.TabIndex = 13;
         this.label3.Text = "Output:";
         // 
         // btnAdd
         // 
         this.btnAdd.Enabled = false;
         this.btnAdd.Location = new System.Drawing.Point(181, 144);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(75, 23);
         this.btnAdd.TabIndex = 3;
         this.btnAdd.Text = "&Add";
         this.btnAdd.UseVisualStyleBackColor = true;
         this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
         // 
         // btnRemove
         // 
         this.btnRemove.Enabled = false;
         this.btnRemove.Location = new System.Drawing.Point(181, 173);
         this.btnRemove.Name = "btnRemove";
         this.btnRemove.Size = new System.Drawing.Size(75, 23);
         this.btnRemove.TabIndex = 4;
         this.btnRemove.Text = "&Remove";
         this.btnRemove.UseVisualStyleBackColor = true;
         this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
         // 
         // lstOutput
         // 
         this.lstOutput.FormattingEnabled = true;
         this.lstOutput.HorizontalScrollbar = true;
         this.lstOutput.Location = new System.Drawing.Point(438, 28);
         this.lstOutput.Name = "lstOutput";
         this.lstOutput.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.lstOutput.Size = new System.Drawing.Size(444, 342);
         this.lstOutput.TabIndex = 14;
         this.lstOutput.TabStop = false;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(258, 66);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(78, 13);
         this.label4.TabIndex = 5;
         this.label4.Text = "&Tasks To Run:";
         // 
         // btnRun
         // 
         this.btnRun.Enabled = false;
         this.btnRun.Location = new System.Drawing.Point(354, 310);
         this.btnRun.Name = "btnRun";
         this.btnRun.Size = new System.Drawing.Size(75, 23);
         this.btnRun.TabIndex = 12;
         this.btnRun.Text = "R&un";
         this.btnRun.UseVisualStyleBackColor = true;
         this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(325, 356);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(85, 13);
         this.label6.TabIndex = 10;
         this.label6.Text = "Tasks in Queue:";
         // 
         // lblTasksInQueue
         // 
         this.lblTasksInQueue.Location = new System.Drawing.Point(408, 356);
         this.lblTasksInQueue.Name = "lblTasksInQueue";
         this.lblTasksInQueue.Size = new System.Drawing.Size(23, 13);
         this.lblTasksInQueue.TabIndex = 11;
         this.lblTasksInQueue.Text = "0";
         this.lblTasksInQueue.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // timerTaskUpdater
         // 
         this.timerTaskUpdater.Enabled = true;
         this.timerTaskUpdater.Tick += new System.EventHandler(this.timerTaskUpdater_Tick);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(325, 340);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(74, 13);
         this.label7.TabIndex = 8;
         this.label7.Text = "Busy Engines:";
         // 
         // lblEnginesBusy
         // 
         this.lblEnginesBusy.Location = new System.Drawing.Point(408, 340);
         this.lblEnginesBusy.Name = "lblEnginesBusy";
         this.lblEnginesBusy.Size = new System.Drawing.Size(23, 13);
         this.lblEnginesBusy.TabIndex = 9;
         this.lblEnginesBusy.Text = "0";
         this.lblEnginesBusy.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.lblTaskInfo);
         this.groupBox2.Location = new System.Drawing.Point(7, 310);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(312, 60);
         this.groupBox2.TabIndex = 7;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Task Description";
         // 
         // lblTaskInfo
         // 
         this.lblTaskInfo.Location = new System.Drawing.Point(4, 17);
         this.lblTaskInfo.MaximumSize = new System.Drawing.Size(295, 37);
         this.lblTaskInfo.MinimumSize = new System.Drawing.Size(295, 37);
         this.lblTaskInfo.Name = "lblTaskInfo";
         this.lblTaskInfo.Size = new System.Drawing.Size(295, 37);
         this.lblTaskInfo.TabIndex = 0;
         this.lblTaskInfo.Tag = "";
         this.lblTaskInfo.Text = "No Task Selected.";
         // 
         // TaskMaster
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(890, 378);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.lblEnginesBusy);
         this.Controls.Add(this.lblTasksInQueue);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.btnRun);
         this.Controls.Add(this.lstOutput);
         this.Controls.Add(this.btnRemove);
         this.Controls.Add(this.btnAdd);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.lstRunTasks);
         this.Controls.Add(this.lstAvailableTasks);
         this.Controls.Add(this.groupBox1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.Name = "TaskMaster";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Task Master";
         this.Load += new System.EventHandler(this.TaskExecutor_Load);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TaskExecutor_FormClosed);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button btnStop;
      private System.Windows.Forms.Button btnStart;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtNumEngines;
      private System.Windows.Forms.ListBox lstAvailableTasks;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ListBox lstRunTasks;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Button btnAdd;
      private System.Windows.Forms.Button btnRemove;
      private System.Windows.Forms.ListBox lstOutput;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label lblEnginesRunning;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Button btnRun;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label lblTasksInQueue;
      private System.Windows.Forms.Timer timerTaskUpdater;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label lblEnginesBusy;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Label lblTaskInfo;
   }
}

