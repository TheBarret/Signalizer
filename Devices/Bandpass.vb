Imports System.ComponentModel

Public Class Bandpass
    Inherits Device
    Public Property Frequency As Single
    <Browsable(False)> Public Property Samplerate As Single
    <Browsable(False)> Public Property Alpha As Single

    Sub New(owner As Rack, frequency As Single)
        MyBase.New(owner)
        Me.Samplerate = 64
        Me.Frequency = frequency
        Me.Reset()
    End Sub

    Public Overrides Sub Reset()
        Dim dsp As DSP = Nothing
        If (Me.Owner.GetDevice(GetType(DSP), dsp)) Then
            Me.Samplerate = dsp.Samplerate
            Me.Alpha = 2 * Math.PI * Me.Frequency / dsp.Samplerate
        End If
    End Sub

    Public Overrides Sub Update(dt As Single)
        Dim result As Single = Me.Alpha * (Me.Input - Me.Output)
        result = Math.Max(Math.Min(result, 255), -255.0)
        Me.Output = result
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Cutoff    : {0:F2}Hz", Me.Alpha), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawString(String.Format("Output    : {0:F2}hz ", Me.Output), f, Brushes.Black, srcrect.X, srcrect.Y + 20)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return String.Format("Bandpass {0}Hz", Me.Frequency)
        End Get
    End Property
End Class
