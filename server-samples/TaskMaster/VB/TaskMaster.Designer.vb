Imports Microsoft.VisualBasic
Imports System
Namespace TaskMaster
    Partial Class TaskMaster
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
         Me.components = New System.ComponentModel.Container
         Me.groupBox1 = New System.Windows.Forms.GroupBox
         Me.lblEnginesRunning = New System.Windows.Forms.Label
         Me.label5 = New System.Windows.Forms.Label
         Me.btnStop = New System.Windows.Forms.Button
         Me.btnStart = New System.Windows.Forms.Button
         Me.label1 = New System.Windows.Forms.Label
         Me.txtNumEngines = New System.Windows.Forms.TextBox
         Me.lstAvailableTasks = New System.Windows.Forms.ListBox
         Me.label2 = New System.Windows.Forms.Label
         Me.lstRunTasks = New System.Windows.Forms.ListBox
         Me.label3 = New System.Windows.Forms.Label
         Me.btnAdd = New System.Windows.Forms.Button
         Me.btnRemove = New System.Windows.Forms.Button
         Me.lstOutput = New System.Windows.Forms.ListBox
         Me.label4 = New System.Windows.Forms.Label
         Me.btnRun = New System.Windows.Forms.Button
         Me.label6 = New System.Windows.Forms.Label
         Me.lblTasksInQueue = New System.Windows.Forms.Label
         Me.timerTaskUpdater = New System.Windows.Forms.Timer(Me.components)
         Me.label7 = New System.Windows.Forms.Label
         Me.lblEnginesBusy = New System.Windows.Forms.Label
         Me.groupBox2 = New System.Windows.Forms.GroupBox
         Me.lblTaskInfo = New System.Windows.Forms.Label
         Me.groupBox1.SuspendLayout()
         Me.groupBox2.SuspendLayout()
         Me.SuspendLayout()
         '
         'groupBox1
         '
         Me.groupBox1.Controls.Add(Me.lblEnginesRunning)
         Me.groupBox1.Controls.Add(Me.label5)
         Me.groupBox1.Controls.Add(Me.btnStop)
         Me.groupBox1.Controls.Add(Me.btnStart)
         Me.groupBox1.Controls.Add(Me.label1)
         Me.groupBox1.Controls.Add(Me.txtNumEngines)
         Me.groupBox1.Location = New System.Drawing.Point(7, 9)
         Me.groupBox1.Name = "groupBox1"
         Me.groupBox1.Size = New System.Drawing.Size(422, 48)
         Me.groupBox1.TabIndex = 0
         Me.groupBox1.TabStop = False
         Me.groupBox1.Text = "Print Engines"
         '
         'lblEnginesRunning
         '
         Me.lblEnginesRunning.Location = New System.Drawing.Point(355, 23)
         Me.lblEnginesRunning.Name = "lblEnginesRunning"
         Me.lblEnginesRunning.Size = New System.Drawing.Size(23, 13)
         Me.lblEnginesRunning.TabIndex = 5
         Me.lblEnginesRunning.Text = "0"
         '
         'label5
         '
         Me.label5.AutoSize = True
         Me.label5.Location = New System.Drawing.Point(299, 22)
         Me.label5.Name = "label5"
         Me.label5.Size = New System.Drawing.Size(50, 13)
         Me.label5.TabIndex = 4
         Me.label5.Text = "Running:"
         '
         'btnStop
         '
         Me.btnStop.Enabled = False
         Me.btnStop.Location = New System.Drawing.Point(218, 17)
         Me.btnStop.Name = "btnStop"
         Me.btnStop.Size = New System.Drawing.Size(75, 23)
         Me.btnStop.TabIndex = 3
         Me.btnStop.Text = "St&op"
         Me.btnStop.UseVisualStyleBackColor = True
         '
         'btnStart
         '
         Me.btnStart.Location = New System.Drawing.Point(137, 17)
         Me.btnStart.Name = "btnStart"
         Me.btnStart.Size = New System.Drawing.Size(75, 23)
         Me.btnStart.TabIndex = 2
         Me.btnStart.Text = "&Start"
         Me.btnStart.UseVisualStyleBackColor = True
         '
         'label1
         '
         Me.label1.AutoSize = True
         Me.label1.Location = New System.Drawing.Point(4, 23)
         Me.label1.Name = "label1"
         Me.label1.Size = New System.Drawing.Size(74, 13)
         Me.label1.TabIndex = 0
         Me.label1.Text = "&Engine Count:"
         '
         'txtNumEngines
         '
         Me.txtNumEngines.Location = New System.Drawing.Point(84, 19)
         Me.txtNumEngines.Name = "txtNumEngines"
         Me.txtNumEngines.Size = New System.Drawing.Size(47, 20)
         Me.txtNumEngines.TabIndex = 1
         Me.txtNumEngines.Text = "3"
         '
         'lstAvailableTasks
         '
         Me.lstAvailableTasks.FormattingEnabled = True
         Me.lstAvailableTasks.Location = New System.Drawing.Point(7, 82)
         Me.lstAvailableTasks.Name = "lstAvailableTasks"
         Me.lstAvailableTasks.Size = New System.Drawing.Size(168, 225)
         Me.lstAvailableTasks.TabIndex = 2
         '
         'label2
         '
         Me.label2.AutoSize = True
         Me.label2.Location = New System.Drawing.Point(4, 66)
         Me.label2.Name = "label2"
         Me.label2.Size = New System.Drawing.Size(85, 13)
         Me.label2.TabIndex = 1
         Me.label2.Text = "&Available Tasks:"
         '
         'lstRunTasks
         '
         Me.lstRunTasks.FormattingEnabled = True
         Me.lstRunTasks.Location = New System.Drawing.Point(261, 82)
         Me.lstRunTasks.Name = "lstRunTasks"
         Me.lstRunTasks.Size = New System.Drawing.Size(168, 225)
         Me.lstRunTasks.TabIndex = 6
         '
         'label3
         '
         Me.label3.AutoSize = True
         Me.label3.Location = New System.Drawing.Point(435, 9)
         Me.label3.Name = "label3"
         Me.label3.Size = New System.Drawing.Size(42, 13)
         Me.label3.TabIndex = 13
         Me.label3.Text = "Output:"
         '
         'btnAdd
         '
         Me.btnAdd.Enabled = False
         Me.btnAdd.Location = New System.Drawing.Point(181, 144)
         Me.btnAdd.Name = "btnAdd"
         Me.btnAdd.Size = New System.Drawing.Size(75, 23)
         Me.btnAdd.TabIndex = 3
         Me.btnAdd.Text = "&Add"
         Me.btnAdd.UseVisualStyleBackColor = True
         '
         'btnRemove
         '
         Me.btnRemove.Enabled = False
         Me.btnRemove.Location = New System.Drawing.Point(181, 173)
         Me.btnRemove.Name = "btnRemove"
         Me.btnRemove.Size = New System.Drawing.Size(75, 23)
         Me.btnRemove.TabIndex = 4
         Me.btnRemove.Text = "&Remove"
         Me.btnRemove.UseVisualStyleBackColor = True
         '
         'lstOutput
         '
         Me.lstOutput.FormattingEnabled = True
         Me.lstOutput.HorizontalScrollbar = True
         Me.lstOutput.Location = New System.Drawing.Point(438, 28)
         Me.lstOutput.Name = "lstOutput"
         Me.lstOutput.SelectionMode = System.Windows.Forms.SelectionMode.None
         Me.lstOutput.Size = New System.Drawing.Size(444, 342)
         Me.lstOutput.TabIndex = 14
         Me.lstOutput.TabStop = False
         '
         'label4
         '
         Me.label4.AutoSize = True
         Me.label4.Location = New System.Drawing.Point(258, 66)
         Me.label4.Name = "label4"
         Me.label4.Size = New System.Drawing.Size(78, 13)
         Me.label4.TabIndex = 5
         Me.label4.Text = "&Tasks To Run:"
         '
         'btnRun
         '
         Me.btnRun.Enabled = False
         Me.btnRun.Location = New System.Drawing.Point(354, 310)
         Me.btnRun.Name = "btnRun"
         Me.btnRun.Size = New System.Drawing.Size(75, 23)
         Me.btnRun.TabIndex = 12
         Me.btnRun.Text = "R&un"
         Me.btnRun.UseVisualStyleBackColor = True
         '
         'label6
         '
         Me.label6.AutoSize = True
         Me.label6.Location = New System.Drawing.Point(325, 356)
         Me.label6.Name = "label6"
         Me.label6.Size = New System.Drawing.Size(85, 13)
         Me.label6.TabIndex = 10
         Me.label6.Text = "Tasks in Queue:"
         '
         'lblTasksInQueue
         '
         Me.lblTasksInQueue.Location = New System.Drawing.Point(408, 356)
         Me.lblTasksInQueue.Name = "lblTasksInQueue"
         Me.lblTasksInQueue.Size = New System.Drawing.Size(23, 13)
         Me.lblTasksInQueue.TabIndex = 11
         Me.lblTasksInQueue.Text = "0"
         Me.lblTasksInQueue.TextAlign = System.Drawing.ContentAlignment.TopRight
         '
         'timerTaskUpdater
         '
         Me.timerTaskUpdater.Enabled = True
         '
         'label7
         '
         Me.label7.AutoSize = True
         Me.label7.Location = New System.Drawing.Point(325, 340)
         Me.label7.Name = "label7"
         Me.label7.Size = New System.Drawing.Size(74, 13)
         Me.label7.TabIndex = 8
         Me.label7.Text = "Busy Engines:"
         '
         'lblEnginesBusy
         '
         Me.lblEnginesBusy.Location = New System.Drawing.Point(408, 340)
         Me.lblEnginesBusy.Name = "lblEnginesBusy"
         Me.lblEnginesBusy.Size = New System.Drawing.Size(23, 13)
         Me.lblEnginesBusy.TabIndex = 9
         Me.lblEnginesBusy.Text = "0"
         Me.lblEnginesBusy.TextAlign = System.Drawing.ContentAlignment.TopRight
         '
         'groupBox2
         '
         Me.groupBox2.Controls.Add(Me.lblTaskInfo)
         Me.groupBox2.Location = New System.Drawing.Point(7, 310)
         Me.groupBox2.Name = "groupBox2"
         Me.groupBox2.Size = New System.Drawing.Size(312, 60)
         Me.groupBox2.TabIndex = 7
         Me.groupBox2.TabStop = False
         Me.groupBox2.Text = "Task Description"
         '
         'lblTaskInfo
         '
         Me.lblTaskInfo.Location = New System.Drawing.Point(4, 17)
         Me.lblTaskInfo.MaximumSize = New System.Drawing.Size(295, 37)
         Me.lblTaskInfo.MinimumSize = New System.Drawing.Size(295, 37)
         Me.lblTaskInfo.Name = "lblTaskInfo"
         Me.lblTaskInfo.Size = New System.Drawing.Size(295, 37)
         Me.lblTaskInfo.TabIndex = 0
         Me.lblTaskInfo.Tag = ""
         Me.lblTaskInfo.Text = "No Task Selected."
         '
         'TaskMaster
         '
         Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
         Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
         Me.ClientSize = New System.Drawing.Size(890, 378)
         Me.Controls.Add(Me.groupBox2)
         Me.Controls.Add(Me.lblEnginesBusy)
         Me.Controls.Add(Me.lblTasksInQueue)
         Me.Controls.Add(Me.label7)
         Me.Controls.Add(Me.label6)
         Me.Controls.Add(Me.btnRun)
         Me.Controls.Add(Me.lstOutput)
         Me.Controls.Add(Me.btnRemove)
         Me.Controls.Add(Me.btnAdd)
         Me.Controls.Add(Me.label4)
         Me.Controls.Add(Me.label3)
         Me.Controls.Add(Me.label2)
         Me.Controls.Add(Me.lstRunTasks)
         Me.Controls.Add(Me.lstAvailableTasks)
         Me.Controls.Add(Me.groupBox1)
         Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
         Me.MaximizeBox = False
         Me.Name = "TaskMaster"
         Me.ShowIcon = False
         Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
         Me.Text = "Task Master"
         Me.groupBox1.ResumeLayout(False)
         Me.groupBox1.PerformLayout()
         Me.groupBox2.ResumeLayout(False)
         Me.ResumeLayout(False)
         Me.PerformLayout()

      End Sub

