Imports Microsoft.VisualBasic
Imports System
Namespace PrintToFileManager
    Partial Class PrintToFileManager
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
         Me.btnOpenBTW = New System.Windows.Forms.Button
         Me.label1 = New System.Windows.Forms.Label
         Me.txtLabelFilename = New System.Windows.Forms.TextBox
         Me.btnPrintToFile = New System.Windows.Forms.Button
         Me.btnPrintPrinterFile = New System.Windows.Forms.Button
         Me.txtPrinterFileFilename = New System.Windows.Forms.TextBox
         Me.label2 = New System.Windows.Forms.Label
         Me.btnOpenPrinterFile = New System.Windows.Forms.Button
         Me.btnGenerateLicense = New System.Windows.Forms.Button
         Me.txtPrintLicense = New System.Windows.Forms.TextBox
         Me.label5 = New System.Windows.Forms.Label
         Me.openBTWDialog = New System.Windows.Forms.OpenFileDialog
         Me.openPrinterFileDialog = New System.Windows.Forms.OpenFileDialog
         Me.label6 = New System.Windows.Forms.Label
         Me.cboStep1Printer = New System.Windows.Forms.ComboBox
         Me.savePrinterFileDialog = New System.Windows.Forms.SaveFileDialog
         Me.groupBox1 = New System.Windows.Forms.GroupBox
         Me.groupBox2 = New System.Windows.Forms.GroupBox
         Me.cboStep2Printer = New System.Windows.Forms.ComboBox
         Me.label3 = New System.Windows.Forms.Label
         Me.groupBox3 = New System.Windows.Forms.GroupBox
         Me.cboStep3Printer = New System.Windows.Forms.ComboBox
         Me.label4 = New System.Windows.Forms.Label
         Me.groupBox1.SuspendLayout()
         Me.groupBox2.SuspendLayout()
         Me.groupBox3.SuspendLayout()
         Me.SuspendLayout()
         '
         'btnOpenBTW
         '
         Me.btnOpenBTW.Location = New System.Drawing.Point(406, 23)
         Me.btnOpenBTW.Name = "btnOpenBTW"
         Me.btnOpenBTW.Size = New System.Drawing.Size(84, 23)
         Me.btnOpenBTW.TabIndex = 2
         Me.btnOpenBTW.Text = "&Open..."
         Me.btnOpenBTW.UseVisualStyleBackColor = True
         '
         'label1
         '
         Me.label1.AutoSize = True
         Me.label1.Location = New System.Drawing.Point(4, 28)
         Me.label1.Name = "label1"
         Me.label1.Size = New System.Drawing.Size(71, 13)
         Me.label1.TabIndex = 0
         Me.label1.Text = "Label For&mat:"
         '
         'txtLabelFilename
         '
         Me.txtLabelFilename.Location = New System.Drawing.Point(87, 25)
         Me.txtLabelFilename.Name = "txtLabelFilename"
         Me.txtLabelFilename.ReadOnly = True
         Me.txtLabelFilename.Size = New System.Drawing.Size(312, 20)
         Me.txtLabelFilename.TabIndex = 1
         '
         'btnPrintToFile
         '
         Me.btnPrintToFile.Enabled = False
         Me.btnPrintToFile.Location = New System.Drawing.Point(406, 54)
         Me.btnPrintToFile.Name = "btnPrintToFile"
         Me.btnPrintToFile.Size = New System.Drawing.Size(84, 23)
         Me.btnPrintToFile.TabIndex = 5
         Me.btnPrintToFile.Text = "Print &To File..."
         Me.btnPrintToFile.UseVisualStyleBackColor = True
         '
         'btnPrintPrinterFile
         '
         Me.btnPrintPrinterFile.Enabled = False
         Me.btnPrintPrinterFile.Location = New System.Drawing.Point(406, 54)
         Me.btnPrintPrinterFile.Name = "btnPrintPrinterFile"
         Me.btnPrintPrinterFile.Size = New System.Drawing.Size(84, 23)
         Me.btnPrintPrinterFile.TabIndex = 5
         Me.btnPrintPrinterFile.Text = "Pri&nt"
         Me.btnPrintPrinterFile.UseVisualStyleBackColor = True
         '
         'txtPrinterFileFilename
         '
         Me.txtPrinterFileFilename.Location = New System.Drawing.Point(87, 25)
         Me.txtPrinterFileFilename.Name = "txtPrinterFileFilename"
         Me.txtPrinterFileFilename.ReadOnly = True
         Me.txtPrinterFileFilename.Size = New System.Drawing.Size(311, 20)
         Me.txtPrinterFileFilename.TabIndex = 1
         '
         'label2
         '
         Me.label2.AutoSize = True
         Me.label2.Location = New System.Drawing.Point(4, 28)
         Me.label2.Name = "label2"
         Me.label2.Size = New System.Drawing.Size(59, 13)
         Me.label2.TabIndex = 0
         Me.label2.Text = "Printer &File:"
         '
         'btnOpenPrinterFile
         '
         Me.btnOpenPrinterFile.Location = New System.Drawing.Point(406, 23)
         Me.btnOpenPrinterFile.Name = "btnOpenPrinterFile"
         Me.btnOpenPrinterFile.Size = New System.Drawing.Size(84, 23)
         Me.btnOpenPrinterFile.TabIndex = 2
         Me.btnOpenPrinterFile.Text = "Op&en..."
         Me.btnOpenPrinterFile.UseVisualStyleBackColor = True
         '
         'btnGenerateLicense
         '
         Me.btnGenerateLicense.Location = New System.Drawing.Point(406, 18)
         Me.btnGenerateLicense.Name = "btnGenerateLicense"
         Me.btnGenerateLicense.Size = New System.Drawing.Size(84, 23)
         Me.btnGenerateLicense.TabIndex = 2
         Me.btnGenerateLicense.Text = "&Generate"
         Me.btnGenerateLicense.UseVisualStyleBackColor = True
         '
         'txtPrintLicense
         '
         Me.txtPrintLicense.Location = New System.Drawing.Point(87, 50)
         Me.txtPrintLicense.Multiline = True
         Me.txtPrintLicense.Name = "txtPrintLicense"
         Me.txtPrintLicense.ReadOnly = True
         Me.txtPrintLicense.Size = New System.Drawing.Size(312, 127)
         Me.txtPrintLicense.TabIndex = 4
         '
         'label5
         '
         Me.label5.AutoSize = True
         Me.label5.Location = New System.Drawing.Point(4, 53)
         Me.label5.Name = "label5"
         Me.label5.Size = New System.Drawing.Size(71, 13)
         Me.label5.TabIndex = 3
         Me.label5.Text = "Print &License:"
         '
         'openBTWDialog
         '
         Me.openBTWDialog.DefaultExt = "btw"
         Me.openBTWDialog.Filter = "BarTender Label Formats (*.btw)|*.btw"
         Me.openBTWDialog.Title = "Open BarTender Label Format"
         '
         'openPrinterFileDialog
         '
         Me.openPrinterFileDialog.DefaultExt = "btw"
         Me.openPrinterFileDialog.Filter = "Printer Files (*.prn)|*.prn|All files (*.*)|*.*"
         Me.openPrinterFileDialog.Title = "Open Printer File"
         '
         'label6
         '
         Me.label6.AutoSize = True
         Me.label6.Location = New System.Drawing.Point(4, 22)
         Me.label6.Name = "label6"
         Me.label6.Size = New System.Drawing.Size(40, 13)
         Me.label6.TabIndex = 0
         Me.label6.Text = "&Printer:"
         '
         'cboStep1Printer
         '
         Me.cboStep1Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
         Me.cboStep1Printer.FormattingEnabled = True
         Me.cboStep1Printer.Location = New System.Drawing.Point(87, 19)
         Me.cboStep1Printer.Name = "cboStep1Printer"
         Me.cboStep1Printer.Size = New System.Drawing.Size(311, 21)
         Me.cboStep1Printer.Sorted = True
         Me.cboStep1Printer.TabIndex = 1
         '
         'savePrinterFileDialog
         '
         Me.savePrinterFileDialog.DefaultExt = "prn"
         Me.savePrinterFileDialog.FileName = "printerFile"
         Me.savePrinterFileDialog.Filter = "Printer Files (*.prn)|*.prn|All files|*.*"
         Me.savePrinterFileDialog.SupportMultiDottedExtensions = True
         Me.savePrinterFileDialog.Title = "Save Printer File"
         '
         'groupBox1
         '
         Me.groupBox1.Controls.Add(Me.cboStep1Printer)
         Me.groupBox1.Controls.Add(Me.btnGenerateLicense)
         Me.groupBox1.Controls.Add(Me.label6)
         Me.groupBox1.Controls.Add(Me.label5)
         Me.groupBox1.Controls.Add(Me.txtPrintLicense)
         Me.groupBox1.Location = New System.Drawing.Point(7, 7)
         Me.groupBox1.Name = "groupBox1"
         Me.groupBox1.Size = New System.Drawing.Size(499, 187)
         Me.groupBox1.TabIndex = 0
         Me.groupBox1.TabStop = False
         Me.groupBox1.Text = "Step 1: Obtain a printing license on the client computer."
         '
         'groupBox2
         '
         Me.groupBox2.Controls.Add(Me.cboStep2Printer)
         Me.groupBox2.Controls.Add(Me.btnPrintToFile)
         Me.groupBox2.Controls.Add(Me.label3)
         Me.groupBox2.Controls.Add(Me.txtLabelFilename)
         Me.groupBox2.Controls.Add(Me.btnOpenBTW)
         Me.groupBox2.Controls.Add(Me.label1)
         Me.groupBox2.Location = New System.Drawing.Point(7, 200)
         Me.groupBox2.Name = "groupBox2"
         Me.groupBox2.Size = New System.Drawing.Size(499, 85)
         Me.groupBox2.TabIndex = 1
         Me.groupBox2.TabStop = False
         Me.groupBox2.Text = "Step 2: Print to file on the server computer using the print license and a compat" & _
             "ible printer driver."
         '
         'cboStep2Printer
         '
         Me.cboStep2Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
         Me.cboStep2Printer.FormattingEnabled = True
         Me.cboStep2Printer.Location = New System.Drawing.Point(87, 55)
         Me.cboStep2Printer.Name = "cboStep2Printer"
         Me.cboStep2Printer.Size = New System.Drawing.Size(311, 21)
         Me.cboStep2Printer.Sorted = True
         Me.cboStep2Printer.TabIndex = 4
         '
         'label3
         '
         Me.label3.AutoSize = True
         Me.label3.Location = New System.Drawing.Point(4, 58)
         Me.label3.Name = "label3"
         Me.label3.Size = New System.Drawing.Size(40, 13)
         Me.label3.TabIndex = 3
         Me.label3.Text = "P&rinter:"
         '
         'groupBox3
         '
         Me.groupBox3.Controls.Add(Me.cboStep3Printer)
         Me.groupBox3.Controls.Add(Me.label4)
         Me.groupBox3.Controls.Add(Me.btnOpenPrinterFile)
         Me.groupBox3.Controls.Add(Me.btnPrintPrinterFile)
         Me.groupBox3.Controls.Add(Me.label2)
         Me.groupBox3.Controls.Add(Me.txtPrinterFileFilename)
         Me.groupBox3.Location = New System.Drawing.Point(7, 292)
         Me.groupBox3.Name = "groupBox3"
         Me.groupBox3.Size = New System.Drawing.Size(499, 85)
         Me.groupBox3.TabIndex = 2
         Me.groupBox3.TabStop = False
         Me.groupBox3.Text = "Step 3: Print the printer file on the client computer."
         '
         'cboStep3Printer
         '
         Me.cboStep3Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
         Me.cboStep3Printer.FormattingEnabled = True
         Me.cboStep3Printer.Location = New System.Drawing.Point(87, 55)
         Me.cboStep3Printer.Name = "cboStep3Printer"
         Me.cboStep3Printer.Size = New System.Drawing.Size(311, 21)
         Me.cboStep3Printer.Sorted = True
         Me.cboStep3Printer.TabIndex = 4
         '
         'label4
         '
         Me.label4.AutoSize = True
         Me.label4.Location = New System.Drawing.Point(4, 58)
         Me.label4.Name = "label4"
         Me.label4.Size = New System.Drawing.Size(40, 13)
         Me.label4.TabIndex = 3
         Me.label4.Text = "Pr&inter:"
         '
         'PrintToFileManager
         '
         Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
         Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
         Me.ClientSize = New System.Drawing.Size(514, 385)
         Me.Controls.Add(Me.groupBox3)
         Me.Controls.Add(Me.groupBox2)
         Me.Controls.Add(Me.groupBox1)
         Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
         Me.MaximizeBox = False
         Me.Name = "PrintToFileManager"
         Me.ShowIcon = False
         Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
         Me.Text = "Print To File Manager"
         Me.groupBox1.ResumeLayout(False)
         Me.groupBox1.PerformLayout()
         Me.groupBox2.ResumeLayout(False)
         Me.groupBox2.PerformLayout()
         Me.groupBox3.ResumeLayout(False)
         Me.groupBox3.PerformLayout()
         Me.ResumeLayout(False)

      End Sub

