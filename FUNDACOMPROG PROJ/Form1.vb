
Imports MySql.Data.MySqlClient
Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Admin.Show()
        Me.Hide()
        Form3.Show()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ComboBox1.Items.Add("LOGIN INTERFACE")
        ComboBox1.Items.Add("ABOUT US")
        If ComboBox1.SelectedItem = "LOGIN INTERFACE" Then
            Me.Show()
        ElseIf ComboBox1.SelectedItem = "ABOUT US" Then
            Form3.Show()
        End If
    End Sub
End Class

