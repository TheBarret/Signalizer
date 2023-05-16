Imports System.Timers
Imports System.Drawing.Drawing2D

Public Class Rack
    Inherits Panel
    Const Maximum As Integer = 8
    Const Speed As Integer = 16
    Public Property Frame As Double
    Public Property Lock As Object
    Public Property Bus As Timer
    Public Property Timestep As Single
    Public Property Redraw As Boolean
    Public Property Devices As List(Of Device)

    Sub New()
        Me.Redraw = True
        Me.DoubleBuffered = True
        Me.Lock = New Object
        Me.BorderStyle = BorderStyle.Fixed3D
    End Sub

    Public Sub Initialize()
        Me.Devices = New List(Of Device)
        Me.Bus = New Timer
        Me.Bus.Interval = Rack.Speed
        Me.Frame = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond
        AddHandler Me.Bus.Elapsed, AddressOf Me.Tick
        Me.Bus.Start()
    End Sub

    Private Sub Tick(sender As Object, e As ElapsedEventArgs)
        Dim current As Double = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond
        Me.Timestep = CSng((current - Me.Frame) / 1000)
        If (Me.Devices.Count > 0) Then
            Me.Devices.First.Update(Me.Timestep)
            SyncLock Me.Lock
                Me.Resync()
            End SyncLock
        End If
        Me.Frame = current
    End Sub

    Public Sub Add(dev As Device)
        If Me.Devices.Count < Rack.Maximum Then
            SyncLock Me.Lock
                Me.Devices.Add(dev)
                AddHandler dev.OutputChanged, AddressOf Me.HandleOutput
                Me.UpdateDevices()
                Me.Resync()
            End SyncLock
        End If
    End Sub

    Public Sub ResetAll()
        For Each dev As Device In Me.Devices
            dev.Reset()
        Next
    End Sub

    Public Function GetDevice(device As Type, ByRef result As Device) As Boolean
        For Each dev As Device In Me.Devices
            If (TypeOf dev Is Device) Then
                result = dev
                Return True
            End If
        Next
        Return False
    End Function

    Public Function GetDevice(position As Point, ByRef result As Device) As Boolean
        Dim base As Rectangle = Me.ClientRectangle
        Dim width As Integer = base.Width
        Dim height As Integer = base.Height \ Rack.Maximum
        For i As Integer = 0 To Rack.Maximum - 1
            Dim srcrect As New Rectangle(5, 5 + i * height, width - 12, height - 5)
            If (srcrect.Contains(position)) Then
                result = Me.Devices(i)
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub Resync()
        Me.Redraw = True
        Me.Invalidate()
    End Sub

    Private Sub HandleOutput(sender As Object, output As Single)
        SyncLock Me.Lock
            Dim index As Integer = Me.Devices.IndexOf(DirectCast(sender, Device))
            If index >= 0 AndAlso index < Me.Devices.Count - 1 Then
                Me.Devices(index + 1).Input = output
                Me.Devices(index + 1).Update(Me.Timestep)
            End If
        End SyncLock
    End Sub

    Private Sub UpdateDevices()
        For i As Integer = 0 To Me.Devices.Count - 1
            If i < Me.Devices.Count - 1 Then
                AddHandler Me.Devices(i).OutputChanged, AddressOf Me.HandleOutput
            Else
                RemoveHandler Me.Devices(i).OutputChanged, AddressOf Me.HandleOutput
            End If
        Next
    End Sub

    Protected Overrides Sub OnResize(eventargs As EventArgs)
        Me.Width = 256
        Me.Height = 512
        Me.Resync()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If (Me.Redraw) Then
            Dim base As Rectangle = Me.ClientRectangle
            Using fnt As New Font("consolas", 8)
                Using bm As New Bitmap(Me.ClientRectangle.Width, Me.ClientRectangle.Height)
                    Using g As Graphics = Graphics.FromImage(bm)
                        g.SmoothingMode = SmoothingMode.AntiAlias
                        g.Clear(Color.FromKnownColor(KnownColor.ControlDark))
                        Dim swidth As Integer = base.Width
                        Dim sheight As Integer = base.Height \ Rack.Maximum
                        For i As Integer = 0 To Rack.Maximum - 1
                            Dim srcrect As New Rectangle(5, 5 + i * sheight, swidth - 12, sheight - 5)
                            Using sf As New StringFormat
                                If i < Me.Devices.Count Then
                                    Me.Devices(i).Draw(srcrect, fnt, g)
                                Else
                                    g.FillRectangle(Brushes.LightGray, srcrect)
                                    g.DrawString(String.Format("Device #{0}", i), fnt, Brushes.Black, srcrect.X, srcrect.Y)
                                    g.DrawRectangle(Pens.Black, srcrect)
                                End If
                            End Using
                        Next
                    End Using
                    Me.BackgroundImage = CType(bm.Clone, Image)
                End Using
            End Using
            Me.Redraw = False
        End If
    End Sub

End Class
