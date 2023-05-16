Public Class Graph
    Public Const Max As Integer = 64
    Public Property Lock As Object
    Public Property Samples As List(Of Single)

    Sub New()
        Me.Lock = New Object
        Me.Samples = New List(Of Single)
    End Sub
    Public Sub Add(value As Single)
        SyncLock Me.Lock
            If (Me.Samples.Count = Graph.Max) Then
                Me.Samples.RemoveAt(0)
            End If
            Me.Samples.Add(value)
        End SyncLock
    End Sub

    Public Sub Draw(bounds As Rectangle, g As Graphics)
        'Using p As New Pen(Color.Red, 1) With {.DashStyle = Drawing2D.DashStyle.Dot}
        '    Dim count As Integer = Me.Samples.Count
        '    Dim scalex As Single = bounds.Width / Graph.Max
        '    Dim scaley As Single = bounds.Height / Me.GetYRange(bounds)
        '    g.DrawLine(Pens.Gray, bounds.Left, bounds.Top + bounds.Height \ 2, bounds.Right, bounds.Top + bounds.Height \ 2)
        '    If count > 1 Then
        '        Dim points As New List(Of PointF)(count)
        '        For i As Integer = 0 To count - 1
        '            Dim x As Single = bounds.Left + i * scalex
        '            Dim y As Single = bounds.Top + bounds.Height \ 2 - Me.Samples(i) * scaley
        '            points.Add(New PointF(x, y))
        '        Next
        '        g.DrawLines(p, points.ToArray)
        '    End If
        'End Using
    End Sub

    Private Function GetYRange(bounds As Rectangle) As Single
        Dim range As Single = 0.0F
        Dim buffer() As Single = New Single(Me.Samples.Count - 1) {}
        Me.Samples.CopyTo(buffer)
        For Each sample As Single In buffer
            Dim value As Single = Math.Abs(sample)
            If value > range Then range = value
        Next
        Return If(range > 0.0F, range * 5.0F, bounds.Height / 2)
    End Function
End Class
