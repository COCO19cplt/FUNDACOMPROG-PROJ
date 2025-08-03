Imports MySql.Data.MySqlClient

Public Class Form17
    Dim conn As New MySqlConnection("server=localhost;user id=root;password=;database=LoadLog;")
    Dim query As String
    Dim cmd As MySqlCommand

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = TextBox1.Text.Trim().Replace("'", "''")
        Dim password As String = TextBox2.Text.Trim().Replace("'", "''")
        Dim role As String = ""
        If ComboBox1.SelectedItem IsNot Nothing Then
            role = ComboBox1.SelectedItem.ToString().ToUpper
        End If

        ' Manual check for missing fields
        If username = "" Then
            MessageBox.Show("Username is required.")
            Exit Sub
        End If
        If password = "" Then
            MessageBox.Show("Password is required.")
            Exit Sub
        End If
        If role = "" Then
            MessageBox.Show("Role is required.")
            Exit Sub
        End If

        Try
            conn.Open()

            ' Manual scan for similar email (example: scanning for existing username)
            query = "SELECT Username FROM users WHERE Username='" & username & "'"
            cmd = New MySqlCommand(query, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            Dim found As Boolean = False
            While reader.Read()
                If reader("Username").ToString().Equals(username, StringComparison.OrdinalIgnoreCase) Then
                    found = True
                    Exit While
                End If
            End While
            reader.Close()
            If found Then
                MessageBox.Show("Username already exists. Please choose another.")
                Exit Sub
            End If

            ' Insert user
            query = "INSERT INTO users (Username, Password, Role) VALUES ('" & username & "', '" & password & "', '" & role & "')"
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Account created successfully! You may now log in.")
            Me.Close()
            Form2.Show()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Form17_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("CUSTOMER")
        ComboBox1.Items.Add("ATTENDANT")
        ComboBox1.Items.Add("MANAGER")
        ComboBox1.SelectedIndex = 0
    End Sub
End Class