<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Editor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Editor))
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.cbDevices = New System.Windows.Forms.ComboBox()
        Me.pEditor = New System.Windows.Forms.PropertyGrid()
        Me.Hardware = New Signalizer.Rack()
        Me.SuspendLayout()
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(239, 12)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(29, 21)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "+"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'cbDevices
        '
        Me.cbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDevices.FormattingEnabled = True
        Me.cbDevices.Location = New System.Drawing.Point(12, 13)
        Me.cbDevices.Name = "cbDevices"
        Me.cbDevices.Size = New System.Drawing.Size(256, 21)
        Me.cbDevices.TabIndex = 3
        '
        'pEditor
        '
        Me.pEditor.Location = New System.Drawing.Point(735, 12)
        Me.pEditor.Name = "pEditor"
        Me.pEditor.Size = New System.Drawing.Size(260, 536)
        Me.pEditor.TabIndex = 5
        '
        'Hardware
        '
        Me.Hardware.BackgroundImage = CType(resources.GetObject("Hardware.BackgroundImage"), System.Drawing.Image)
        Me.Hardware.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Hardware.Bus = Nothing
        Me.Hardware.Devices = Nothing
        Me.Hardware.Frame = 0R
        Me.Hardware.Location = New System.Drawing.Point(12, 40)
        Me.Hardware.Lock = CType(resources.GetObject("Hardware.Lock"), Object)
        Me.Hardware.Name = "Hardware"
        Me.Hardware.Redraw = False
        Me.Hardware.Size = New System.Drawing.Size(256, 512)
        Me.Hardware.TabIndex = 4
        Me.Hardware.Timestep = 0!
        '
        'Editor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1007, 561)
        Me.Controls.Add(Me.pEditor)
        Me.Controls.Add(Me.Hardware)
        Me.Controls.Add(Me.cbDevices)
        Me.Controls.Add(Me.btnAdd)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Editor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Signalizer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAdd As Button
    Friend WithEvents Hardware As Rack
    Friend WithEvents cbDevices As ComboBox
    Friend WithEvents pEditor As PropertyGrid
End Class
