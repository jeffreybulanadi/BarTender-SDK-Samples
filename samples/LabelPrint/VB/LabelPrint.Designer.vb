Imports Microsoft.VisualBasic
Imports System
Namespace LabelPrint
    Partial Class LabelPrint
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
         Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
         Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
         Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
         Me.txtFolderPath = New System.Windows.Forms.TextBox
         Me.btnOpen = New System.Windows.Forms.Button
         Me.label1 = New System.Windows.Forms.Label
         Me.substringGrid = New System.Windows.Forms.DataGridView
         Me.label2 = New System.Windows.Forms.Label
         Me.cboPrinters = New System.Windows.Forms.ComboBox
         Me.btnPrint = New System.Windows.Forms.Button
         Me.label3 = New System.Windows.Forms.Label
         Me.label4 = New System.Windows.Forms.Label
         Me.txtIdenticalCopies = New System.Windows.Forms.TextBox
         Me.label5 = New System.Windows.Forms.Label
         Me.txtSerializedCopies = New System.Windows.Forms.TextBox
         Me.groupBox1 = New System.Windows.Forms.GroupBox
         Me.lstLabelBrowser = New System.Windows.Forms.ListView
         Me.thumbnailCacheWorker = New System.ComponentModel.BackgroundWorker
         Me.folderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog
         Me.picUpdatingFormat = New System.Windows.Forms.PictureBox
         Me.lblNoSubstrings = New System.Windows.Forms.Label
         Me.lblFormatError = New System.Windows.Forms.Label
         CType(Me.substringGrid, System.ComponentModel.ISupportInitialize).BeginInit()
         Me.groupBox1.SuspendLayout()
         CType(Me.picUpdatingFormat, System.ComponentModel.ISupportInitialize).BeginInit()
         Me.SuspendLayout()
         '
         'txtFolderPath
         '
         Me.txtFolderPath.Location = New System.Drawing.Point(86, 9)
         Me.txtFolderPath.Name = "txtFolderPath"
         Me.txtFolderPath.ReadOnly = True
         Me.txtFolderPath.Size = New System.Drawing.Size(315, 20)
         Me.txtFolderPath.TabIndex = 8
         '
         'btnOpen
         '
         Me.btnOpen.Location = New System.Drawing.Point(407, 7)
         Me.btnOpen.Name = "btnOpen"
         Me.btnOpen.Size = New System.Drawing.Size(75, 23)
         Me.btnOpen.TabIndex = 0
         Me.btnOpen.Text = "&Open..."
         Me.btnOpen.UseVisualStyleBackColor = True
         '
         'label1
         '
         Me.label1.AutoSize = True
         Me.label1.Location = New System.Drawing.Point(4, 12)
         Me.label1.Name = "label1"
         Me.label1.Size = New System.Drawing.Size(68, 13)
         Me.label1.TabIndex = 7
         Me.label1.Text = "&Label Folder:"
         '
         'substringGrid
         '
         Me.substringGrid.AllowUserToAddRows = False
         Me.substringGrid.AllowUserToDeleteRows = False
         Me.substringGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
         Me.substringGrid.BackgroundColor = System.Drawing.Color.White
         Me.substringGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
         DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
         DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
         DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
         DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
         DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
         DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
         DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
         Me.substringGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
         Me.substringGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
         DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
         DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
         DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
         DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
         DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
         DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
         DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
         Me.substringGrid.DefaultCellStyle = DataGridViewCellStyle2
         Me.substringGrid.Location = New System.Drawing.Point(491, 37)
         Me.substringGrid.MultiSelect = False
         Me.substringGrid.Name = "substringGrid"
         DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
         DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
         DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
         DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
         DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
         DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
         DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
         Me.substringGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
         Me.substringGrid.Size = New System.Drawing.Size(389, 325)
         Me.substringGrid.TabIndex = 3
         '
         'label2
         '
         Me.label2.AutoSize = True
         Me.label2.Location = New System.Drawing.Point(491, 14)
         Me.label2.Name = "label2"
         Me.label2.Size = New System.Drawing.Size(96, 13)
         Me.label2.TabIndex = 2
         Me.label2.Text = "&Named Substrings:"
         '
         'cboPrinters
         '
         Me.cboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
         Me.cboPrinters.FormattingEnabled = True
         Me.cboPrinters.Location = New System.Drawing.Point(98, 18)
         Me.cboPrinters.Name = "cboPrinters"
         Me.cboPrinters.Size = New System.Drawing.Size(282, 21)
         Me.cboPrinters.Sorted = True
         Me.cboPrinters.TabIndex = 1
         '
         'btnPrint
         '
         Me.btnPrint.Enabled = False
         Me.btnPrint.Location = New System.Drawing.Point(306, 68)
         Me.btnPrint.Name = "btnPrint"
         Me.btnPrint.Size = New System.Drawing.Size(75, 23)
         Me.btnPrint.TabIndex = 6
         Me.btnPrint.Text = "&Print"
         Me.btnPrint.UseVisualStyleBackColor = True
         '
         'label3
         '
         Me.label3.AutoSize = True
         Me.label3.Location = New System.Drawing.Point(6, 21)
         Me.label3.Name = "label3"
         Me.label3.Size = New System.Drawing.Size(40, 13)
         Me.label3.TabIndex = 0
         Me.label3.Text = "P&rinter:"
         '
         'label4
         '
         Me.label4.AutoSize = True
         Me.label4.Location = New System.Drawing.Point(6, 48)
         Me.label4.Name = "label4"
         Me.label4.Size = New System.Drawing.Size(85, 13)
         Me.label4.TabIndex = 2
         Me.label4.Text = "&Identical Copies:"
         '
         'txtIdenticalCopies
         '
         Me.txtIdenticalCopies.Location = New System.Drawing.Point(98, 45)
         Me.txtIdenticalCopies.Name = "txtIdenticalCopies"
         Me.txtIdenticalCopies.ReadOnly = True
         Me.txtIdenticalCopies.Size = New System.Drawing.Size(112, 20)
         Me.txtIdenticalCopies.TabIndex = 3
         '
         'label5
         '
         Me.label5.AutoSize = True
         Me.label5.Location = New System.Drawing.Point(6, 74)
         Me.label5.Name = "label5"
         Me.label5.Size = New System.Drawing.Size(90, 13)
         Me.label5.TabIndex = 4
         Me.label5.Text = "&Serialized Copies:"
         '
         'txtSerializedCopies
         '
         Me.txtSerializedCopies.Location = New System.Drawing.Point(98, 71)
         Me.txtSerializedCopies.Name = "txtSerializedCopies"
         Me.txtSerializedCopies.ReadOnly = True
         Me.txtSerializedCopies.Size = New System.Drawing.Size(112, 20)
         Me.txtSerializedCopies.TabIndex = 5
         '
         'groupBox1
         '
         Me.groupBox1.Controls.Add(Me.txtIdenticalCopies)
         Me.groupBox1.Controls.Add(Me.label5)
         Me.groupBox1.Controls.Add(Me.txtSerializedCopies)
         Me.groupBox1.Controls.Add(Me.label4)
         Me.groupBox1.Controls.Add(Me.label3)
         Me.groupBox1.Controls.Add(Me.btnPrint)
         Me.groupBox1.Controls.Add(Me.cboPrinters)
         Me.groupBox1.Location = New System.Drawing.Point(491, 368)
         Me.groupBox1.Name = "groupBox1"
         Me.groupBox1.Size = New System.Drawing.Size(389, 99)
         Me.groupBox1.TabIndex = 6
         Me.groupBox1.TabStop = False
         Me.groupBox1.Text = "Printing"
         '
         'lstLabelBrowser
         '
         Me.lstLabelBrowser.BackColor = System.Drawing.Color.White
         Me.lstLabelBrowser.HideSelection = False
         Me.lstLabelBrowser.Location = New System.Drawing.Point(7, 37)
         Me.lstLabelBrowser.MultiSelect = False
         Me.lstLabelBrowser.Name = "lstLabelBrowser"
         Me.lstLabelBrowser.Size = New System.Drawing.Size(475, 430)
         Me.lstLabelBrowser.TabIndex = 1
         Me.lstLabelBrowser.UseCompatibleStateImageBehavior = False
         Me.lstLabelBrowser.VirtualMode = True
         '
         'thumbnailCacheWorker
         '
         Me.thumbnailCacheWorker.WorkerReportsProgress = True
         Me.thumbnailCacheWorker.WorkerSupportsCancellation = True
         '
         'folderBrowserDialog
         '
         Me.folderBrowserDialog.ShowNewFolderButton = False
         '
         'picUpdatingFormat
         '
         Me.picUpdatingFormat.BackColor = System.Drawing.Color.White
         Me.picUpdatingFormat.Image = My.Resources.updating
         Me.picUpdatingFormat.Location = New System.Drawing.Point(675, 198)
         Me.picUpdatingFormat.Name = "picUpdatingFormat"
         Me.picUpdatingFormat.Size = New System.Drawing.Size(28, 25)
         Me.picUpdatingFormat.TabIndex = 34
         Me.picUpdatingFormat.TabStop = False
         Me.picUpdatingFormat.Visible = False
         '
         'lblNoSubstrings
         '
         Me.lblNoSubstrings.AutoSize = True
         Me.lblNoSubstrings.BackColor = System.Drawing.Color.White
         Me.lblNoSubstrings.Location = New System.Drawing.Point(565, 175)
         Me.lblNoSubstrings.Name = "lblNoSubstrings"
         Me.lblNoSubstrings.Size = New System.Drawing.Size(242, 13)
         Me.lblNoSubstrings.TabIndex = 4
         Me.lblNoSubstrings.Text = "This label does not contain any named substrings."
         Me.lblNoSubstrings.Visible = False
         '
         'lblFormatError
         '
         Me.lblFormatError.AutoSize = True
         Me.lblFormatError.BackColor = System.Drawing.Color.White
         Me.lblFormatError.Location = New System.Drawing.Point(588, 190)
         Me.lblFormatError.Name = "lblFormatError"
         Me.lblFormatError.Size = New System.Drawing.Size(191, 13)
         Me.lblFormatError.TabIndex = 5
         Me.lblFormatError.Text = "There was an error opening this format."
         Me.lblFormatError.Visible = False
         '
         'LabelPrint
         '
         Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
         Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
         Me.ClientSize = New System.Drawing.Size(887, 476)
         Me.Controls.Add(Me.lblFormatError)
         Me.Controls.Add(Me.lblNoSubstrings)
         Me.Controls.Add(Me.picUpdatingFormat)
         Me.Controls.Add(Me.lstLabelBrowser)
         Me.Controls.Add(Me.groupBox1)
         Me.Controls.Add(Me.label2)
         Me.Controls.Add(Me.substringGrid)
         Me.Controls.Add(Me.txtFolderPath)
         Me.Controls.Add(Me.btnOpen)
         Me.Controls.Add(Me.label1)
         Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
         Me.MaximizeBox = False
         Me.Name = "LabelPrint"
         Me.ShowIcon = False
         Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
         Me.Text = "Label Print"
         CType(Me.substringGrid, System.ComponentModel.ISupportInitialize).EndInit()
         Me.groupBox1.ResumeLayout(False)
         Me.groupBox1.PerformLayout()
         CType(Me.picUpdatingFormat, System.ComponentModel.ISupportInitialize).EndInit()
         Me.ResumeLayout(False)
         Me.PerformLayout()

      End Sub

