Public Class Editor
    Public Property Selected As Device
    Private Sub Editor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize UI
        Me.Hardware.Initialize()
        Me.Hardware.Add(New DSP(Me.Hardware, 128, 1.0F, 5.0F, 0F, 2.0F, DSP.Wave.Sinus))
        Me.Hardware.Add(New Monitor(Me.Hardware))
        Me.Hardware.Add(New Coil(Me.Hardware, 0.1F))
        Me.Hardware.Add(New Bandpass(Me.Hardware, 15.0F))
        Me.Hardware.Add(New Resonance(Me.Hardware, 80.0F, 2.9))
        Me.Hardware.Add(New Amplifer(Me.Hardware, 35.0F))
        Me.Hardware.Add(New Monitor(Me.Hardware))
        Me.Hardware.Add(New Speaker(Me.Hardware))

        Me.cbDevices.Items.Add("DSP")
        Me.cbDevices.Items.Add("Amplifier")
        Me.cbDevices.Items.Add("Choke")
        Me.cbDevices.Items.Add("Coil")
        Me.cbDevices.Items.Add("Resonance Filter")
        Me.cbDevices.Items.Add("Badpass Filter")
        Me.cbDevices.SelectedIndex = 0
    End Sub

    Private Sub Hardware_MouseDown(sender As Object, e As MouseEventArgs) Handles Hardware.MouseDown
        Dim device As Device = Nothing
        If (Me.Hardware.GetDevice(e.Location, device)) Then
            Me.Selected = device
            Me.pEditor.SelectedObject = device
        End If
    End Sub

    Private Sub pEditor_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pEditor.PropertyValueChanged
        If (Me.Selected IsNot Nothing) Then
            Me.Hardware.ResetAll()
        End If
    End Sub
End Class
