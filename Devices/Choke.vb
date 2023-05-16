Public Class Choke
    Inherits Device
    Public Property Min As Single
    Public Property Max As Single
    Sub New(owner As Rack, min As Single, max As Single)
        MyBase.New(owner)
        Me.Min = min
        Me.Max = max
    End Sub

    Public Overrides Sub Update(dt As Single)
        Me.Output = Math.Max(Me.Min, Math.Min(Me.Max, Me.Input))
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Min Max   : ⋀({0},{1})", Me.Min, Me.Max), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawString(String.Format("Output    : {0:F2}hz ", Me.Output), f, Brushes.Black, srcrect.X, srcrect.Y + 20)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Choke"
        End Get
    End Property
End Class
