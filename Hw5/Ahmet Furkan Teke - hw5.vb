Imports System
Imports System.IO

Public Class Form1

    Private Sub UnsplitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnsplitToolStripMenuItem.Click
        Panel1.Hide()
        Panel2.Dock = DockStyle.Fill

    End Sub

    Private Sub SplitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SplitToolStripMenuItem.Click
        Panel1.Show()
        Panel2.Dock = DockStyle.None
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Using sr As New StreamReader(CurDir() & "\" & "input.txt")
            Dim line As String

            With DataGridView1
                .ColumnCount = 10
                .Columns(0).Name = "Name"
                .Columns(1).Name = "var-1"
                .Columns(2).Name = "var-2"
                .Columns(3).Name = "var-3"
                .Columns(4).Name = "var-4"
                .Columns(5).Name = "var-5"
                .Columns(6).Name = "var-6"
                .Columns(7).Name = "var-7"
                .Columns(8).Name = "var-8"
                .Columns(9).Name = "var-9"
            End With

            With DataGridView2
                .ColumnCount = 10
                .Columns(0).Name = "Name"
                .Columns(1).Name = "var-1"
                .Columns(2).Name = "var-2"
                .Columns(3).Name = "var-3"
                .Columns(4).Name = "var-4"
                .Columns(5).Name = "var-5"
                .Columns(6).Name = "var-6"
                .Columns(7).Name = "var-7"
                .Columns(8).Name = "var-8"
                .Columns(9).Name = "var-9"
            End With

            line = sr.ReadToEnd()
            Dim entities As String() = line.Split(New Char() {";"c})


            For Each entity In entities
                entity = entity.Replace("""", "")
                Dim variables As String() = entity.Split(New Char() {","c})
                Dim counter As Integer = 0
                For Each cell In variables
                    If cell.Length > 15 Then
                        cell = cell.Substring(0, 15)
                    End If
                    variables(counter) = cell
                    counter += 1
                Next
                DataGridView1.Rows.Add(variables)
                DataGridView2.Rows.Add(variables)
            Next

        End Using
    End Sub
End Class
