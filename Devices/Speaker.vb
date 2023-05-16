Imports NAudio.Wave

Public Class Speaker
    Inherits Device
    Public Property Samplerate As Single
    Public Property Buffer As List(Of Single)
    Public Property Device As WaveOutEvent
    Public Property Provider As BufferedWaveProvider
    Sub New(owner As Rack)
        MyBase.New(owner)
        Me.Device = New WaveOutEvent
        Me.Provider = New BufferedWaveProvider(New WaveFormat)
        Me.Device.Init(Me.Provider)
        Me.Buffer = New List(Of Single)
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
        SyncLock Me.Lock
            Me.Buffer.Add(CSng(Me.Output))
            If Me.Buffer.Count > Me.Samplerate Then
                Me.Buffer.RemoveAt(0)
            End If
            If (Me.Buffer.Count = Me.Samplerate) Then
                For Each sample As Single In Me.Buffer
                    Dim offset As Byte = CShort(Math.Max(0, Math.Min(255, sample)))
                    Me.Provider.AddSamples(New Byte() {BitConverter.GetBytes(offset).First}, 0, 1)
                Next
                Me.Device.Play()
                Me.Buffer.Clear()
            End If
        End SyncLock
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Output    : {0} Samples", Me.Samplerate), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub


    Public Overrides ReadOnly Property Name As String
        Get
            Return "Speaker"
        End Get
    End Property
End Class
