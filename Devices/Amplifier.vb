
Public Class Amplifer
    Inherits Device
    Public Property Gain As Single

    Sub New(owner As Rack, gain As Single)
        MyBase.New(owner)
        Me.Gain = gain
    End Sub

    Public Overrides Sub Update(dt As Single)
        Me.Output = Me.Input * Me.Gain
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Gain      : {0:F2}", Me.Gain), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawString(String.Format("Output    : {0:F2}hz ", Me.Output), f, Brushes.Black, srcrect.X, srcrect.Y + 20)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Amplifier"
        End Get
    End Property
End Class