#End Region

        Private WithEvents btnOpenBTW As System.Windows.Forms.Button
        Private label1 As System.Windows.Forms.Label
        Private txtLabelFilename As System.Windows.Forms.TextBox
        Private txtPrinterFileFilename As System.Windows.Forms.TextBox
        Private label2 As System.Windows.Forms.Label
        Private WithEvents btnOpenPrinterFile As System.Windows.Forms.Button
        Private openBTWDialog As System.Windows.Forms.OpenFileDialog
        Private openPrinterFileDialog As System.Windows.Forms.OpenFileDialog
        Private label5 As System.Windows.Forms.Label
        Private txtPrintLicense As System.Windows.Forms.TextBox
        Private WithEvents btnPrintToFile As System.Windows.Forms.Button
        Private label6 As System.Windows.Forms.Label
        Private WithEvents cboStep1Printer As System.Windows.Forms.ComboBox
        Private savePrinterFileDialog As System.Windows.Forms.SaveFileDialog
        Private WithEvents btnGenerateLicense As System.Windows.Forms.Button
        Private WithEvents btnPrintPrinterFile As System.Windows.Forms.Button
        Private groupBox1 As System.Windows.Forms.GroupBox
        Private groupBox2 As System.Windows.Forms.GroupBox
        Private cboStep2Printer As System.Windows.Forms.ComboBox
        Private label3 As System.Windows.Forms.Label
        Private groupBox3 As System.Windows.Forms.GroupBox
        Private cboStep3Printer As System.Windows.Forms.ComboBox
        Private label4 As System.Windows.Forms.Label
    End Class
End Namespace

