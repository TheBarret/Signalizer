Public Class Coil
    Inherits Device
    Public Property Inductance As Single
    Sub New(owner As Rack, inductance As Single)
        MyBase.New(owner)
        Me.Inductance = inductance
    End Sub

    Public Overrides Sub Update(dt As Single)
        If (Me.Inductance = 0) Then Return
        Me.Output += (Me.Input - Me.Output * Me.Inductance) / Me.Inductance
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Overrides Sub Draw(srcrect As Rectangle, f As Font, g As Graphics)
        Dim half As Single = srcrect.Height \ 2
        g.FillRectangle(Brushes.LightGray, srcrect)
        g.DrawString(String.Format("Device    : {0}", Me.Name), f, Brushes.Black, srcrect.X, srcrect.Y)
        g.DrawString(String.Format("Inductance: {0:F2}", Me.Inductance), f, Brushes.Black, srcrect.X, srcrect.Y + 10)
        g.DrawString(String.Format("Output    : {0:F2}hz", Me.Output), f, Brushes.Black, srcrect.X, srcrect.Y + 20)
        g.DrawRectangle(Pens.Black, srcrect)
        Me.Graph.Draw(New Rectangle(srcrect.X, srcrect.Y + half, srcrect.Width, srcrect.Height - half), g)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Coil"
        End Get
    End Property
End Class