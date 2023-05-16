Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Monitor
    Inherits Device
    Public Property Warm As Color
    Public Property Cold As Color
    Public Property Interpolate As Boolean
    <Browsable(False)> Public Property Samplerate As Single
    <Browsable(False)> Public Property buffer As List(Of Single)

    Sub New(owner As Rack)
        MyBase.New(owner)
        Me.Warm = Color.Red
        Me.Cold = Color.Blue
        Me.Interpolate = False
        Me.buffer = New List(Of Single)
        Me.Reset()
    End Sub

    Public Overrides Sub Reset()
        Dim dsp As DSP = Nothing
        If (Me.Owner.GetDevice(GetType(DSP), dsp)) Then
            Me.Samplerate = dsp.Samplerate
        End If
    End Sub

    Public Overrides Sub Update(dt As Single)
        Me.Output = Me.Input
        Me.buffer.Add(CSng(Me.Output))
        If Me.buffer.Count > Me.Samplerate Then Me.buffer.RemoveAt(0)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        If (Me.buffer.Count < Me.Samplerate) Then
            g.FillRectangle(Brushes.Black, srcrect)
            g.DrawString(String.Format("{0} Loading...", Me.buffer.Count), f, Brushes.White, srcrect.X, srcrect.Y)
            g.DrawRectangle(Pens.Gray, srcrect)
            Return
        End If
        Dim baseHeight As Single = srcrect.Height
        Dim baseWidth As Single = srcrect.Width
        Dim count As Integer = Me.buffer.Count
        g.FillRectangle(Brushes.Black, srcrect)

        Using pen As New Pen(If(Me.Interpolate, Me.Intp(Me.Cold, Me.Warm, Me.Output), Me.Warm))
            Dim graphRect As New Rectangle(srcrect.X, srcrect.Y, srcrect.Width, srcrect.Height - 30)
            If count > 1 Then
                Dim graphWidth As Single = graphRect.Width
                Dim graphHeight As Single = graphRect.Height
                Dim xStep As Single = graphWidth / (count - 1)
                Dim yScale As Single = graphHeight * 0.1F
                Using path As New GraphicsPath
                    For i As Integer = 0 To count - 1
                        Try
                            Dim x As Single = graphRect.Left + i * xStep
                            Dim y As Single = graphRect.Bottom - Me.buffer(i) * yScale
                            path.AddLine(x, graphRect.Bottom, x, y)
                        Catch ex As Exception
                            'TODO: Fix this
                        End Try
                    Next
                    g.Clip = New Region(srcrect)
                    g.DrawPath(pen, path)
                    g.ResetClip()
                End Using
            End If
            g.DrawString(String.Format("{0} / {1} Samples", Me.Name, Me.Samplerate), f, Brushes.White, srcrect.X, srcrect.Y)
            g.DrawRectangle(Pens.Gray, srcrect)
        End Using
    End Sub

    Public Function Intp(c1 As Color, c2 As Color, value As Single) As Color
        If (Single.IsNaN(value) Or Single.IsInfinity(value)) Then Return c1
        value = Math.Max(-1.0F, Math.Min(1.0F, value))
        Dim t As Single = (value + 1.0F) / 2.0F
        Dim r As Integer = CInt(CInt(c1.R) + (CInt(c2.R) - CInt(c1.R)) * t)
        Dim g As Integer = CInt(CInt(c1.G) + (CInt(c2.G) - CInt(c1.G)) * t)
        Dim b As Integer = CInt(CInt(c1.B) + (CInt(c2.B) - CInt(c1.B)) * t)
        r = Math.Max(0, Math.Min(255, r))
        g = Math.Max(0, Math.Min(255, g))
        b = Math.Max(0, Math.Min(255, b))
        Return Color.FromArgb(r, g, b)
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Monitor"
        End Get
    End Property
End Class
