Imports Microsoft.VisualBasic.PowerPacks

Public Class hw4_ahmetfteke
    Dim move_right As Boolean
    Dim move_top As Boolean
    Dim speed As Integer
    Dim x_speed As Integer
    Dim y_speed As Integer
    Dim angle As Double
    Dim canvas As New ShapeContainer
    Dim theShape As New OvalShape

    Private Function DrawBall()
        canvas.Parent = Me
        theShape.Parent = canvas
        theShape.FillStyle = FillStyle.Solid
        theShape.FillColor = Color.Blue
        theShape.Size = New System.Drawing.Size(24, 24)
        theShape.Location = New System.Drawing.Point((ClientSize.Width - 24) \ 2, (ClientSize.Height - 24) \ 2)
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If (theShape.Left - x_speed) <= 0 Then
            move_right = True
        End If
        If (theShape.Right + x_speed) >= Me.ClientRectangle.Right Then
            move_right = False
        End If
        If (theShape.Top - y_speed) <= 0 Then
            move_top = False
        End If
        If (theShape.Bottom + y_speed) >= Me.ClientRectangle.Bottom Then
            move_top = True
        End If
        If move_right = False Then
            theShape.Left -= x_speed
        Else
            theShape.Left += x_speed
        End If
        If move_top = False Then
            theShape.Top += y_speed
        Else
            theShape.Top -= y_speed
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DrawBall()
        Timer1.Stop()
        TrackBar1.Value = 10
        Label3.Text = "10"
        TrackBar2.Value = 45
        Label4.Text = "45"
        speed = TrackBar1.Value
        x_speed = Math.Cos(Math.PI * angle / 180.0) * ((2 * speed) ^ (0.5))
        y_speed = Math.Sin(Math.PI * angle / 180.0) * ((2 * speed) ^ (0.5))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Start()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer1.Stop()
    End Sub

    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        speed = TrackBar1.Value
        Label3.Text = TrackBar1.Value
        x_speed = Math.Cos(Math.PI * angle / 180.0) * ((2 * speed) ^ (0.5))
        y_speed = Math.Sin(Math.PI * angle / 180.0) * ((2 * speed) ^ (0.5))
    End Sub
    Private Sub TrackBar2_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar2.ValueChanged
        angle = TrackBar2.Value
        Label4.Text = TrackBar2.Value
        x_speed = Math.Cos(Math.PI * angle / 180.0) * ((2 * speed) ^ (0.5))
        y_speed = Math.Sin(Math.PI * angle / 180.0) * ((2 * speed) ^ (0.5))
    End Sub

End Class
