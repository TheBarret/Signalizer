Imports System.ComponentModel

<Serializable> Public MustInherit Class Device
    <Browsable(False)> Public Property Owner As Rack
    <Browsable(False)> Public Property Graph As Graph
    <Browsable(False)> Public Property Lock As Object
    <Browsable(False)> Public Overridable Property Input As Single
    <Browsable(False)> Public Overridable Property Output As Single
    <Browsable(False)> Public MustOverride ReadOnly Property Name As String
    Public MustOverride Sub Draw(base As Rectangle, f As Font, g As Graphics)
    Public Event OutputChanged As EventHandler(Of Single)

    Sub New(owner As Rack)
        Me.Owner = owner
        Me.Graph = New Graph
        Me.Input = 0F
        Me.Output = 0F
        Me.Lock = New Object
    End Sub

    Public Overridable Sub Reset()

    End Sub

    Public Overridable Sub Update(dt As Single)
        Me.Output = Me.Input
        Me.Graph.Add(Me.Output)
        Me.CallEvent(Me, Me.Output)
    End Sub

    Public Sub CallEvent(sender As Device, value As Single)
        RaiseEvent OutputChanged(sender, value)
    End Sub


End Class

