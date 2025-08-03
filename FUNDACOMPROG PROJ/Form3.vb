Imports MySql.Data.MySqlClient

Public Class Form3
        Public ds As DataSet
        Private connectionString As String = "server=localhost;user id=root;password=;database=LoadLog;"

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ComboBox1.Items.Add("LOGIN INTERFACE")
        ComboBox1.Items.Add("ABOUT US")
        If ComboBox1.SelectedItem = "HOME" Then
            Form1.Show()
        ElseIf ComboBox1.SelectedItem = "LOG IN" Then
            Form2.Show()
        End If
    End Sub

End Class

