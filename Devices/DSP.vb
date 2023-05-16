
Imports System.ComponentModel
Imports System.Timers

Public Class DSP
    Inherits Device
    Enum Wave
        Sinus
        Squared
        StepPulse
        Noise
    End Enum
    Public Property Gain As Single
    Public Property Phase As Single
    Public Property Amplitude As Single
    Public Property Frequency As Single
    Public Property Samplerate As Integer
    Public Property Waveformat As Wave
    Public Property AutoReset As Boolean
    <Browsable(False)> Public Property Buffer As Single()
    <Browsable(False)> Public Property Clock As Timers.Timer
    <Browsable(False)> Public Property Frame As Integer

    Sub New(owner As Rack, samplerate As Integer, frequency As Single, amplitude As Single, phase As Single, gain As Single, format As Wave)
        MyBase.New(owner)
        Me.Lock = New Object
        Me.Frame = 0
        Me.Phase = phase
        Me.Amplitude = amplitude
        Me.AutoReset = True
        Me.Gain = gain
        Me.Samplerate = samplerate
        Me.Frequency = frequency
        Me.Waveformat = format
        Me.Clock = New Timers.Timer
        Me.Reset()
    End Sub

    Public Overrides Sub Reset()
        SyncLock Me.Lock
            Me.Clock.Stop()
            Me.Frame = 0
            Me.Buffer = New Single(Me.Samplerate - 1) {}
            Me.Clock.Interval = 1000.0F / Me.Samplerate
            AddHandler Me.Clock.Elapsed, AddressOf Me.Elapsed
            Me.Clock.Start()
        End SyncLock
    End Sub

    Private Sub Elapsed(sender As Object, e As ElapsedEventArgs)
        Me.Buffer = Me.Generate
        Me.Clock.Stop()
    End Sub

    Private Function Generate() As Single()
        Select Case Me.Waveformat
            Case Wave.Sinus
                ' Generate sinusoidal signal
                For i As Integer = 0 To Me.Samplerate - 1
                    Dim t As Single = i / Me.Samplerate
                    Me.Buffer(i) = Me.Gain * Me.Amplitude * CSng(Math.Sin(2 * Math.PI * Me.Frequency * t + Me.Phase))
                Next i

            Case Wave.Squared
                ' Generate square wave signal
                For i As Integer = 0 To Me.Samplerate - 1
                    Dim t As Single = i / Me.Samplerate
                    Me.Buffer(i) = Me.Gain * Me.Amplitude * If(Math.Sin(2 * Math.PI * Me.Frequency * t + Me.Phase) >= 0, 1.0F, -1.0F)
                Next i

            Case Wave.StepPulse
                ' Generate step pulse signal
                For i As Integer = 0 To Me.Samplerate - 1
                    Dim t As Single = i / Me.Samplerate
                    Me.Buffer(i) = Me.Gain * Me.Amplitude * If(t < 0.5F / Me.Frequency, 1.0F, 0.0F)
                Next i

            Case Wave.Noise
                ' Generate white noise signal
                For i As Integer = 0 To Me.Samplerate - 1
                    Me.Buffer(i) = Me.Gain * Me.Amplitude * (2 * CSng(DSP.RandomRangedFloat(-Me.Frequency, Me.Frequency)) - 1)
                Next i
        End Select
        Return Me.Buffer
    End Function

    Public Overrides Sub Update(dt As Single)
        If (Me.Buffer.Count > 0) Then
            If (Not Me.Frame < Me.Buffer.Count) Then
                If (Me.AutoReset) Then
                    Me.Owner.ResetAll()
                End If
                Return
            End If
            Me.Output = Me.Buffer(Me.Frame)
            Me.Frame += 1
            If (Me.Frame >= Me.Samplerate) Then Me.Frame = 0
        End If
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Frequency : {0:F2}hz ", Me.Frequency), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawString(String.Format("Output    : {0:F2}hz ", Me.Output), f, Brushes.Black, srcrect.X, srcrect.Y + 20)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Shared ReadOnly Property Randomizer As Random
        Get
            Static r As New Random(DateTime.Now.Millisecond)
            Return r
        End Get
    End Property

    Public Shared Function RandomRangedFloat(min As Single, max As Single) As Single
        Return CSng((max - min) * DSP.Randomizer.NextDouble) + min
    End Function


    Public Overrides ReadOnly Property Name As String
        Get
            Return "Digital Signal Processor"
        End Get
    End Property
End Class
