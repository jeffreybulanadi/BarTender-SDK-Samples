Imports Microsoft.VisualBasic
Imports System
Namespace XMLScripter
    Partial Class XMLScripter
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
         Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XMLScripter))
         Me.txtXMLScript = New System.Windows.Forms.TextBox
         Me.label1 = New System.Windows.Forms.Label
         Me.label3 = New System.Windows.Forms.Label
         Me.btnRunXMLScript = New System.Windows.Forms.Button
         Me.webXMLResponse = New System.Windows.Forms.WebBrowser
         Me.backgroundWorker = New System.ComponentModel.BackgroundWorker
         Me.panel1 = New System.Windows.Forms.Panel
         Me.picUpdating = New System.Windows.Forms.PictureBox
         Me.btnLoadXMLScript = New System.Windows.Forms.Button
         Me.btnSaveXMLScript = New System.Windows.Forms.Button
         Me.btnCopyResponseToClipboard = New System.Windows.Forms.Button
         Me.panel1.SuspendLayout()
         CType(Me.picUpdating, System.ComponentModel.ISupportInitialize).BeginInit()
         Me.SuspendLayout()
         '
         'txtXMLScript
         '
         Me.txtXMLScript.AcceptsReturn = True
         Me.txtXMLScript.AcceptsTab = True
         Me.txtXMLScript.BackColor = System.Drawing.SystemColors.Window
         Me.txtXMLScript.Location = New System.Drawing.Point(7, 34)
         Me.txtXMLScript.Multiline = True
         Me.txtXMLScript.Name = "txtXMLScript"
         Me.txtXMLScript.ScrollBars = System.Windows.Forms.ScrollBars.Both
         Me.txtXMLScript.Size = New System.Drawing.Size(551, 279)
         Me.txtXMLScript.TabIndex = 1
         Me.txtXMLScript.TabStop = False
         Me.txtXMLScript.Text = resources.GetString("txtXMLScript.Text")
         Me.txtXMLScript.WordWrap = False
         '
         'label1
         '
         Me.label1.AutoSize = True
         Me.label1.Location = New System.Drawing.Point(7, 18)
         Me.label1.Name = "label1"
         Me.label1.Size = New System.Drawing.Size(62, 13)
         Me.label1.TabIndex = 0
         Me.label1.Text = "&XML Script:"
         '
         'label3
         '
         Me.label3.AutoSize = True
         Me.label3.Location = New System.Drawing.Point(7, 333)
         Me.label3.Name = "label3"
         Me.label3.Size = New System.Drawing.Size(83, 13)
         Me.label3.TabIndex = 5
         Me.label3.Text = "X&ML Response:"
         '
         'btnRunXMLScript
         '
         Me.btnRunXMLScript.Location = New System.Drawing.Point(444, 319)
         Me.btnRunXMLScript.Name = "btnRunXMLScript"
         Me.btnRunXMLScript.Size = New System.Drawing.Size(114, 23)
         Me.btnRunXMLScript.TabIndex = 4
         Me.btnRunXMLScript.Text = "&Run XML Script"
         Me.btnRunXMLScript.UseVisualStyleBackColor = True
         '
         'webXMLResponse
         '
         Me.webXMLResponse.AllowWebBrowserDrop = False
         Me.webXMLResponse.IsWebBrowserContextMenuEnabled = False
         Me.webXMLResponse.Location = New System.Drawing.Point(-2, 0)
         Me.webXMLResponse.MinimumSize = New System.Drawing.Size(20, 20)
         Me.webXMLResponse.Name = "webXMLResponse"
         Me.webXMLResponse.Size = New System.Drawing.Size(549, 276)
         Me.webXMLResponse.TabIndex = 0
         Me.webXMLResponse.TabStop = False
         Me.webXMLResponse.WebBrowserShortcutsEnabled = False
         '
         'backgroundWorker
         '
         Me.backgroundWorker.WorkerReportsProgress = True
         '
         'panel1
         '
         Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
         Me.panel1.Controls.Add(Me.picUpdating)
         Me.panel1.Controls.Add(Me.webXMLResponse)
         Me.panel1.Location = New System.Drawing.Point(7, 349)
         Me.panel1.Name = "panel1"
         Me.panel1.Size = New System.Drawing.Size(551, 279)
         Me.panel1.TabIndex = 6
         '
         'picUpdating
         '
         Me.picUpdating.BackColor = System.Drawing.Color.White
         Me.picUpdating.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
         Me.picUpdating.Image = Global.My.Resources.Resources.updating
         Me.picUpdating.Location = New System.Drawing.Point(260, 134)
         Me.picUpdating.Name = "picUpdating"
         Me.picUpdating.Size = New System.Drawing.Size(24, 24)
         Me.picUpdating.TabIndex = 6
         Me.picUpdating.TabStop = False
         Me.picUpdating.Visible = False
         '
         'btnLoadXMLScript
         '
         Me.btnLoadXMLScript.Location = New System.Drawing.Point(401, 5)
         Me.btnLoadXMLScript.Name = "btnLoadXMLScript"
         Me.btnLoadXMLScript.Size = New System.Drawing.Size(75, 23)
         Me.btnLoadXMLScript.TabIndex = 2
         Me.btnLoadXMLScript.Text = "&Load..."
         Me.btnLoadXMLScript.UseVisualStyleBackColor = True
         '
         'btnSaveXMLScript
         '
         Me.btnSaveXMLScript.Location = New System.Drawing.Point(483, 5)
         Me.btnSaveXMLScript.Name = "btnSaveXMLScript"
         Me.btnSaveXMLScript.Size = New System.Drawing.Size(75, 23)
         Me.btnSaveXMLScript.TabIndex = 3
         Me.btnSaveXMLScript.Text = "&Save..."
         Me.btnSaveXMLScript.UseVisualStyleBackColor = True
         '
         'btnCopyResponseToClipboard
         '
         Me.btnCopyResponseToClipboard.Enabled = False
         Me.btnCopyResponseToClipboard.Location = New System.Drawing.Point(378, 634)
         Me.btnCopyResponseToClipboard.Name = "btnCopyResponseToClipboard"
         Me.btnCopyResponseToClipboard.Size = New System.Drawing.Size(180, 23)
         Me.btnCopyResponseToClipboard.TabIndex = 7
         Me.btnCopyResponseToClipboard.Text = "&Copy XML Response To Clipboard"
         Me.btnCopyResponseToClipboard.UseVisualStyleBackColor = True
         '
         'XMLScripter
         '
         Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
         Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
         Me.ClientSize = New System.Drawing.Size(564, 662)
         Me.Controls.Add(Me.btnCopyResponseToClipboard)
         Me.Controls.Add(Me.btnSaveXMLScript)
         Me.Controls.Add(Me.btnLoadXMLScript)
         Me.Controls.Add(Me.panel1)
         Me.Controls.Add(Me.btnRunXMLScript)
         Me.Controls.Add(Me.label3)
         Me.Controls.Add(Me.label1)
         Me.Controls.Add(Me.txtXMLScript)
         Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
         Me.MaximizeBox = False
         Me.Name = "XMLScripter"
         Me.ShowIcon = False
         Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
         Me.Text = "XML Scripter"
         Me.panel1.ResumeLayout(False)
         CType(Me.picUpdating, System.ComponentModel.ISupportInitialize).EndInit()
         Me.ResumeLayout(False)
         Me.PerformLayout()

      End Sub

#End Region

        Private txtXMLScript As System.Windows.Forms.TextBox
        Private label1 As System.Windows.Forms.Label
        Private label3 As System.Windows.Forms.Label
        Private WithEvents btnRunXMLScript As System.Windows.Forms.Button
        Private webXMLResponse As System.Windows.Forms.WebBrowser
        Private WithEvents backgroundWorker As System.ComponentModel.BackgroundWorker
        Private picUpdating As System.Windows.Forms.PictureBox
        Private panel1 As System.Windows.Forms.Panel
        Private WithEvents btnLoadXMLScript As System.Windows.Forms.Button
        Private WithEvents btnSaveXMLScript As System.Windows.Forms.Button
        Private WithEvents btnCopyResponseToClipboard As System.Windows.Forms.Button
    End Class
End Namespace

