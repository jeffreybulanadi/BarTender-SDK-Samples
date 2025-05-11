Imports Microsoft.VisualBasic
Imports System
Namespace PrintPreview
    Partial Class PrintPreview
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
         Me.cboPrinters = New System.Windows.Forms.ComboBox
         Me.label4 = New System.Windows.Forms.Label
         Me.btnPreview = New System.Windows.Forms.Button
         Me.btnOpen = New System.Windows.Forms.Button
         Me.label3 = New System.Windows.Forms.Label
         Me.label1 = New System.Windows.Forms.Label
         Me.txtFilename = New System.Windows.Forms.TextBox
         Me.panel1 = New System.Windows.Forms.Panel
         Me.picUpdating = New System.Windows.Forms.PictureBox
         Me.picPreview = New System.Windows.Forms.PictureBox
         Me.backgroundWorker = New System.ComponentModel.BackgroundWorker
         Me.btnFirst = New System.Windows.Forms.Button
         Me.btnPrev = New System.Windows.Forms.Button
         Me.btnLast = New System.Windows.Forms.Button
         Me.btnNext = New System.Windows.Forms.Button
         Me.lblNumPreviews = New System.Windows.Forms.Label
         Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
         Me.panel1.SuspendLayout()
         CType(Me.picUpdating, System.ComponentModel.ISupportInitialize).BeginInit()
         CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
         Me.SuspendLayout()
         '
         'cboPrinters
         '
         Me.cboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
         Me.cboPrinters.FormattingEnabled = True
         Me.cboPrinters.Location = New System.Drawing.Point(83, 39)
         Me.cboPrinters.Name = "cboPrinters"
         Me.cboPrinters.Size = New System.Drawing.Size(344, 21)
         Me.cboPrinters.Sorted = True
         Me.cboPrinters.TabIndex = 4
         '
         'label4
         '
         Me.label4.AutoSize = True
         Me.label4.Location = New System.Drawing.Point(7, 71)
         Me.label4.Name = "label4"
         Me.label4.Size = New System.Drawing.Size(72, 13)
         Me.label4.TabIndex = 6
         Me.label4.Text = "Print Preview:"
         '
         'btnPreview
         '
         Me.btnPreview.Enabled = False
         Me.btnPreview.Location = New System.Drawing.Point(434, 39)
         Me.btnPreview.Name = "btnPreview"
         Me.btnPreview.Size = New System.Drawing.Size(75, 23)
         Me.btnPreview.TabIndex = 5
         Me.btnPreview.Text = "P&review"
         Me.btnPreview.UseVisualStyleBackColor = True
         '
         'btnOpen
         '
         Me.btnOpen.Location = New System.Drawing.Point(434, 9)
         Me.btnOpen.Name = "btnOpen"
         Me.btnOpen.Size = New System.Drawing.Size(75, 23)
         Me.btnOpen.TabIndex = 2
         Me.btnOpen.Text = "&Open..."
         Me.btnOpen.UseVisualStyleBackColor = True
         '
         'label3
         '
         Me.label3.AutoSize = True
         Me.label3.Location = New System.Drawing.Point(7, 43)
         Me.label3.Name = "label3"
         Me.label3.Size = New System.Drawing.Size(40, 13)
         Me.label3.TabIndex = 3
         Me.label3.Text = "&Printer:"
         '
         'label1
         '
         Me.label1.AutoSize = True
         Me.label1.Location = New System.Drawing.Point(7, 13)
         Me.label1.Name = "label1"
         Me.label1.Size = New System.Drawing.Size(71, 13)
         Me.label1.TabIndex = 0
         Me.label1.Text = "&Label Format:"
         '
         'txtFilename
         '
         Me.txtFilename.Location = New System.Drawing.Point(83, 10)
         Me.txtFilename.Name = "txtFilename"
         Me.txtFilename.ReadOnly = True
         Me.txtFilename.Size = New System.Drawing.Size(345, 20)
         Me.txtFilename.TabIndex = 1
         '
         'panel1
         '
         Me.panel1.BackColor = System.Drawing.Color.Gray
         Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
         Me.panel1.Controls.Add(Me.picUpdating)
         Me.panel1.Controls.Add(Me.picPreview)
         Me.panel1.Location = New System.Drawing.Point(9, 87)
         Me.panel1.Margin = New System.Windows.Forms.Padding(0)
         Me.panel1.Name = "panel1"
         Me.panel1.Size = New System.Drawing.Size(500, 500)
         Me.panel1.TabIndex = 7
         '
         'picUpdating
         '
         Me.picUpdating.BackColor = System.Drawing.Color.White
         Me.picUpdating.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
         Me.picUpdating.Image = My.Resources.updating
         Me.picUpdating.Location = New System.Drawing.Point(236, 236)
         Me.picUpdating.Name = "picUpdating"
         Me.picUpdating.Size = New System.Drawing.Size(24, 24)
         Me.picUpdating.TabIndex = 12
         Me.picUpdating.TabStop = False
         Me.picUpdating.Visible = False
         '
         'picPreview
         '
         Me.picPreview.BackColor = System.Drawing.Color.Gray
         Me.picPreview.Location = New System.Drawing.Point(0, 0)
         Me.picPreview.Margin = New System.Windows.Forms.Padding(0)
         Me.picPreview.Name = "picPreview"
         Me.picPreview.Size = New System.Drawing.Size(495, 495)
         Me.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
         Me.picPreview.TabIndex = 11
         Me.picPreview.TabStop = False
         '
         'backgroundWorker
         '
         '
         'btnFirst
         '
         Me.btnFirst.Location = New System.Drawing.Point(8, 593)
         Me.btnFirst.Name = "btnFirst"
         Me.btnFirst.Size = New System.Drawing.Size(37, 23)
         Me.btnFirst.TabIndex = 8
         Me.btnFirst.Text = "<<"
         Me.btnFirst.UseVisualStyleBackColor = True
         '
         'btnPrev
         '
         Me.btnPrev.Location = New System.Drawing.Point(51, 593)
         Me.btnPrev.Name = "btnPrev"
         Me.btnPrev.Size = New System.Drawing.Size(37, 23)
         Me.btnPrev.TabIndex = 9
         Me.btnPrev.Text = "<"
         Me.btnPrev.UseVisualStyleBackColor = True
         '
         'btnLast
         '
         Me.btnLast.Location = New System.Drawing.Point(472, 593)
         Me.btnLast.Name = "btnLast"
         Me.btnLast.Size = New System.Drawing.Size(37, 23)
         Me.btnLast.TabIndex = 12
         Me.btnLast.Text = ">>"
         Me.btnLast.UseVisualStyleBackColor = True
         '
         'btnNext
         '
         Me.btnNext.Location = New System.Drawing.Point(429, 593)
         Me.btnNext.Name = "btnNext"
         Me.btnNext.Size = New System.Drawing.Size(37, 23)
         Me.btnNext.TabIndex = 11
         Me.btnNext.Text = ">"
         Me.btnNext.UseVisualStyleBackColor = True
         '
         'lblNumPreviews
         '
         Me.lblNumPreviews.AutoSize = True
         Me.lblNumPreviews.Location = New System.Drawing.Point(226, 598)
         Me.lblNumPreviews.Name = "lblNumPreviews"
         Me.lblNumPreviews.Size = New System.Drawing.Size(62, 13)
         Me.lblNumPreviews.TabIndex = 10
         Me.lblNumPreviews.Text = "Page 0 of 0"
         '
         'openFileDialog
         '
         Me.openFileDialog.DefaultExt = "btw"
         Me.openFileDialog.Filter = "BarTender Label Formats (*.btw)|*.btw"
         Me.openFileDialog.Title = "Open BarTender Label Format"
         '
         'PrintPreview
         '
         Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
         Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
         Me.ClientSize = New System.Drawing.Size(516, 623)
         Me.Controls.Add(Me.lblNumPreviews)
         Me.Controls.Add(Me.btnNext)
         Me.Controls.Add(Me.btnLast)
         Me.Controls.Add(Me.btnPrev)
         Me.Controls.Add(Me.btnFirst)
         Me.Controls.Add(Me.panel1)
         Me.Controls.Add(Me.cboPrinters)
         Me.Controls.Add(Me.label4)
         Me.Controls.Add(Me.btnPreview)
         Me.Controls.Add(Me.btnOpen)
         Me.Controls.Add(Me.label3)
         Me.Controls.Add(Me.label1)
         Me.Controls.Add(Me.txtFilename)
         Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
         Me.MaximizeBox = False
         Me.Name = "PrintPreview"
         Me.ShowIcon = False
         Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
         Me.Text = "Print Preview"
         Me.panel1.ResumeLayout(False)
         CType(Me.picUpdating, System.ComponentModel.ISupportInitialize).EndInit()
         CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
         Me.ResumeLayout(False)
         Me.PerformLayout()

      End Sub

#End Region

        Private cboPrinters As System.Windows.Forms.ComboBox
        Private label4 As System.Windows.Forms.Label
        Private picPreview As System.Windows.Forms.PictureBox
        Private WithEvents btnPreview As System.Windows.Forms.Button
        Private WithEvents btnOpen As System.Windows.Forms.Button
        Private label3 As System.Windows.Forms.Label
        Private label1 As System.Windows.Forms.Label
        Private txtFilename As System.Windows.Forms.TextBox
        Private panel1 As System.Windows.Forms.Panel
        Private WithEvents backgroundWorker As System.ComponentModel.BackgroundWorker
        Private WithEvents btnFirst As System.Windows.Forms.Button
        Private WithEvents btnPrev As System.Windows.Forms.Button
        Private WithEvents btnLast As System.Windows.Forms.Button
        Private WithEvents btnNext As System.Windows.Forms.Button
        Private lblNumPreviews As System.Windows.Forms.Label
        Private picUpdating As System.Windows.Forms.PictureBox
        Private openFileDialog As System.Windows.Forms.OpenFileDialog
    End Class
End Namespace

