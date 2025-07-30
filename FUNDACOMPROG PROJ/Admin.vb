Imports MySql.Data.MySqlClient

Public Class Admin
    Public ds As DataSet
    Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Using conn As New MySqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM customers"
                Dim dataAdapter1 As New MySqlDataAdapter(sql, conn)
                ds = New DataSet()
                dataAdapter1.Fill(ds, "customers")
                DataGridView1.DataSource = ds.Tables("customers")
            End Using
        Catch ex As Exception
            MsgBox("Error in collecting data from Database. Error is :" & ex.Message)
        End Try
    End Sub

End Class

