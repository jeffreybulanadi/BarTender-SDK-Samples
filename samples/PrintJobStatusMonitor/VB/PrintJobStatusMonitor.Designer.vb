Imports Microsoft.VisualBasic
Imports System
Namespace PrintJobStatusMonitor
    Partial Class PrintJobStatusMonitor
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
         Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
         Me.txtFilename = New System.Windows.Forms.TextBox
         Me.label1 = New System.Windows.Forms.Label
         Me.btnOpen = New System.Windows.Forms.Button
         Me.lstJobStatus = New System.Windows.Forms.ListBox
         Me.btnPrint = New System.Windows.Forms.Button
         Me.label2 = New System.Windows.Forms.Label
         Me.label3 = New System.Windows.Forms.Label
         Me.cboPrinters = New System.Windows.Forms.ComboBox
         Me.statusUpdater = New System.ComponentModel.BackgroundWorker
         Me.panel1 = New System.Windows.Forms.Panel
         Me.picThumbnail = New System.Windows.Forms.PictureBox
         Me.label4 = New System.Windows.Forms.Label
         Me.lstMessages = New System.Windows.Forms.ListBox
         Me.label8 = New System.Windows.Forms.Label
         Me.panel1.SuspendLayout()
         CType(Me.picThumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
         Me.SuspendLayout()
         '
         'openFileDialog
         '
         Me.openFileDialog.DefaultExt = "btw"
         Me.openFileDialog.Filter = "BarTender Label Formats (*.btw)|*.btw"
         Me.openFileDialog.Title = "Open BarTender Label Format"
         '
         'txtFilename
         '
         Me.txtFilename.Location = New System.Drawing.Point(80, 9)
         Me.txtFilename.Name = "txtFilename"
         Me.txtFilename.ReadOnly = True
         Me.txtFilename.Size = New System.Drawing.Size(282, 20)
         Me.txtFilename.TabIndex = 1
         '
         'label1
         '
         Me.label1.AutoSize = True
         Me.label1.Location = New System.Drawing.Point(4, 12)
         Me.label1.Name = "label1"
         Me.label1.Size = New System.Drawing.Size(71, 13)
         Me.label1.TabIndex = 0
         Me.label1.Text = "&Label Format:"
         '
         'btnOpen
         '
         Me.btnOpen.Location = New System.Drawing.Point(368, 7)
         Me.btnOpen.Name = "btnOpen"
         Me.btnOpen.Size = New System.Drawing.Size(75, 23)
         Me.btnOpen.TabIndex = 2
         Me.btnOpen.Text = "&Open..."
         Me.btnOpen.UseVisualStyleBackColor = True
         '
         'lstJobStatus
         '
         Me.lstJobStatus.FormattingEnabled = True
         Me.lstJobStatus.HorizontalScrollbar = True
         Me.lstJobStatus.Location = New System.Drawing.Point(328, 89)
         Me.lstJobStatus.Name = "lstJobStatus"
         Me.lstJobStatus.Size = New System.Drawing.Size(544, 147)
         Me.lstJobStatus.TabIndex = 9
         Me.lstJobStatus.UseTabStops = False
         '
         'btnPrint
         '
         Me.btnPrint.Enabled = False
         Me.btnPrint.Location = New System.Drawing.Point(368, 37)
         Me.btnPrint.Name = "btnPrint"
         Me.btnPrint.Size = New System.Drawing.Size(75, 23)
         Me.btnPrint.TabIndex = 5
         Me.btnPrint.Text = "P&rint"
         Me.btnPrint.UseVisualStyleBackColor = True
         '
         'label2
         '
         Me.label2.AutoSize = True
         Me.label2.Location = New System.Drawing.Point(328, 70)
         Me.label2.Name = "label2"
         Me.label2.Size = New System.Drawing.Size(60, 13)
         Me.label2.TabIndex = 8
         Me.label2.Text = "&Job Status:"
         '
         'label3
         '
         Me.label3.AutoSize = True
         Me.label3.Location = New System.Drawing.Point(4, 42)
         Me.label3.Name = "label3"
         Me.label3.Size = New System.Drawing.Size(40, 13)
         Me.label3.TabIndex = 3
         Me.label3.Text = "&Printer:"
         '
         'cboPrinters
         '
         Me.cboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
         Me.cboPrinters.FormattingEnabled = True
         Me.cboPrinters.Location = New System.Drawing.Point(80, 38)
         Me.cboPrinters.Name = "cboPrinters"
         Me.cboPrinters.Size = New System.Drawing.Size(282, 21)
         Me.cboPrinters.Sorted = True
         Me.cboPrinters.TabIndex = 4
         '
         'statusUpdater
         '
         Me.statusUpdater.WorkerReportsProgress = True
         '
         'panel1
         '
         Me.panel1.BackColor = System.Drawing.Color.Gray
         Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
         Me.panel1.Controls.Add(Me.picThumbnail)
         Me.panel1.Location = New System.Drawing.Point(7, 89)
         Me.panel1.Margin = New System.Windows.Forms.Padding(0)
         Me.panel1.Name = "panel1"
         Me.panel1.Size = New System.Drawing.Size(314, 314)
         Me.panel1.TabIndex = 7
         '
         'picThumbnail
         '
         Me.picThumbnail.Location = New System.Drawing.Point(0, 0)
         Me.picThumbnail.Margin = New System.Windows.Forms.Padding(0)
         Me.picThumbnail.Name = "picThumbnail"
         Me.picThumbnail.Size = New System.Drawing.Size(309, 309)
         Me.picThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
         Me.picThumbnail.TabIndex = 0
         Me.picThumbnail.TabStop = False
         '
         'label4
         '
         Me.label4.AutoSize = True
         Me.label4.Location = New System.Drawing.Point(4, 70)
         Me.label4.Name = "label4"
         Me.label4.Size = New System.Drawing.Size(59, 13)
         Me.label4.TabIndex = 6
         Me.label4.Text = "Thumbnail:"
         '
         'lstMessages
         '
         Me.lstMessages.FormattingEnabled = True
         Me.lstMessages.HorizontalScrollbar = True
         Me.lstMessages.Location = New System.Drawing.Point(328, 256)
         Me.lstMessages.Name = "lstMessages"
         Me.lstMessages.Size = New System.Drawing.Size(544, 147)
         Me.lstMessages.TabIndex = 11
         Me.lstMessages.UseTabStops = False
         '
         'label8
         '
         Me.label8.AutoSize = True
         Me.label8.Location = New System.Drawing.Point(328, 239)
         Me.label8.Name = "label8"
         Me.label8.Size = New System.Drawing.Size(111, 13)
         Me.label8.TabIndex = 10
         Me.label8.Text = "&BarTender Messages:"
         '
         'PrintJobStatusMonitor
         '
         Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
         Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
         Me.ClientSize = New System.Drawing.Size(878, 410)
         Me.Controls.Add(Me.label8)
         Me.Controls.Add(Me.lstMessages)
         Me.Controls.Add(Me.label4)
         Me.Controls.Add(Me.panel1)
         Me.Controls.Add(Me.cboPrinters)
         Me.Controls.Add(Me.label2)
         Me.Controls.Add(Me.btnPrint)
         Me.Controls.Add(Me.lstJobStatus)
         Me.Controls.Add(Me.btnOpen)
         Me.Controls.Add(Me.label3)
         Me.Controls.Add(Me.label1)
         Me.Controls.Add(Me.txtFilename)
         Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
         Me.MaximizeBox = False
         Me.Name = "PrintJobStatusMonitor"
         Me.ShowIcon = False
         Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
         Me.Text = "Print Job Status Monitor"
         Me.panel1.ResumeLayout(False)
         CType(Me.picThumbnail, System.ComponentModel.ISupportInitialize).EndInit()
         Me.ResumeLayout(False)
         Me.PerformLayout()

      End Sub

#End Region

        Private openFileDialog As System.Windows.Forms.OpenFileDialog
        Private txtFilename As System.Windows.Forms.TextBox
        Private label1 As System.Windows.Forms.Label
        Private WithEvents btnOpen As System.Windows.Forms.Button
        Private lstJobStatus As System.Windows.Forms.ListBox
        Private WithEvents btnPrint As System.Windows.Forms.Button
        Private label2 As System.Windows.Forms.Label
        Private picThumbnail As System.Windows.Forms.PictureBox
        Private label3 As System.Windows.Forms.Label
        Private cboPrinters As System.Windows.Forms.ComboBox
        Private WithEvents statusUpdater As System.ComponentModel.BackgroundWorker
        Private panel1 As System.Windows.Forms.Panel
        Private label4 As System.Windows.Forms.Label
        Private lstMessages As System.Windows.Forms.ListBox
        Private label8 As System.Windows.Forms.Label
    End Class
End Namespace

