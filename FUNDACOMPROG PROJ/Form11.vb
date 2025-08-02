Imports MySql.Data.MySqlClient
Public Class Form11
    Private RefreshData()
    ' 1. Declare conn properly
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"
    Private conn As New MySqlConnection(connectionString)
    Private cmd As MySqlCommand
    Private da As MySqlDataAdapter
    Private ds As DataSet
    Private query As String

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form12.Show()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Form11_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using tempConn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM order_items"
                Dim dataAdapter As New MySqlDataAdapter(sql, tempConn)
                ds = New DataSet()
                dataAdapter.Fill(ds, "order_items")
                DataGridView1.DataSource = ds.Tables("order_items")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Me.Show()
    End Sub
End Class