#End Region

        Private groupBox1 As System.Windows.Forms.GroupBox
        Private WithEvents btnStop As System.Windows.Forms.Button
        Private WithEvents btnStart As System.Windows.Forms.Button
        Private label1 As System.Windows.Forms.Label
        Private txtNumEngines As System.Windows.Forms.TextBox
        Private WithEvents lstAvailableTasks As System.Windows.Forms.ListBox
        Private label2 As System.Windows.Forms.Label
        Private WithEvents lstRunTasks As System.Windows.Forms.ListBox
        Private label3 As System.Windows.Forms.Label
        Private WithEvents btnAdd As System.Windows.Forms.Button
        Private WithEvents btnRemove As System.Windows.Forms.Button
        Private lstOutput As System.Windows.Forms.ListBox
        Private label4 As System.Windows.Forms.Label
        Private lblEnginesRunning As System.Windows.Forms.Label
        Private label5 As System.Windows.Forms.Label
        Private WithEvents btnRun As System.Windows.Forms.Button
        Private label6 As System.Windows.Forms.Label
        Private lblTasksInQueue As System.Windows.Forms.Label
        Private WithEvents timerTaskUpdater As System.Windows.Forms.Timer
        Private label7 As System.Windows.Forms.Label
        Private lblEnginesBusy As System.Windows.Forms.Label
        Private groupBox2 As System.Windows.Forms.GroupBox
        Private lblTaskInfo As System.Windows.Forms.Label
    End Class
End Namespace

