
Imports System.ComponentModel

Public Class Resonance
    Inherits Device
    Public Property Quality As Double
    Public Property Frequency As Double
    <Browsable(False)> Public Property Samplerate As Double
    <Browsable(False)> Public Property A0 As Double
    <Browsable(False)> Public Property A1 As Double
    <Browsable(False)> Public Property A2 As Double
    <Browsable(False)> Public Property B0 As Double
    <Browsable(False)> Public Property B1 As Double
    <Browsable(False)> Public Property B2 As Double
    <Browsable(False)> Public Property X1 As Double
    <Browsable(False)> Public Property X2 As Double
    <Browsable(False)> Public Property Y1 As Double
    <Browsable(False)> Public Property Y2 As Double

    Sub New(owner As Rack, frequency As Single, quality As Single)
        MyBase.New(owner)
        Me.Quality = quality
        Me.Frequency = frequency
        Me.Quality = quality
        Me.Reset()
    End Sub

    Public Overrides Sub Reset()
        Dim dsp As DSP = Nothing
        If (Me.Owner.GetDevice(GetType(DSP), dsp)) Then
            Me.Samplerate = dsp.Samplerate
        End If
        Dim w0 As Double = 2 * Math.PI * Me.Frequency / Me.Samplerate
        Dim alpha As Double = Math.Sin(w0) / (2 * Me.Quality)

        Me.X1 = 0
        Me.X2 = 0
        Me.Y1 = 0
        Me.Y2 = 0

        ' Adjust the coefficient calculations
        Me.B0 = alpha
        Me.B1 = 0
        Me.B2 = -alpha
        Me.A0 = 1 + alpha
        Me.A1 = -2 * Math.Cos(w0)
        Me.A2 = 1 - alpha

        ' Normalize coefficients
        Me.B0 /= Me.A0
        Me.B1 /= Me.A0
        Me.B2 /= Me.A0
        Me.A1 /= Me.A0
        Me.A2 /= Me.A0
    End Sub

    Public Overrides Sub Update(dt As Single)
        Dim offset As Double = Me.B0 * Me.Input + Me.B1 * Me.X1 + Me.B2 * Me.X2 - Me.A1 * Me.Y1 - Me.A2 * Me.Y2
        offset = Math.Max(Math.Min(offset, 1.0), -1.0)
        Me.X2 = Me.X1
        Me.X1 = Me.Input
        Me.Y2 = Me.Y1
        Me.Y1 = offset
        Me.Output = offset
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Cutoff    : {0:F2}Hz {1:F2}Qf", Me.Frequency, Me.Quality), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawString(String.Format("Coeff.    : {0:F2} {1:F2} {2:F2}", Me.A0, Me.A1, Me.A2), f, Brushes.Black, srcrect.X, srcrect.Y + 20)
        g.DrawString(String.Format("Output    : {0:F2}hz", Me.Output), f, Brushes.Black, srcrect.X, srcrect.Y + 30)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return String.Format("Resonance Filter")
        End Get
    End Property


End Class
