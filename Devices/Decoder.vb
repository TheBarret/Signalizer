Imports System.ComponentModel

Public Class Decoder
    Inherits Device
    <Browsable(False)> Public Property Samplerate As Single
    <Browsable(False)> Public Property Buffer As List(Of Single)

    Sub New(owner As Rack)
        MyBase.New(owner)
        Me.Lock = New Object
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
        End SyncLock
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        Dim message As String = Me.Decode(Me.Buffer.ToArray)
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Output    : {0}", message.Substring(0, Math.Min(20, message.Length))), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Function Decode(bits() As Single) As String
        If (bits.Length <> Me.Samplerate) Then Return String.Empty
        Dim numChars As Integer = bits.Length \ 8
        Dim message(numChars - 1) As Char
        For i As Integer = 0 To numChars - 1
            Dim charBits(7) As Single
            Array.Copy(bits, i * 8, charBits, 0, 8)
            message(i) = Strings.ChrW(Decoder.ToByte(Decoder.Level(charBits)))
        Next
        Return New String(message)
    End Function

    Private Shared Function Level(values() As Single) As Integer()
        Return values.Select(Function(x) If(x >= 0, 1, 0)).Cast(Of Integer).ToArray
    End Function

    Private Shared Function ToByte(bits() As Integer) As Byte
        Dim value As Byte = 0
        For i As Integer = 0 To 7
            If bits(i) Then
                value = value Or (1 << (7 - i))
            End If
        Next
        Return value
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Binary Decoder"
        End Get
    End Property
End Class