#End Region

        Private txtFolderPath As System.Windows.Forms.TextBox
        Private WithEvents btnOpen As System.Windows.Forms.Button
        Private label1 As System.Windows.Forms.Label
        Private substringGrid As System.Windows.Forms.DataGridView
        Private label2 As System.Windows.Forms.Label
        Private cboPrinters As System.Windows.Forms.ComboBox
        Private WithEvents btnPrint As System.Windows.Forms.Button
        Private label3 As System.Windows.Forms.Label
        Private label4 As System.Windows.Forms.Label
        Private txtIdenticalCopies As System.Windows.Forms.TextBox
        Private label5 As System.Windows.Forms.Label
        Private txtSerializedCopies As System.Windows.Forms.TextBox
        Private groupBox1 As System.Windows.Forms.GroupBox
        Private WithEvents lstLabelBrowser As System.Windows.Forms.ListView
        Private WithEvents thumbnailCacheWorker As System.ComponentModel.BackgroundWorker
        Private folderBrowserDialog As System.Windows.Forms.FolderBrowserDialog
        Private picUpdatingFormat As System.Windows.Forms.PictureBox
        Private lblNoSubstrings As System.Windows.Forms.Label
        Private lblFormatError As System.Windows.Forms.Label
    End Class
End Namespace

